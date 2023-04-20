using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace iShape.Mesh2d {

    public static class math2d {

        private const float eps = 0.000001f;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float cross(float2 a, float2 b) {
            return a.x * b.y - a.y * b.x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2x2 rotationMatrix(float angleInRadians) {
            float cs = math.cos(angleInRadians);
            float sn = math.sin(angleInRadians);

            return new float2x2(cs, -sn, sn, cs);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2 Ortho(this float2 vec, bool clockwise) {
            if (math.abs(vec.x) < eps) {
                if (clockwise == vec.y > 0) {
                    return new float2(1, 0);
                } else {
                    return new float2(-1, 0);
                }
            }

            float k = vec.y / vec.x;
            float qy = 1 / (1 + k * k);
            float y0 = math.sqrt(qy);
            float x0 = -k * y0;

            float ab = vec.x * y0 - vec.y * x0;
            if (ab < 0 == clockwise) {
                return new float2(x0, y0);
            } else {
                return new float2(-x0, -y0);
            }
        }
    }

}