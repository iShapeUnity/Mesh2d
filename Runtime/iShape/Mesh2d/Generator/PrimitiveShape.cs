using Unity.Collections;
using Unity.Mathematics;

namespace iShape.Mesh2d {

    internal static class PrimitiveShape {

        internal static NativePrimitiveMesh Rect(float2 center, float2 size, float z, Allocator allocator) {

            var vertices = new NativeList<float3>(4, allocator);
            var triangles = new NativeList<int>(12, allocator);

            float2 ds = 0.5f * size;
            
            vertices.Add(new float3(center - ds, z));
            vertices.Add(new float3(center + new float2(-ds.x, ds.y), z));
            vertices.Add(new float3(center + ds, z));
            vertices.Add(new float3(center + new float2(ds.x, -ds.y), z));

            triangles.Add(0);
            triangles.Add(1);
            triangles.Add(2);
            
            triangles.Add(0);
            triangles.Add(2);
            triangles.Add(3);

            return new NativePrimitiveMesh(vertices, triangles);
        }
        
        internal static NativePrimitiveMesh Circle(float2 center, float radius, int count, float z, Allocator allocator) {
            float da = 2 * math.PI / count;
            
            var vertices = new NativeList<float3>(count + 1, allocator);
            var triangles = new NativeList<int>(3 * count, allocator);

            float a = 0;
            int j = count - 1;
            for (int i = 0; i < count; ++i) {
                float2 xy = new float2(math.cos(a), math.sin(a));
                float2 p = center + radius * xy;
                vertices.Add(new float3(p, z));
                
                triangles.Add(count);
                triangles.Add(i);
                triangles.Add(j);
                
                a += da;
                j = i;
            }
            
            vertices.Add(new float3(center, z));

            return new NativePrimitiveMesh(vertices, triangles);
        }

    }

}