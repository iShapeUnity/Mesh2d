using Unity.Mathematics;

namespace iShape.Mesh2d {

    internal readonly struct ColorVertex {
        
        private readonly float3 position;
        private readonly float4 color;

        internal ColorVertex(float3 position, float4 color) {
            this.position = position;
            this.color = color;
        }
    }

}