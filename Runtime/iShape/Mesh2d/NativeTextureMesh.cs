using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace iShape.Mesh2d {

    public struct NativeTextureMesh {
        
        private NativeList<float3> vertices;
        private NativeList<int> triangles;
        private NativeList<float2> uvs;
        
        public NativeTextureMesh(int capacity, Allocator allocator) {
            this.vertices = new NativeList<float3>(capacity, allocator);
            this.uvs = new NativeList<float2>(capacity, allocator);
            this.triangles = new NativeList<int>(3 * capacity, allocator);
        }

        public void Add(NativeTextureMesh mesh) {
            int count = this.vertices.Length;
            this.vertices.AddRange(mesh.vertices);
            this.uvs.AddRange(mesh.uvs);
            for (int i = 0; i < mesh.triangles.Length; i++) {
                int j = mesh.triangles[i];
                this.triangles.Add(j + count);    
            }
        }
        
        public void Add(NativePrimitiveMesh primitiveMesh, float scale) {
            int count = this.vertices.Length;
            this.vertices.AddRange(primitiveMesh.vertices);
            for (int i = 0; i < primitiveMesh.vertices.Length; ++i) {
                var v = primitiveMesh.vertices[i] / scale;
                this.uvs.Add(new float2(v.x, v.y));    
            }
            for (int i = 0; i < primitiveMesh.triangles.Length; ++i) {
                int j = primitiveMesh.triangles[i];
                this.triangles.Add(j + count);    
            }
        }
        
        public void Add(StaticPrimitiveMesh primitiveMesh, float scale) {
            int count = this.vertices.Length;
            this.vertices.AddRange(primitiveMesh.vertices);
            for (int i = 0; i < primitiveMesh.vertices.Length; ++i) {
                var v = primitiveMesh.vertices[i] / scale;
                this.uvs.Add(new float2(v.x, v.y));
            }
            for (int i = 0; i < primitiveMesh.triangles.Length; ++i) {
                int j = primitiveMesh.triangles[i];
                this.triangles.Add(j + count);    
            }
        }

        public void AddAndDispose(NativeTextureMesh mesh) {
            Add(mesh);
            mesh.Dispose();
        }
        
        public void AddAndDispose(NativePrimitiveMesh mesh, float scale) {
            Add(mesh, scale);
            mesh.Dispose();
        }
        
        public void AddAndDispose(StaticPrimitiveMesh mesh, float scale) {
            Add(mesh, scale);
            mesh.Dispose();
        }
    
        public Mesh Convert(bool isDebug = false) {
            var mesh = new Mesh();

            if (isDebug) {
                mesh.Clear();
                mesh.vertices = this.vertices.AsArray().Reinterpret<Vector3>().ToArray();
                mesh.uv = this.uvs.AsArray().Reinterpret<Vector2>().ToArray();
                mesh.triangles = this.triangles.AsArray().ToArray();
                mesh.RecalculateBounds();
            } else {
                Fill(mesh);    
            }

            this.Dispose();

            return mesh;
        }
        
        public void Fill(Mesh mesh) {
            int vertexCount = vertices.Length;
            mesh.SetVertexBufferParams(vertexCount, new VertexAttributeDescriptor[]
            {
                new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3),
                new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2)
            });

            var vertexData = new NativeArray<TextureVertex>(vertexCount, Allocator.Temp);
            for (int i = 0; i < vertexCount; ++i) {
                vertexData[i] = new TextureVertex(vertices[i], uvs[i]);
            }
            
            mesh.SetVertexBufferData(vertexData, 0, 0, vertexCount);

            vertexData.Dispose();
            
            int indexCount = triangles.Length;
            mesh.SetIndexBufferParams(indexCount, IndexFormat.UInt32);
            mesh.SetIndexBufferData(triangles.AsArray(), 0, 0, indexCount);
            
            var bounds = vertices.Bounds();
            var subMeshDesc = new SubMeshDescriptor(0, indexCount, MeshTopology.Triangles) {
                bounds = bounds,
                vertexCount = vertexCount
            };

            mesh.SetSubMesh(0, subMeshDesc, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
            mesh.bounds = bounds;
        }

        public void Dispose() {
            this.vertices.Dispose();
            this.triangles.Dispose();
            this.uvs.Dispose();
        }

        public void Clear() {
            this.vertices.Clear();
            this.triangles.Clear();
            this.uvs.Clear();
        }
    }

}