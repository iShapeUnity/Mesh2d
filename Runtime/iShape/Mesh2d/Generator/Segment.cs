using Unity.Mathematics;

namespace iShape.Mesh2d {

    internal readonly struct Segment {
        
        internal readonly float2 a;
        internal readonly float2 b;
        internal readonly float2 direction;
        internal readonly float length;

        internal float2 ortho => new(-direction.y, direction.x);

        internal Segment(float2 a, float2 b) {
            float2 v = b - a;
            this.a = a;
            this.b = b;

            this.length = math.length(v);

            float x = v.x / length;
            float y = v.y / length;

            direction = new float2(x, y);
        }
    }

}