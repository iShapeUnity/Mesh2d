using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace iShape.Mesh2d {

    public struct NativePrimitiveMesh {
        
        public NativeList<float3> vertices;
        public NativeList<int> triangles;

        public NativePrimitiveMesh(int capacity, Allocator allocator) {
            this.vertices = new NativeList<float3>(capacity, allocator);
            this.triangles = new NativeList<int>(3 * capacity, allocator);
        }
        
        public NativePrimitiveMesh(NativeList<float3> vertices, NativeList<int> triangles) {
            this.vertices = vertices;
            this.triangles = triangles;
        }
        
        public NativePrimitiveMesh(NativeArray<float3> vertices, NativeArray<int> triangles, Allocator allocator) {
            this.vertices = new NativeList<float3>(vertices.Length, allocator);
            this.vertices.CopyFrom(vertices);
            
            this.triangles = new NativeList<int>(triangles.Length, allocator);
            this.triangles.CopyFrom(triangles);
        }

        public void Add(NativePrimitiveMesh primitiveMesh) {
            int count = this.vertices.Length;
            this.vertices.AddRange(primitiveMesh.vertices);

            for (int i = 0; i < primitiveMesh.triangles.Length; ++i) {
                int j = primitiveMesh.triangles[i];
                this.triangles.Add(j + count);    
            }
        }
        
        public void Add(StaticPrimitiveMesh primitiveMesh) {
            int count = this.vertices.Length;
            this.vertices.AddRange(primitiveMesh.vertices);

            for (int i = 0; i < primitiveMesh.triangles.Length; ++i) {
                int j = primitiveMesh.triangles[i];
                this.triangles.Add(j + count);    
            }
        }
        
        public void AddAndDispose(NativePrimitiveMesh primitiveMesh) {
            Add(primitiveMesh);
            primitiveMesh.Dispose();
        }
        
        public void AddAndDispose(StaticPrimitiveMesh primitiveMesh) {
            Add(primitiveMesh);
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

            this.Fill(mesh);
            
            this.Dispose();
            
            return mesh;
        }
        
        public void Fill(Mesh mesh) {
            int vertexCount = vertices.Length;
            mesh.SetVertexBufferParams(vertexCount, new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3));
            mesh.SetVertexBufferData(vertices.AsArray(), 0, 0, vertexCount);

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

        public void DebugFill(Mesh mesh, Color color) {
            mesh.Clear();
            
            var colors = new Color[vertices.Length];
            for (int i = 0; i < vertices.Length; ++i) {
                colors[i] = color;
            }
            mesh.vertices = vertices.AsArray().Reinterpret<Vector3>().ToArray();
            
            mesh.colors = colors;
            mesh.triangles = triangles.AsArray().ToArray();
            mesh.MarkModified();
        }
        
        public void ReleaseFill(Mesh mesh, Color color) {
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
            mesh.SetIndexBufferData(triangles.AsArray(), 0, 0, indexCount);

            var bounds = vertices.Bounds();
            var subMeshDesc = new SubMeshDescriptor(0, indexCount, MeshTopology.Triangles) {
                bounds = bounds,
                vertexCount = vertexCount,
            };
            mesh.SetSubMesh(0, subMeshDesc, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
            mesh.bounds = bounds;
        }

        public void Dispose() {
            this.vertices.Dispose();
            this.triangles.Dispose();            
        }
        
        public void Clear() {
            this.vertices.Clear();
            this.triangles.Clear();
        }

    }

}