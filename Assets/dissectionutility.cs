using Microsoft.MixedReality.Toolkit;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.InputSystem.Android;
using UnityEngine.UIElements;

public class dissectionutility : MonoBehaviour
{/*
    public BoxCollider boxCollider;
    public BoxCollider delCollider;
    private int counter;
    public GameObject spherePrefab;
    private Vector3 oldPos;
    List<int> indToRemove = new List<int>();

    private void OnTriggerEnter(Collider collision)
    {
        oldPos = gameObject.transform.position;
        //Debug.Log("collision");
        GameObject obj = collision.gameObject;

        MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
        
        Mesh mesh = meshFilter.mesh;

        Vector3[] vertices = mesh.vertices;
       // List<Vector3> newVertices = new List<Vector3>();
        int[] triangles = mesh.triangles;
       
        //Debug.Log("vertice length: " +  vertices.Length);
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldVertex = collision.gameObject.transform.TransformPoint(vertices[i]);
            Vector3 closestPoint = boxCollider.ClosestPoint(worldVertex);
            float distance = Vector3.Distance(worldVertex, closestPoint);

            //Debug.Log($"Vertex {i}: {worldVertex}, Closest Point: {closestPoint}, Distance: {distance}");
            if (distance < 0.03f)
            {
                counter++;
                
                
                //Instantiate(spherePrefab, worldVertex, Quaternion.identity);
                Vector3 direction = boxCollider.ClosestPoint(worldVertex) - worldVertex;
                Vector3 newWorld = worldVertex - direction.normalized * 0.2f;

                if (!indToRemove.Contains(i))
                {
                    if (!collision.bounds.Contains(newWorld) && !indToRemove.Contains(i))
                    {
                        Debug.Log("Vertex exited collider!");
                        indToRemove.Add(i);
                        delCollider.enabled = true;
                        newWorld = delCollider.ClosestPointOnBounds(newWorld);

                    }

                    vertices[i] = collision.gameObject.transform.InverseTransformPoint(newWorld);
                }
                
                
            }
        }
        /*if (indToRemove.Count > 0)
        {
            
            Vector3[] newVertices = new Vector3[vertices.Length - indToRemove.Count];
            int newIndex = 0;
            for (int i = 0; i < vertices.Length; i++)
            {
               
                if (!indToRemove.Contains(i))
                {
                    newVertices[newIndex] = vertices[i];
                    newIndex++;
                }
            }
            List<int> newTriangles = new List<int>();
            for (int i = 0; i < triangles.Length; i += 3)
            {
                int count = 0;

                int v1 = triangles[i];     // First vertex index of the triangle
                int v2 = triangles[i + 1]; // Second vertex index of the triangle
                int v3 = triangles[i + 2]; // Third vertex index of the triangle

                if (indToRemove.Contains(v1)) count++;
                if (indToRemove.Contains(v2)) count++;
                if (indToRemove.Contains(v3)) count++;
                // If none of the triangle's vertices are in the indicesToRemove list, keep the triangle
                if (count < 2)
                {
                    newTriangles.Add(v1);
                    newTriangles.Add(v2);
                    newTriangles.Add(v3);
                }
            }

            // Convert the new triangle list back to an array and assign it to the mesh
            mesh.triangles = newTriangles.ToArray();
        }
        Debug.Log(counter);
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        delCollider.enabled = false;

    }
    private void OnTriggerStay(Collider collision)
    {

       OnTriggerEnter(collision);
    }

    // Start is called before the first frame update
    void Start()
    {
        delCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
