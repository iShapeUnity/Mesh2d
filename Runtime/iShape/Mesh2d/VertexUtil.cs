using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace iShape.Mesh2d {

    public static class VertexUtil {

        public static Bounds Bounds(this NativeArray<float3> vertices) {
            var v = vertices[0];
            float3 min = v;
            float3 max = v;
            
            for (int i = 1; i < vertices.Length; ++i) {
                v = vertices[i];
                min = math.min(min, v);
                max = math.max(max, v);
            }

            var size = max - min;
            var center = 0.5f * (min + max);
            
            return new Bounds(center, size);
        }
        
        public static Bounds Bounds(this NativeList<float3> vertices) {
            var v = vertices[0];
            float3 min = v;
            float3 max = v;
            
            for (int i = 1; i < vertices.Length; ++i) {
                v = vertices[i];
                min = math.min(min, v);
                max = math.max(max, v);
            }

            var size = max - min;
            var center = 0.5f * (min + max);
            
            return new Bounds(center, size);
        }

        public static NativeArray<Vector2> ConvertToArray(this NativeList<Vector2> vertices, Allocator allocator) {
            var result = new NativeArray<Vector2>(vertices, allocator);
            vertices.Dispose();
            return result;
        }

        public static NativeArray<float2> ConvertToFloat(this NativeList<Vector2> vertices, Allocator allocator) {
            var result = new NativeArray<float2>(vertices.AsArray().Reinterpret<float2>(), allocator);
            vertices.Dispose();
            return result;
        }
    }

}