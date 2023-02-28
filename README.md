# Mesh2d
Mesh2d is a lightweight library for generating 2D stroke meshes in Unity. It build over Native Arrays and is compatible with Unity's Burst Compiler for improved performance.

## Installation
To use iShape.Mesh2d in your Unity project, follow these steps:

- Open your Unity project.
- In the top menu, select "Window" > "Package Manager".
- Click on the "+" button in the top-left corner of the Package Manager window.
- Select "Add package from git URL...".
- Enter the following URL: https://github.com/iShapeUnity/Mesh2d.git
- Click the "Add" button.
- Wait for the package to be imported.

## Usage
To use iShape.Mesh2d in your Unity project, you'll need to import the iShape.Mesh2d namespace in your scripts. Here's an example of how to use it:

```csharp
using iShape.Mesh2d;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Example : MonoBehaviour {
    void Start() {
        // Create a new NativeColorMesh with a capacity of 256 vertices
            var mesh = new NativeColorMesh(256, Allocator.Temp);
        
            // Generate a blue stroke mesh for an edge
            var edgeStyle = new StrokeStyle(0.4f);
            var edgeMesh = MeshGenerator.StrokeForEdge(new float2(3, 3), new float2(-3, 3), edgeStyle, 0, Allocator.Temp);
            mesh.AddAndDispose(edgeMesh, Color.blue);
            
            // Generate a magenta stroke mesh for a path
            var pathStyle = new StrokeStyle(0.5f);
            var path = new NativeArray<float2>(4, Allocator.Temp);
            path[0] = new float2(-4, 3);
            path[1] = new float2(-4,  -3);
            path[2] = new float2(4, -3);
            path[3] = new float2(4, 3);
            
            var pathMesh = MeshGenerator.StrokeByPath(path, false, pathStyle, 0, Allocator.Temp);
            mesh.AddAndDispose(pathMesh, Color.magenta);
            
            path.Dispose();

            // Generate a green stroke mesh for a rectangle
            var rectStyle = new StrokeStyle(0.2f);
            var rectMesh = MeshGenerator.StrokeForRect(float2.zero, new float2(4, 4), rectStyle, 0, Allocator.Temp);
            mesh.AddAndDispose(rectMesh, Color.green);

            // Generate a red stroke mesh for a circle
            var circleStyle = new StrokeStyle(0.1f);
            var circleMesh = MeshGenerator.StrokeForCircle(float2.zero, 1, 32, circleStyle, 0, Allocator.Temp);
            mesh.AddAndDispose(circleMesh, Color.red);
        
            // Generate a yellow stroke mesh for a soft star
            var starStyle = new StrokeStyle(0.05f);
            var starMesh = MeshGenerator.StrokeForSoftStar(float2.zero, 0.4f,0.7f, 64, starStyle, 0, Allocator.Temp);
            mesh.AddAndDispose(starMesh, Color.yellow);
            
            // Set the generated mesh as the MeshFilter's mesh
            var meshFilter = this.GetComponent<MeshFilter>();
            
            meshFilter.mesh = mesh.Convert();
    }
}
```

In this example, we're generating four different stroke meshes (for an edge, a rectangle, a circle, and a soft star) and adding them to a NativeColorMesh. Then, we're setting the resulting mesh as the MeshFilter's mesh.

For more information on how to use iShape.Mesh2d, please see [MeshGenerator](https://github.com/iShapeUnity/Mesh2d/blob/main/Runtime/iShape/Mesh2d/MeshGenerator.cs)

Minimal working project could be found at [Mesh2Test](https://github.com/iShapeUnity/Mesh2Test)
