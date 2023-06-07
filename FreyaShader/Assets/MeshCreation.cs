using UnityEngine;

public class MeshCreation : MonoBehaviour
{
    public int widthSegments = 10;  // Number of segments along the width
    public int lengthSegments = 10; // Number of segments along the length
    public float width = 1f;        // Width of the plane
    public float length = 1f;       // Length of the plane

    void Start()
    {
        // Create a new mesh
        Mesh mesh = new Mesh();

        // Calculate the number of vertices, triangles, and UV coordinates
        int numVertices = (widthSegments + 1) * (lengthSegments + 1);
        int numTriangles = widthSegments * lengthSegments * 2 * 3;
        int numUvs = numVertices;

        // Initialize arrays to store the vertices, triangles, and UV coordinates
        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numTriangles];
        Vector2[] uvs = new Vector2[numUvs];

        // Calculate the step sizes for the vertices and UV coordinates
        float stepSizeX = width / widthSegments;
        float stepSizeZ = length / lengthSegments;
        float uvStepSizeX = 1f / widthSegments;
        float uvStepSizeZ = 1f / lengthSegments;

        // Generate the vertices and UV coordinates
        int vertexIndex = 0;
        int uvIndex = 0;
        for (int z = 0; z <= lengthSegments; z++)
        {
            for (int x = 0; x <= widthSegments; x++)
            {
                float posX = x * stepSizeX - width / 2f;
                float posZ = z * stepSizeZ - length / 2f;
                vertices[vertexIndex] = new Vector3(posX, 0f, posZ);
                uvs[uvIndex] = new Vector2(x * uvStepSizeX, z * uvStepSizeZ);
                vertexIndex++;
                uvIndex++;
            }
        }

        // Generate the triangles
        int triangleIndex = 0;
        for (int z = 0; z < lengthSegments; z++)
        {
            for (int x = 0; x < widthSegments; x++)
            {
                int topLeft = z * (widthSegments + 1) + x;
                int topRight = topLeft + 1;
                int bottomLeft = (z + 1) * (widthSegments + 1) + x;
                int bottomRight = bottomLeft + 1;

                // First triangle
                triangles[triangleIndex++] = topLeft;
                triangles[triangleIndex++] = bottomLeft;
                triangles[triangleIndex++] = topRight;

                // Second triangle
                triangles[triangleIndex++] = topRight;
                triangles[triangleIndex++] = bottomLeft;
                triangles[triangleIndex++] = bottomRight;
            }
        }

        // Set the vertices, triangles, and UV coordinates of the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        // Recalculate the normals and bounds of the mesh
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // Create a new game object and assign the mesh to it
        GameObject meshObject = new GameObject("MeshObject");
        meshObject.AddComponent<MeshFilter>().mesh = mesh;
        meshObject.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));

        // Optional: Set the position, rotation, and scale of the mesh object
        meshObject.transform.position = Vector3.zero;
        meshObject.transform.rotation = Quaternion.identity;
        meshObject.transform.localScale = Vector3.one;
    }
}
