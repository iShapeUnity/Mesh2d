using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace iShape.Mesh2d {

    public static class math2d {
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float cross(float2 a, float2 b)
        {
            return a.x * b.y - a.y * b.x;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float2x2 rotationMatrix(float angleInRadians)
        {
            float cs = math.cos(angleInRadians);
            float sn = math.sin(angleInRadians);

            return new float2x2(cs, -sn, sn, cs);
        }
    }

}