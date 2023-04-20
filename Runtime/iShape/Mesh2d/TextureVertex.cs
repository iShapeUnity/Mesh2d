using Unity.Mathematics;

namespace iShape.Mesh2d {

    internal readonly struct TextureVertex {
        
        private readonly float3 position;
        private readonly float2 uv;

        internal TextureVertex(float3 position, float2 uv) {
            this.position = position;
            this.uv = uv;
        }
    }

}