using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace iShape.Mesh2d {

    public struct NativeColorMesh {
        
        private NativeList<float3> vertices;
        private NativeList<int> triangles;
        private NativeList<float4> colors;
        
        public NativeColorMesh(int capacity, Allocator allocator) {
            this.vertices = new NativeList<float3>(capacity, allocator);
            this.colors = new NativeList<float4>(capacity, allocator);
            this.triangles = new NativeList<int>(3 * capacity, allocator);
        }

        public void Add(NativeColorMesh colorMesh) {
            int count = this.vertices.Length;
            this.vertices.AddRange(colorMesh.vertices);
            this.colors.AddRange(colorMesh.colors);
            for (int i = 0; i < colorMesh.triangles.Length; i++) {
                int j = colorMesh.triangles[i];
                this.triangles.Add(j + count);    
            }
        }
        
        public void Add(NativePrimitiveMesh primitiveMesh, Color color) {
            int count = this.vertices.Length;
            float4 clr = new float4(color.r, color.g, color.b, color.a);
            this.vertices.AddRange(primitiveMesh.vertices);
            for (int i = 0; i < primitiveMesh.vertices.Length; ++i) {
                this.colors.Add(clr);    
            }
            for (int i = 0; i < primitiveMesh.triangles.Length; ++i) {
                int j = primitiveMesh.triangles[i];
                this.triangles.Add(j + count);    
            }
        }
        
        public void Add(StaticPrimitiveMesh primitiveMesh, Color color) {
            int count = this.vertices.Length;
            float4 clr = new float4(color.r, color.g, color.b, color.a);
            this.vertices.AddRange(primitiveMesh.vertices);
            for (int i = 0; i < primitiveMesh.vertices.Length; ++i) {
                this.colors.Add(clr);    
            }
            for (int i = 0; i < primitiveMesh.triangles.Length; ++i) {
                int j = primitiveMesh.triangles[i];
                this.triangles.Add(j + count);    
            }
        }

        public void AddAndDispose(NativeColorMesh colorMesh) {
            Add(colorMesh);
            colorMesh.Dispose();
        }
        
        public void AddAndDispose(NativePrimitiveMesh primitiveMesh, Color color) {
            Add(primitiveMesh, color);
            primitiveMesh.Dispose();
        }
        
        public void AddAndDispose(StaticPrimitiveMesh primitiveMesh, Color color) {
            Add(primitiveMesh, color);
            primitiveMesh.Dispose();
        }
        
        public void Shift(int offset) {
            for (int i = 0; i < triangles.Length; ++i) {
                this.triangles[i] = triangles[i] + offset;
            }
        }
    
        public void ShiftZ(float offset) {
            for (int i = 0; i < vertices.Length; ++i) {
                var v = this.vertices[i];
                v.z += offset;
                this.vertices[i] = v;
            }
        }
    
        public Mesh Convert() {
            var mesh = new Mesh();
            
            Fill(mesh);

            this.Dispose();

            return mesh;
        }
        
        public void Fill(Mesh mesh) {
            int vertexCount = vertices.Length;
            mesh.SetVertexBufferParams(vertexCount, new VertexAttributeDescriptor[]
            {
                new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3),
                new VertexAttributeDescriptor(VertexAttribute.Color, VertexAttributeFormat.Float32, 4)
            });

            var vertexData = new NativeArray<ColorVertex>(vertexCount, Allocator.Temp);
            for (int i = 0; i < vertexCount; ++i) {
                vertexData[i] = new ColorVertex(vertices[i], colors[i]);
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
        }

        public void Dispose() {
            this.vertices.Dispose();
            this.triangles.Dispose();
            this.colors.Dispose();
        }

        public void Clear() {
            this.vertices.Clear();
            this.triangles.Clear();
            this.colors.Clear();
        }

    }
}