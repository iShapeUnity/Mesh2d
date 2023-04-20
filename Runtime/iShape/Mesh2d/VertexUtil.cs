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
            var vSize = new Vector3(size.x, size.y, 1f);
            var vCenter = new Vector3(0.5f * (max.x + min.x), 0.5f * (max.y + min.y), 0f);
            
            return new Bounds(vCenter, vSize);
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
        
        public static NativeArray<float2> ConvertToArray(this NativeList<float2> vertices, Allocator allocator) {
            var result = new NativeArray<float2>(vertices, allocator);
            vertices.Dispose();
            return result;
        }
        
        public static Vector3[] ToVertices(this NativeArray<float3> vertices) {
            var vectors = new NativeArray<Vector3>(vertices.Reinterpret<Vector3>(), Allocator.Temp);
            var array = vectors.ToArray();
            vectors.Dispose();
            return array;
        }
        
        public static Color[] ToColors(this NativeArray<float4> colors) {
            var vectors = new NativeArray<Color>(colors.Reinterpret<Color>(), Allocator.Temp);
            var array = vectors.ToArray();
            vectors.Dispose();
            return array;
        }
        
        public static Vector3[] ToVertices(this NativeList<float3> vertices) {
            var vectors = new NativeArray<Vector3>(vertices.AsArray().Reinterpret<Vector3>(), Allocator.Temp);
            var array = vectors.ToArray();
            vectors.Dispose();
            return array;
        }
        
        public static Color[] ToColors(this NativeList<float4> colors) {
            var vectors = new NativeArray<Color>(colors.AsArray().Reinterpret<Color>(), Allocator.Temp);
            var array = vectors.ToArray();
            vectors.Dispose();
            return array;
        }
    }

}