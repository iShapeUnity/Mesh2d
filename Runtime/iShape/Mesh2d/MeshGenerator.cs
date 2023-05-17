using Unity.Collections;
using Unity.Mathematics;

namespace iShape.Mesh2d {

    /// <summary>
    /// Contains static methods for generating primitive stroke meshes in 2D space.
    /// </summary>
    public static class MeshGenerator {

        /// <summary>
        /// Generates a stroke mesh for a sequence of points.
        /// </summary>
        /// <param name="path">The points of the path.</param>
        /// <param name="isClosed">Is the path closed or not.</param>
        /// <param name="strokeStyle">The style of the stroke to apply.</param>
        /// <param name="z">The Z position of the stroke.</param>
        /// <param name="allocator">The memory allocator to use.</param>
        /// <returns>A mesh representing the stroke.</returns>
        public static NativePrimitiveMesh StrokeByPath(NativeArray<float2> path, bool isClosed, StrokeStyle strokeStyle, float z, Allocator allocator) {
            return PathStroke.BuildMesh(path, isClosed, strokeStyle, z, allocator);
        }

        /// <summary>
        /// Generates a stroke mesh for a straight line segment between two points.
        /// </summary>
        /// <param name="start">The start point of the line segment.</param>
        /// <param name="end">The end point of the line segment.</param>
        /// <param name="strokeStyle">The style of the stroke to apply.</param>
        /// <param name="z">The Z position of the stroke.</param>
        /// <param name="allocator">The memory allocator to use.</param>
        /// <returns>A mesh representing the stroke.</returns>
        public static NativePrimitiveMesh StrokeForEdge(float2 start, float2 end, StrokeStyle strokeStyle, float z, Allocator allocator) {
            return PrimitiveStroke.StrokeForEdge(start, end, strokeStyle, z, allocator);
        }

        /// <summary>
        /// Generates a stroke mesh for a rectangle.
        /// </summary>
        /// <param name="center">The center point of the rectangle.</param>
        /// <param name="size">The size of the rectangle.</param>
        /// <param name="strokeStyle">The style of the stroke to apply.</param>
        /// <param name="z">The Z position of the stroke.</param>
        /// <param name="allocator">The memory allocator to use.</param>
        /// <returns>A mesh representing the stroke.</returns>
        public static NativePrimitiveMesh StrokeForRect(float2 center, float2 size, StrokeStyle strokeStyle, float z, Allocator allocator) {
            return PrimitiveStroke.StrokeForRect(center, size, strokeStyle, z, allocator);
        }
        
        /// <summary>
        /// Generates a stroke mesh for a circle.
        /// </summary>
        /// <param name="center">The center point of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="count">The number of segments to use when generating the circle.</param>
        /// <param name="strokeStyle">The style of the stroke to apply.</param>
        /// <param name="z">The Z position of the stroke.</param>
        /// <param name="allocator">The memory allocator to use.</param>
        /// <returns>A mesh representing the stroke.</returns>
        public static NativePrimitiveMesh StrokeForCircle(float2 center, float radius, int count, StrokeStyle strokeStyle, float z, Allocator allocator) {
            return PrimitiveStroke.StrokeForCircle(center, radius, count, strokeStyle, z, allocator);
        }
        
        /// <summary>
        /// Generates a stroke mesh for a soft star.
        /// </summary>
        /// <param name="center">The center point of the circle.</param>
        /// <param name="smallRadius">The inner radius of the star.</param>
        /// <param name="largeRadius">The outer radius of the star.</param>
        /// <param name="count">The number of segments to use when generating the star.</param>
        /// <param name="strokeStyle">The style of the stroke to apply.</param>
        /// <param name="z">The Z position of the stroke.</param>
        /// <param name="allocator">The memory allocator to use.</param>
        /// <returns>A mesh representing the stroke.</returns>
        public static NativePrimitiveMesh StrokeForSoftStar(float2 center, float smallRadius, float largeRadius, int count, StrokeStyle strokeStyle, float z, Allocator allocator) {
            return PrimitiveStroke.StrokeForSoftStar(center, smallRadius, largeRadius, count, strokeStyle, z, allocator);
        }
        
        /// <summary>
        /// Generates a shape mesh for a rectangle.
        /// </summary>
        /// <param name="center">The center point of the rectangle.</param>
        /// <param name="size">The size of the rectangle.</param>
        /// <param name="z">The Z position of the shape.</param>
        /// <param name="allocator">The memory allocator to use.</param>
        /// <returns>A mesh representing the shape.</returns>
        public static StaticPrimitiveMesh FillRect(float2 center, float2 size, float z, Allocator allocator) {
            return PrimitiveShape.Rect(center, size, z, allocator);
        }
        
        /// <summary>
        /// Generates a shape mesh for a circle.
        /// </summary>
        /// <param name="center">The center point of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="count">The number of segments to use when generating the circle.</param>
        /// <param name="z">The Z position of the shape.</param>
        /// <param name="allocator">The memory allocator to use.</param>
        /// <returns>A mesh representing the shape.</returns>
        public static StaticPrimitiveMesh FillCircle(float2 center, float radius, int count, float z, Allocator allocator) {
            return PrimitiveShape.Circle(center, radius, count, z, allocator);
        }
    }

}