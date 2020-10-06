## Optimization of drawcalls when batching over 8000 house meshes
**Draw call:** each group of triangles drawn with the same material and texture properties<br />
 <br />
Max vert count per mesh depends on index buffer size. (Unity defaults to 16-bit)<br />
16-bit index buffer supports up to 65,535 vertices
32-bit index buffer supports up to 4 billion vertices<br />

**Dynamic batching** = 300 verts per batch.<br />
**CombineMesh** = 65,536 verts per batch. (16 bit index buffer)
![](images/house_batching.png)


# Dynamic Batching
* Batches: 233 (~198 fps) [~5.1ms]
* Tris: 880.4k
* Verts: 1.7M

# Static batching
* Batches: 1476 (~220 fps) [~4.6ms]
* Tris: 880.4k
* Verts: 1.7M

# No Batching
* Batches: 8615 (~265 fps) [~4.1ms]
* Tris: 880.4k
* Verts: 1.7M

# CombineMesh
* Batches: 64 (~350 fps) [~2.8ms]
* Tris: 1.6M
* Verts: 3.1M


