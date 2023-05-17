using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace iShape.Mesh2d {

    public struct StaticPrimitiveMesh {
        
        public NativeArray<float3> vertices;
        public NativeArray<int> triangles;
        
        public StaticPrimitiveMesh(NativeArray<float3> vertices, NativeArray<int> triangles) {
            this.vertices = vertices;
            this.triangles = triangles;
        }
        
        public StaticPrimitiveMesh(NativeArray<float3> vertices, NativeArray<int> triangles, Allocator allocator) {
            this.vertices = new NativeArray<float3>(vertices.Length, allocator);
            this.vertices.CopyFrom(vertices);
            
            this.triangles = new NativeArray<int>(triangles.Length, allocator);
            this.triangles.CopyFrom(triangles);
        }
        
        public void Shift(int offset) {
            for (int i = 0; i < triangles.Length; ++i) {
                this.triangles[i] = triangles[i] + offset;
            }
        }
        
        public void ShiftZ(float offset) {
            for (int i = 0; i < vertices.Length; ++i) {
                float3 v = this.vertices[i];
                v.z += offset;
                this.vertices[i] = v;
            }
        }
        
        public Mesh Convert() {
            var mesh = new Mesh();

            this.Fill(mesh);
            
            this.Dispose();
            
            return mesh;
        }
        
        public void Fill(Mesh mesh) {
#if UNITY_EDITOR
            DebugFill(mesh);
#else
            ReleaseFill(mesh);
#endif
        }

        private void DebugFill(Mesh mesh) {
            mesh.Clear();
            mesh.vertices = vertices.Reinterpret<Vector3>().ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.MarkModified();
        }
        
        private void ReleaseFill(Mesh mesh) {
            int vertexCount = vertices.Length;
            mesh.SetVertexBufferParams(vertexCount, new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3));
            mesh.SetVertexBufferData(vertices, 0, 0, vertexCount);

            int indexCount = triangles.Length;
            mesh.SetIndexBufferParams(indexCount, IndexFormat.UInt32);
            mesh.SetIndexBufferData(triangles, 0, 0, indexCount);

            var bounds = vertices.Bounds();
            var subMeshDesc = new SubMeshDescriptor(0, indexCount, MeshTopology.Triangles) {
                bounds = bounds,
                vertexCount = vertexCount
            };

            mesh.SetSubMesh(0, subMeshDesc, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
            mesh.bounds = bounds;
        }

        public Mesh Convert(Color color) {
            var mesh = new Mesh();
            
            this.Fill(mesh, color);
            
            this.Dispose();

            return mesh;
        }

        public void Fill(Mesh mesh, Color color) {
#if UNITY_EDITOR
            DebugFill(mesh, color);
#else
            ReleaseFill(mesh, color);
#endif
        }
        
        private void DebugFill(Mesh mesh, Color color) {
            mesh.Clear();
            mesh.vertices = vertices.Reinterpret<Vector3>().ToArray();
            mesh.triangles = triangles.ToArray();

            var colors = new Color[vertices.Length];
            for (int i = 0; i < vertices.Length; i++) {
                colors[i] = color;
            }

            mesh.colors = colors;
            
            mesh.MarkModified();
        }
        
        private void ReleaseFill(Mesh mesh, Color color) {
            int vertexCount = vertices.Length;
            mesh.SetVertexBufferParams(vertexCount, new VertexAttributeDescriptor[]
            {
                new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3),
                new VertexAttributeDescriptor(VertexAttribute.Color, VertexAttributeFormat.Float32, 4)
            });

            float4 clr = new float4(color.r, color.g, color.b, color.a);
            var vertexData = new NativeArray<ColorVertex>(vertexCount, Allocator.Temp);
            for (int i = 0; i < vertexCount; ++i) {
                vertexData[i] = new ColorVertex(vertices[i], clr);
            }
            
            mesh.SetVertexBufferData(vertexData, 0, 0, vertexCount);

            vertexData.Dispose();
            
            int indexCount = triangles.Length;
            mesh.SetIndexBufferParams(indexCount, IndexFormat.UInt32);
            mesh.SetIndexBufferData(triangles, 0, 0, indexCount);

            var bounds = vertices.Bounds();
            var subMeshDesc = new SubMeshDescriptor(0, indexCount, MeshTopology.Triangles) {
                bounds = bounds,
                vertexCount = vertexCount
            };

            mesh.SetSubMesh(0, subMeshDesc, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
        }
        

        public void Dispose() {
            this.vertices.Dispose();
            this.triangles.Dispose();            
        }

    }

}