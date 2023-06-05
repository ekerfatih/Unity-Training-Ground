using System;
using System.CodeDom.Compiler;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer), (typeof(MeshFilter)))]
public class Cube : MonoBehaviour {

    private Mesh _mesh;
    private Vector3[] _vertices;
    public int xSize, ySize, zSize;

    //   private void Awake() {
    //       Generate();
    //   }
    //
    //   private void Generate() {
    //       _vertices = new Vector3[(xSize + 1) * (ySize + 1)];
    //       _vertices = new Vector3[(xSize + 1) * (ySize + 1)];
    // for (int i = 0, y = 0; y <= ySize; y++) {
    // 	for (int x = 0; x <= xSize; x++, i++) {
    //               _vertices[i] = new Vector3(x, y);
    // 	}
    // }
    //   }
    // private void Awake() {
    //     Generate();
    // }
    //
    // private void Generate () {
    //     GetComponent<MeshFilter>().mesh = _mesh = new Mesh();
    //     _mesh.name = "Procedural Cube";
    //
    //     _vertices = new Vector3[(xSize + 1) * (ySize + 1)];
    //     Vector2[] uv = new Vector2[_vertices.Length];
    //     Vector4[] tangents = new Vector4[_vertices.Length];
    //     Vector4 tangent = new Vector4(1, 0, 0, -1);
    //     for (int i = 0, y = 0; y <= ySize; y++) {
    //         for (int x = 0; x <= xSize; x++, i++) {
    //             _vertices[i] = new Vector3(x, y);
    //             uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
    //             tangents[i] = tangent;
    //         }
    //     }
    //     _mesh.vertices = _vertices;
    //     _mesh.uv = uv;
    //     _mesh.tangents = tangents;
    //
    //     int[] triangles = new int[xSize * ySize * 6];
    //     for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++) {
    //         for (int x = 0; x < xSize; x++, ti += 6, vi++) {
    //             triangles[ti] = vi;
    //             triangles[ti + 3] = triangles[ti + 2] = vi + 1;
    //             triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
    //             triangles[ti + 5] = vi + xSize + 2;
    //         }
    //     }
    //     _mesh.triangles = triangles;
    //     _mesh.RecalculateNormals();
    //}

    private void Awake() {
        Generate();
    }

    private void Generate() {
        GetComponent<MeshFilter>().mesh = _mesh = new Mesh();
        _mesh.name = "Procedural Cube";

        CreateVertices();
        CreateTriangles();
    }

    private void CreateTriangles() {
        int quads = (xSize * ySize + xSize * zSize + ySize * zSize) * 2;
        int[] triangles = new int[quads * 6];
        
        int ring = (xSize + zSize) * 2;
        int t = 0, v = 0;
        for (int y = 0; y < ySize; y++, v++) {
            for (int q = 0; q < ring - 1; q++, v++) {
                t = SetQuad(triangles, t, v, v + 1, v + ring, v + ring + 1);
            }
            t = SetQuad(triangles, t, v, v - ring + 1, v + ring, v + 1);
        }
        t = CreateTopFace(triangles, t, ring);
        t = CreateBottomFace(triangles, t, ring);
        _mesh.triangles = triangles;
    }
private int CreateBottomFace (int[] triangles, int t, int ring) {
        int v = 1;
        int vMid = _vertices.Length - (xSize - 1) * (zSize - 1);
        t = SetQuad(triangles, t, ring - 1, vMid, 0, 1);
        for (int x = 1; x < xSize - 1; x++, v++, vMid++) {
            t = SetQuad(triangles, t, vMid, vMid + 1, v, v + 1);
        }
        t = SetQuad(triangles, t, vMid, v + 2, v, v + 1);

        int vMin = ring - 2;
        vMid -= xSize - 2;
        int vMax = v + 2;

        for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++) {
            t = SetQuad(triangles, t, vMin, vMid + xSize - 1, vMin + 1, vMid);
            for (int x = 1; x < xSize - 1; x++, vMid++) {
                t = SetQuad(
                    triangles, t,
                    vMid + xSize - 1, vMid + xSize, vMid, vMid + 1);
            }
            t = SetQuad(triangles, t, vMid + xSize - 1, vMax + 1, vMid, vMax);
        }

        int vTop = vMin - 1;
        t = SetQuad(triangles, t, vTop + 1, vTop, vTop + 2, vMid);
        for (int x = 1; x < xSize - 1; x++, vTop--, vMid++) {
            t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vMid + 1);
        }
        t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vTop - 2);
		
        return t;
    }

    private int CreateTopFace (int[] triangles, int t, int ring) {
        int v = ring * ySize;
        for (int x = 0; x < xSize - 1; x++, v++) {
            t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + ring);
        }
        t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + 2);
        int vMin = ring * (ySize + 1) - 1;
        int vMid = vMin + 1;
        int vMax = v + 2;
        for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++) {
            t = SetQuad(triangles, t, vMin, vMid, vMin - 1, vMid + xSize - 1);
            for (int x = 1; x < xSize - 1; x++, vMid++) {
                t = SetQuad(triangles, t, vMid, vMid + 1, vMid + xSize - 1, vMid + xSize);
            }
            t = SetQuad(triangles, t, vMid, vMax, vMid + xSize - 1, vMax + 1);
        }
        int vTop = vMin - 2;
        t = SetQuad(triangles, t, vMin, vMid, vTop + 1, vTop);
        for (int x = 1; x < xSize - 1; x++, vTop--, vMid++) {
            t = SetQuad(triangles, t, vMid, vMid + 1, vTop, vTop - 1);
        }
        t = SetQuad(triangles, t, vMid, vTop - 2, vTop, vTop - 1);
        return t;
    }
    
    private void CreateVertices() {

        int cornerVertices = 8;
        int edgeVertices = (xSize + ySize + zSize - 3) * 4;
        int faceVertices = ((xSize - 1) * (ySize - 1) + (xSize - 1) * (zSize - 1) + (ySize - 1) * (zSize - 1)) * 2;
        _vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];

        int v = 0;
        for (int y = 0; y <= ySize; y++) {
            for (int x = 0; x <= xSize; x++) {
                _vertices[v++] = new Vector3(x, y, 0);
            }
            for (int z = 1; z <= zSize; z++) {
                _vertices[v++] = new Vector3(xSize, y, z);
            }
            for (int x = xSize - 1; x >= 0; x--) {
                _vertices[v++] = new Vector3(x, y, zSize);
            }
            for (int z = zSize - 1; z > 0; z--) {
                _vertices[v++] = new Vector3(0, y, z);
            }
        }
        for (int z = 1; z < zSize; z++) {
            for (int x = 1; x < xSize; x++) {
                _vertices[v++] = new Vector3(x, ySize, z);
            }
        }
        for (int z = 1; z < zSize; z++) {
            for (int x = 1; x < xSize; x++) {
                _vertices[v++] = new Vector3(x, 0, z);
            }
        }
        _mesh.vertices = _vertices;
    }

    private static int SetQuad(int[] triangles, int i, int v00, int v10, int v01, int v11) {
        triangles[i] = v00;
        triangles[i + 1] = triangles[i + 4] = v01;
        triangles[i + 2] = triangles[i + 3] = v10;
        triangles[i + 5] = v11;
        return i + 6;
    }

    private void OnDrawGizmos() {
        if (_vertices == null) {
            return;
        }
        Gizmos.color = Color.black;
        for (int i = 0; i < _vertices.Length; i++) {
            Gizmos.DrawSphere(_vertices[i], 0.1f);
        }
    }
}
