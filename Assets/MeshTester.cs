using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class MeshTester : MonoBehaviour
{/*

    public BoxCollider boxCollider;
    private int counter;
   
    private Vector3 oldPos;

    public GameObject cuttingTool;  // The cutting object that collides with the mesh
    public float subdivisionRadius = 0.2f;  // Radius within which new vertices will be added around the collision
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    void OnTriggerEnter(Collider other)
    {
        // Get the GameObject that the trigger collided with
        GameObject obj = other.gameObject;
        MeshFilter meshFilter = obj.GetComponent<MeshFilter>();

        if (meshFilter != null)
        {
            Mesh mesh = meshFilter.mesh;
            Vector3[] vertices = mesh.vertices;
            List<Vector3> newVertices = new List<Vector3>(vertices);  // Allow new vertices to be added
            int[] triangles = mesh.triangles;
            List<int> newTriangles = new List<int>(triangles);

            // Get the closest point on the cutting tool trigger from the other object's mesh
            Vector3 triggerPoint = other.ClosestPoint(cuttingTool.transform.position);

            // Find the triangle(s) near the trigger point
            for (int i = 0; i < triangles.Length; i += 3)
            {
                int v1 = triangles[i];
                int v2 = triangles[i + 1];
                int v3 = triangles[i + 2];

                Vector3 worldV1 = obj.transform.TransformPoint(vertices[v1]);
                Vector3 worldV2 = obj.transform.TransformPoint(vertices[v2]);
                Vector3 worldV3 = obj.transform.TransformPoint(vertices[v3]);

                // Calculate the distance from the trigger point to each vertex
                float distV1 = Vector3.Distance(triggerPoint, worldV1);
                float distV2 = Vector3.Distance(triggerPoint, worldV2);
                float distV3 = Vector3.Distance(triggerPoint, worldV3);

                // If the trigger point is close to the triangle, add new vertices
                if (distV1 < subdivisionRadius || distV2 < subdivisionRadius || distV3 < subdivisionRadius)
                {
                    // Add a new vertex at or near the trigger point
                    Vector3 newVertex = obj.transform.InverseTransformPoint(triggerPoint + Random.insideUnitSphere * 0.01f); // Small random offset
                    newVertices.Add(newVertex);
                    int newVertexIndex = newVertices.Count - 1;

                    // Create new triangles using the new vertex and existing ones
                    newTriangles.Add(v1);
                    newTriangles.Add(v2);
                    newTriangles.Add(newVertexIndex);

                    newTriangles.Add(v2);
                    newTriangles.Add(v3);
                    newTriangles.Add(newVertexIndex);

                    newTriangles.Add(v3);
                    newTriangles.Add(v1);
                    newTriangles.Add(newVertexIndex);
                }
            }

            // Update the mesh with new vertices and triangles
            mesh.vertices = newVertices.ToArray();
            mesh.triangles = newTriangles.ToArray();
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }
    }

    // Optionally, this can be used to continue subdividing while inside the trigger zone
    

    */
}
