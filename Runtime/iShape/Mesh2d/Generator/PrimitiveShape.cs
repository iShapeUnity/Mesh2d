using Unity.Collections;
using Unity.Mathematics;

namespace iShape.Mesh2d {

    internal static class PrimitiveShape {

        internal static StaticPrimitiveMesh Rect(float2 center, float2 size, float z, Allocator allocator) {

            var vertices = new NativeArray<float3>(4, allocator);
            var triangles = new NativeArray<int>(12, allocator);

            float2 ds = 0.5f * size;
            
            vertices[0] = new float3(center - ds, z);
            vertices[1] = new float3(center + new float2(-ds.x, ds.y), z);
            vertices[2] = new float3(center + ds, z);
            vertices[3] = new float3(center + new float2(ds.x, -ds.y), z);

            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;
            
            triangles[3] = 0;
            triangles[4] = 2;
            triangles[5] = 3;

            return new StaticPrimitiveMesh(vertices, triangles);
        }
        
        internal static StaticPrimitiveMesh Circle(float2 center, float radius, int count, float z, Allocator allocator) {
            float da = 2 * math.PI / count;
            
            var vertices = new NativeArray<float3>(count + 1, allocator);
            var triangles = new NativeArray<int>(3 * count, allocator);

            float a = 0;
            int j = count - 1;
            int k = 0;
            for (int i = 0; i < count; ++i) {
                float2 xy = new float2(math.cos(a), math.sin(a));
                float2 p = center + radius * xy;
                vertices[i] = new float3(p, z);
                
                triangles[k++] = count;
                triangles[k++] = i;
                triangles[k++] = j;
                
                a += da;
                j = i;
            }
            
            vertices[count] = new float3(center, z);

            return new StaticPrimitiveMesh(vertices, triangles);
        }

    }

}