using System.Runtime.CompilerServices;
using UnityEngine;

namespace iShape.Mesh2d {

    public static class MeshExtension {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetAndDispose(this Mesh mesh, NativeColorMesh nativeMesh) {
            nativeMesh.Fill(mesh);
            nativeMesh.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetAndDispose(this Mesh mesh, StaticPrimitiveMesh nativeMesh) {
            nativeMesh.Fill(mesh);
            nativeMesh.Dispose();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetAndDispose(this Mesh mesh, NativePrimitiveMesh nativeMesh, Color color) {
            nativeMesh.Fill(mesh, color);
            nativeMesh.Dispose();
        }
        
    }

}