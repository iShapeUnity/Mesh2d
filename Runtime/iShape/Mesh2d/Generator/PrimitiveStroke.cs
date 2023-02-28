using Unity.Collections;
using Unity.Mathematics;

namespace iShape.Mesh2d {

    internal static class PrimitiveStroke {

        internal static NativePrimitiveMesh StrokeForEdge(float2 star, float2 end, StrokeStyle strokeStyle, float z, Allocator allocator) {
            var path = new NativeArray<float2>(2, Allocator.Temp);
            path[0] = star;
            path[1] = end;
            
            var mesh = PathStroke.BuildMesh(path, false, strokeStyle, z, allocator);
            
            path.Dispose();
            
            return mesh;
        }
        
        internal static NativePrimitiveMesh StrokeForRect(float2 center, float2 size, StrokeStyle strokeStyle, float z, Allocator allocator) {
            var path = new NativeArray<float2>(4, Allocator.Temp);
            float2 ds = 0.5f * size;
            path[0] = center - ds;
            path[1] = center + new float2(-ds.x, ds.y);
            path[2] = center + ds;
            path[3] = center + new float2(ds.x, -ds.y);
            
            var mesh = PathStroke.BuildMesh(path, true, strokeStyle, z, allocator);
            
            path.Dispose();
            
            return mesh;
        }
        
        internal static NativePrimitiveMesh StrokeForCircle(float2 center, float radius, int count, StrokeStyle strokeStyle, float z, Allocator allocator) {
            float da = 2 * math.PI / count;
            var path = new NativeArray<float2>(count, Allocator.Temp);
            float a = 0;
            for (int i = 0; i < count; ++i) {
                float2 xy = new float2(math.cos(a), math.sin(a));
                path[i] = center + radius * xy;
                a += da;
            }
            
            var mesh = PathStroke.BuildMesh(path, true, strokeStyle, z, allocator);
            
            path.Dispose();
            
            return mesh;
        }
        
        internal static NativePrimitiveMesh StrokeForSoftStar(float2 center, float smallRadius, float largeRadius, int count, StrokeStyle strokeStyle, float z, Allocator allocator) {
            float da0 = 2 * math.PI / count;
            float da1 = 16 * math.PI / count;
            var path = new NativeArray<float2>(count, Allocator.Temp);
            float a0 = 0;
            float a1 = 0;
            float delta = largeRadius - smallRadius;
            
            for (int i = 0; i < count; ++i) {
                float r = smallRadius + delta * math.sin(a1);
                float2 xy = new float2(math.cos(a0), math.sin(a0));
                path[i] = center + r * xy;
                a0 += da0;
                a1 += da1;
            }
            
            var mesh = PathStroke.BuildMesh(path, true, strokeStyle, z, allocator);
            
            path.Dispose();
            
            return mesh;
        }
    }

}