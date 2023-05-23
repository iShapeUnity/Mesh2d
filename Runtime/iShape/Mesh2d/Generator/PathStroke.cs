using Unity.Collections;
using Unity.Mathematics;

namespace iShape.Mesh2d {

    internal static class PathStroke {
        
        private const float eA = 0.125f * math.PI;
        
        internal static NativePrimitiveMesh BuildMesh(NativeArray<float2> path, bool isClosed, StrokeStyle strokeStyle, float z, Allocator allocator) {
            if (isClosed) {
                return ClosedStroke(path, strokeStyle, z, allocator);
            } else {
                return OpenStroke(path, strokeStyle, z, allocator);
            }
        }

        private static NativePrimitiveMesh ClosedStroke(NativeArray<float2> path, StrokeStyle strokeStyle, float z, Allocator allocator) {
            int n = path.Length;

            var vertices = new NativeList<float3>(4 * n, allocator);
            var triangles = new NativeList<int>(12 * n, allocator);
            
            float r = 0.5f * strokeStyle.Width;
            
            float2 a = path[n - 2];
            float2 b = path[n - 1];
            
            Segment seg0 = new Segment(a, b);

            for(int i = 0; i < n; ++i) {
                float2 c = path[i];
                Segment seg1 = new Segment(seg0.b, c);
                JoinSegment(ref vertices, ref triangles, seg0, r, strokeStyle.Step, z);

                bool isClose = i + 1 == n;
                JoinJoint(ref vertices, ref triangles, seg0, seg1, r, z, isClose);

                seg0 = seg1;
            }

            return new NativePrimitiveMesh(vertices, triangles);
        }
        
        private static NativePrimitiveMesh OpenStroke(NativeArray<float2> path, StrokeStyle strokeStyle, float z, Allocator allocator) {
            int n = path.Length;

            var vertices = new NativeList<float3>(4 * n, allocator);
            var triangles = new NativeList<int>(12 * n, allocator);
            
            float r = 0.5f * strokeStyle.Width;
            
            float2 a = path[0];
            float2 b = path[1];
            
            Segment seg0 = new Segment(a, b);

            if (strokeStyle.StartCap) {
                JoinCap(ref vertices, ref triangles,seg0.ortho, a, r, z);                
            }
            JoinSegment(ref vertices, ref triangles, seg0, r, strokeStyle.Step, z);

            for(int i = 2; i < n; ++i) {
                float2 c = path[i];
                Segment seg1 = new Segment(seg0.b, c);
                JoinJoint(ref vertices, ref triangles, seg0, seg1, r, z, false);
                JoinSegment(ref vertices, ref triangles, seg1, r, strokeStyle.Step, z);

                seg0 = seg1;
            }

            if (strokeStyle.EndCap) {
                JoinCap(ref vertices, ref triangles,-seg0.ortho, seg0.b, r, z);                
            }

            return new NativePrimitiveMesh(vertices, triangles);
        }

        private static void JoinSegment(ref NativeList<float3> vertices, ref NativeList<int> triangles, Segment segment, float r, float step, float z) {
            int n = (int)(segment.length / step + 0.5f);
            float ds = segment.length / n;

            float2 ortho = r * segment.ortho;
            float2 sTop = segment.a + ortho;
            float2 sBot = segment.a - ortho;

            int index = vertices.Length;
            
            vertices.Add(new float3(sTop, z));
            vertices.Add(new float3(sBot, z));

            int i0Top = index;
            int i0Bot = index + 1;

            int i1Top = index + 2;
            int i1Bot = index + 3;

            float s = ds;
            
            index += 4;
            
            for (int i = 1; i < n; ++i) {
                float2 dir = s * segment.direction;
                float2 pTop = sTop + dir;
                float2 pBot = sBot + dir;
                
                vertices.Add(new float3(pTop, z));
                vertices.Add(new float3(pBot, z));

                triangles.Add(i0Bot);
                triangles.Add(i0Top);
                triangles.Add(i1Top);
                
                triangles.Add(i0Bot);
                triangles.Add(i1Top);
                triangles.Add(i1Bot);

                i0Top = i1Top;
                i0Bot = i1Bot;
                
                i1Top = index;
                i1Bot = index + 1;

                index += 2;
                
                s += ds;
            }

            triangles.Add(i0Bot);
            triangles.Add(i0Top);
            triangles.Add(i1Top);
                
            triangles.Add(i0Bot);
            triangles.Add(i1Top);
            triangles.Add(i1Bot);
            
            sTop = segment.b + ortho;
            sBot = segment.b - ortho;
            
            vertices.Add(new float3(sTop, z));
            vertices.Add(new float3(sBot, z));
        }

        private static void JoinJoint(ref NativeList<float3> vertices, ref NativeList<int> triangles, Segment seg0, Segment seg1, float r, float z, bool isLast) {
            float2 v0 = seg0.ortho;
            float2 v1 = seg1.ortho;
            float cross = math2d.cross(v0, v1);
            const float eCross = 0.00_0001f;
            if (math.abs(cross) < eCross) {
                return;
            }

            float dot = math.dot(v0, v1);

            float angle = math.acos(dot);

            int n = (int)(angle / eA + 0.5f);

            if (cross > 0) {
                int prev = vertices.Length - 1;
                
                int middle = vertices.Length;
                vertices.Add(new float3(seg0.b, z));
                

                int index = middle + 1;
                if (n > 1) {
                    float dA = angle / n;

                    float2x2 m = math2d.rotationMatrix(dA);
                    float2 ortho = r * v0;
                    for (int i = 1; i < n; ++i) {
                        ortho = math.mul(m, ortho);
                        float2 p = seg0.b - ortho;

                        triangles.Add(prev);
                        triangles.Add(middle);
                        triangles.Add(index);
                    
                        vertices.Add(new float3(p, z));

                        prev = index;
                        
                        ++index;
                    }
                }

                if (isLast) {
                    index = 1;
                } else {
                    ++index;                    
                }

                triangles.Add(prev);
                triangles.Add(middle);
                triangles.Add(index);
            } else {
                int prev = vertices.Length - 2;
                
                int middle = vertices.Length;
                vertices.Add(new float3(seg0.b, z));
                

                int index = middle + 1;
                if (n > 1) {
                    float dA = angle / n;

                    float2x2 m = math2d.rotationMatrix(-dA);

                    float2 ortho = r * v0;
                    for (int i = 1; i < n; ++i) {
                        ortho = math.mul(m, ortho);
                        float2 p = seg0.b + ortho;

                        triangles.Add(prev);
                        triangles.Add(index);
                        triangles.Add(middle);
                    
                        vertices.Add(new float3(p, z));

                        prev = index;
                        
                        ++index;
                    }
                }
                
                if (isLast) {
                    index = 0;
                }

                triangles.Add(prev);
                triangles.Add(index);
                triangles.Add(middle);
            }
        }

        private static void JoinCap(ref NativeList<float3> vertices, ref NativeList<int> triangles, float2 vec, float2 pos, float r, float z) {
            const int n = (int)(math.PI / eA + 0.1f);
            const float dA = math.PI / n;

            float2x2 m = math2d.rotationMatrix(dA);
            float2 ortho = r * vec;

            int prev;
            int close;
            if (vertices.Length == 0) {
                prev = n;
                close = n + 1;
            } else {
                prev = vertices.Length - 1;
                close = prev - 1;
            }

            int middle = vertices.Length;
            vertices.Add(new float3(pos, z));

            int index = middle + 1;
            
            for (int i = 1; i < n; ++i) {
                ortho = math.mul(m, ortho);
                float2 p = pos + ortho;

                triangles.Add(prev);
                triangles.Add(middle);
                triangles.Add(index);
                    
                vertices.Add(new float3(p, z));

                prev = index;
                
                ++index;
            }
            
            triangles.Add(prev);
            triangles.Add(middle);
            triangles.Add(close);
        }
    }

}