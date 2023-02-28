using UnityEngine.Assertions;

namespace iShape.Mesh2d {

    public struct StrokeStyle {
        
        public float Width;
        internal float Step;
        public int PointCount;

        public StrokeStyle(float Width = 0.1f, float MinSegmentStep = 0.1f, int PointCount = 16) {
            Assert.IsTrue(Width > 0, "Width must be more 0");
            this.Width = Width;
            float s = 2f * Width;
            this.Step = s < MinSegmentStep ? MinSegmentStep : s;
            this.PointCount = PointCount;
        }
    }

}