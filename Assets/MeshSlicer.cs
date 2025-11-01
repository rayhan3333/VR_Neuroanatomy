using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshSlicer
{ 
    public static Mesh[] SliceMesh(Mesh mesh, Vector3 cutOrigin, Vector3 cutNormal)
    {
        Plane plane = new Plane(cutNormal, cutOrigin);
        MeshConstructionHelper positiveMesh = new MeshConstructionHelper();
        MeshConstructionHelper negativeMesh = new MeshConstructionHelper();

        int[] meshTriangles = mesh.triangles;
        for (int i = 0; i < meshTriangles.Length; i += 3)
        {
            VertexData vertexA = GetVertexData(mesh, plane, meshTriangles[i]);
            VertexData vertexB = GetVertexData(mesh, plane, meshTriangles[i + 1]);
            VertexData vertexC = GetVertexData(mesh, plane, meshTriangles[i + 2]);

            bool isABSameSide = vertexA.Side == vertexB.Side;
            bool isBCSameSide = vertexB.Side == vertexC.Side;

            if (isABSameSide && isBCSameSide)
            {
                MeshConstructionHelper helper = vertexA.Side ? positiveMesh : negativeMesh;
                helper.AddMeshSection(vertexA, vertexB, vertexC);
            }
        }


        return new[] { positiveMesh.ConstructMesh(), negativeMesh.ConstructMesh() };
    }

    private static VertexData GetVertexData(Mesh mesh, Plane plane, int index) {

        Vector3 position = mesh.vertices[index];
        VertexData vertexData = new VertexData()
        {
            Position = position,
            Side = plane.GetSide(position),
            Uv = mesh.uv[index],
            Normal = mesh.normals[index]
        };
        return vertexData;

    }

    class MeshConstructionHelper
    {
        private List<Vector3> _vertices;
        private List<int> _triangles;
        private List<Vector2> _uvs;
        private List<Vector3> _normals;

        public MeshConstructionHelper()
        {
            _vertices = new List<Vector3>();
            _triangles = new List<int>();
            _uvs = new List<Vector2>();
            _normals = new List<Vector3>();



        }

        public Mesh ConstructMesh()
        {
            Mesh mesh = new Mesh();
            mesh.vertices = _vertices.ToArray();
            mesh.triangles = _triangles.ToArray();
            mesh.normals = _normals.ToArray();
            mesh.uv = _uvs.ToArray();
            return mesh;
        }

        public void AddMeshSection(VertexData vertexA, VertexData vertexB, VertexData vertexC)
        {
            int indexA = TryAddVertex(vertexA);
            int indexB = TryAddVertex(vertexB);
            int indexC = TryAddVertex(vertexC);

            AddTriangle(indexA, indexB, indexC);
        }


        private void AddTriangle(int indexA, int indexB, int indexC)
        {
            _triangles.Add(indexA);
            _triangles.Add(indexB);
            _triangles.Add(indexC);
        }

        private int TryAddVertex(VertexData vertex)
        {
            _vertices.Add(vertex.Position);
            _uvs.Add(vertex.Uv);
            _normals.Add(vertex.Normal);
            return _vertices.Count - 1;
        }
    }
}
