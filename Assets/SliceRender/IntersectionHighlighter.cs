using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntersectionHighlighter : MonoBehaviour
{
    public Material planeMaterial; // The material on the plane with the intersection shader
    public RenderTexture intersectionMaskTexture; // The RenderTexture to store intersection regions
    public Transform[] meshes; // All the meshes that should contribute to the intersection highlights
    public Transform planeTransform; // The plane the meshes are intersecting with

    void Start()
    {
        // Initialize the intersection mask texture (for simplicity, assume it’s already created)
        Graphics.SetRenderTarget(intersectionMaskTexture);
        GL.Clear(true, true, Color.clear); // Clear the texture

        // Loop through all meshes and update the mask texture
        foreach (Transform mesh in meshes)
        {
            HighlightIntersection(mesh);
        }

        // Now assign the intersection mask to the plane's material
        planeMaterial.SetTexture("_IntersectionMask", intersectionMaskTexture);
    }

    void HighlightIntersection(Transform mesh)
    {
        // Calculate the intersection region of the mesh and the plane
        // You may need to use raycasting or other methods to detect the intersection
        // For simplicity, we'll assume we already have the intersection region as a mask
        // Set this region on the intersection mask texture

        // Example: mark the region where the mesh intersects the plane
        // Here you could use any method to determine the area of intersection
        // This could be a projection of the mesh on the plane or collision detection

        // For demonstration, let's assume we simply mark some area on the texture
        // Update the intersectionMaskTexture based on mesh-plane interaction
        Vector2 intersectionUV = GetIntersectionUV(mesh);
        Graphics.SetRenderTarget(intersectionMaskTexture);
        GL.PushMatrix();
        GL.LoadOrtho();
        GL.Begin(GL.QUADS);
        GL.Color(Color.red); // Marking the region with red for example
        GL.Vertex3(intersectionUV.x, intersectionUV.y, 0);
        GL.Vertex3(intersectionUV.x + 0.1f, intersectionUV.y, 0); // Adjust the region size
        GL.Vertex3(intersectionUV.x + 0.1f, intersectionUV.y + 0.1f, 0);
        GL.Vertex3(intersectionUV.x, intersectionUV.y + 0.1f, 0);
        GL.End();
        GL.PopMatrix();
    }
    bool LinePlaneIntersection(Vector3 lineStart, Vector3 lineEnd, Vector3 planeNormal, Vector3 planePoint, out Vector3 intersection)
    {
        intersection = Vector3.zero;

        Vector3 lineDirection = lineEnd - lineStart;
        float dot = Vector3.Dot(planeNormal, lineDirection);

        // If the line is parallel to the plane, there's no intersection
        if (Mathf.Abs(dot) < 1e-6f) return false;

        float t = Vector3.Dot(planeNormal, planePoint - lineStart) / dot;
        if (t < 0.0f || t > 1.0f) return false; // Intersection is outside the line segment

        intersection = lineStart + t * lineDirection;
        return true;
    }

    Vector2 GetIntersectionUV(Transform mesh)
    {
        // Get the plane's Transform
        Transform plane = planeTransform;

        // Get the mesh's bounds
        MeshRenderer renderer = mesh.GetComponent<MeshRenderer>();
        if (renderer == null) return Vector2.zero;

        // Get the world bounds of the mesh
        Bounds meshBounds = renderer.bounds;

        // Iterate through the corners of the mesh's bounds to find intersections
        Vector3[] corners = new Vector3[8];
        corners[0] = meshBounds.min;
        corners[1] = new Vector3(meshBounds.max.x, meshBounds.min.y, meshBounds.min.z);
        corners[2] = new Vector3(meshBounds.min.x, meshBounds.max.y, meshBounds.min.z);
        corners[3] = new Vector3(meshBounds.min.x, meshBounds.min.y, meshBounds.max.z);
        corners[4] = meshBounds.max;
        corners[5] = new Vector3(meshBounds.min.x, meshBounds.max.y, meshBounds.max.z);
        corners[6] = new Vector3(meshBounds.max.x, meshBounds.min.y, meshBounds.max.z);
        corners[7] = new Vector3(meshBounds.max.x, meshBounds.max.y, meshBounds.min.z);

        // Plane's normal and point
        Vector3 planeNormal = plane.up; // Assuming the plane's "up" is its normal
        Vector3 planePoint = plane.position;

        // Find intersection points
        List<Vector3> intersections = new List<Vector3>();
        for (int i = 0; i < corners.Length; i++)
        {
            for (int j = i + 1; j < corners.Length; j++)
            {
                Vector3 intersection;
                if (LinePlaneIntersection(corners[i], corners[j], planeNormal, planePoint, out intersection))
                {
                    intersections.Add(intersection);
                }
            }
        }

        // If no intersections, return default
        if (intersections.Count == 0) return Vector2.zero;

        // Convert the first intersection to plane local coordinates
        Vector3 localIntersection = plane.InverseTransformPoint(intersections[0]);

        // Map local plane coordinates to UV
        float u = localIntersection.x / plane.localScale.x + 0.5f;
        float v = localIntersection.z / plane.localScale.z + 0.5f;

        return new Vector2(u, v);
    }
}
