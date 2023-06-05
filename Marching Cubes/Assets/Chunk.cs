using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
public class Chunk  {

    bool smoothTerrain;
    bool flatShaded;

   


    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private MeshFilter _meshFilter;
    private int _configIndex = -1;
    private float[,,] terrainMap;
    private MeshCollider _meshCollider;
    public GameObject chunkObject;
    private MeshRenderer _meshRenderer;
    private Vector3Int chunkPosition;
    
    int width { get { return GameData.chunkWidth; } }
    int height { get { return GameData.chunkHeight; } }
    float terrainSurface { get { return GameData.terrainSurface; } }

    void ClearMeshData() {
        vertices.Clear();
        triangles.Clear();
    }
    public void PlaceTerrain(Vector3 pos) {
        Vector3Int v3int = new Vector3Int(Mathf.CeilToInt(pos.x), Mathf.CeilToInt(pos.y), Mathf.CeilToInt(pos.z));
        v3int -= chunkPosition;
        terrainMap[v3int.x, v3int.y, v3int.z] = 0;
        CreateMeshData();
    }
    public void RemoveTerrain(Vector3 pos) {
        Vector3Int v3int = new Vector3Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), Mathf.FloorToInt(pos.z));
        v3int -= chunkPosition;
        terrainMap[v3int.x, v3int.y, v3int.z] = 0;
        CreateMeshData();
    }
    
    void BuildMesh() {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        _meshFilter.sharedMesh = mesh;
        _meshCollider.sharedMesh = mesh;
    }

    public  Chunk(Vector3Int pos,bool flatShaded , bool smoothTerrain) {
        this.flatShaded = flatShaded;
        this.smoothTerrain = smoothTerrain;
        chunkObject = new GameObject();
        chunkPosition = pos;
        chunkObject.transform.position = chunkPosition;
        _meshFilter = chunkObject.AddComponent<MeshFilter>();
        _meshCollider = chunkObject.AddComponent<MeshCollider>();
        _meshRenderer = chunkObject.AddComponent<MeshRenderer>();
        chunkObject.transform.tag = "Terrain";
        chunkObject.name = $"Chunk {pos.x},{pos.z}";
        _meshRenderer.sharedMaterial = Resources.Load<Material>("Material/Terrain");
        terrainMap = new float[width + 1, height + 1, width + 1];
        PopulateTerrainMap();
        CreateMeshData();
    }

    void CreateMeshData() {
        ClearMeshData();
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                for (int z = 0; z < width; z++) {
                    MarchCube(new Vector3Int(x, y, z));
                }
            }
        }
        BuildMesh();
    }

    void PopulateTerrainMap() {
        for (int x = 0; x < width + 1; x++) {
            for (int y = 0; y < height + 1; y++) {
                for (int z = 0; z < width + 1; z++) {
                    float thisHeight = GameData.GetTerrainHeight(x + chunkPosition.x,z + chunkPosition.z);
                    terrainMap[x, y, z] = (float)y - thisHeight;
                }
            }
        }
    }

    int GetCubeConfiguration(float[] cube) {
        int configurationIndex = 0;
        int cornerCount = 8;
        for (int i = 0; i < cornerCount; i++) {
            if (cube[i] > terrainSurface) {
                configurationIndex |= 1 << i;
            }
        }
        return configurationIndex;
    }
    void MarchCube(Vector3Int position) {
        float[] cube = new float[8];
        for (int i = 0; i < cube.Length; i++) {
            cube[i] = SampleTerrain(position + GameData.CornerTable[i]);
        }
        int configIndex = GetCubeConfiguration(cube);
        if (configIndex == 0 || configIndex == 255)
            return;

        int edgeIndex = 0;
        int maxEdgeCount = 5; // There is no more than 5 edges inside triangulation table
        int maxPoint = 3; // There is no more than 3 points in any triangle
        for (int i = 0; i < maxEdgeCount; i++) {
            for (int j = 0; j < maxPoint; j++) {
                int indice = GameData.TriangleTable[configIndex, edgeIndex];
                if (indice == -1) {
                    return;
                }
                Vector3 vert1 = position + GameData.CornerTable[GameData.EdgeIndexes[indice, 0]];
                Vector3 vert2 = position + GameData.CornerTable[GameData.EdgeIndexes[indice, 1]];
                Vector3 vertPosition;
                if (smoothTerrain) {
                    float vert1Sample = cube[GameData.EdgeIndexes[indice, 0]];
                    float vert2Sample = cube[GameData.EdgeIndexes[indice, 1]];

                    float difference = vert2Sample - vert1Sample;
                    if (difference == 0) difference = terrainSurface;
                    else difference = (terrainSurface - vert1Sample) / difference;
                    vertPosition = vert1 + ((vert2 - vert1)) * difference;
                }
                else {
                    vertPosition = (vert1 + vert2) / 2f;
                }
                if (flatShaded) {
                    vertices.Add(vertPosition);
                    triangles.Add(vertices.Count - 1);
                }
                else {
                    triangles.Add(VertForIndice(vertPosition));
                }
                edgeIndex++;

            }
        }
    }

    float SampleTerrain(Vector3Int point) {
        return terrainMap[point.x, point.y, point.z];
    }

    int VertForIndice(Vector3 vert) {
        for (int i = 0; i < vertices.Count; i++) {
            if (vertices[i] == vert) {
                return i;
            }
        }
        vertices.Add(vert);
        return vertices.Count - 1;
    }

}