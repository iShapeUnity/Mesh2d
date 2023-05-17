using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace iShape.Mesh2d {

    public static class BoundaryUtil {

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
    }

}