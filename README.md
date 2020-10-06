Draw call: each group of triangles drawn with the same material and texture properties

Max vert count per mesh depends on index buffer size. (Unity defaults to 16-bit)

16-bit index buffer supports up to 65,535 vertices
32-bit index buffer supports up to 4 billion vertices



I was wondering if combine mesh is not an optimal way to batch,

I always wondered how batching works so I started running some tests.
I noticed: 

Dynamic batching = 300 verts per batch.
CombineMesh = 65,536 verts per batch. (16 bit index buffer)

#No Batching#
Batches: 8615 (~265 fps) [~4.1ms]
Tris: 880.4k
Verts: 1.7M

#Static batching#
Batches: 1476 (~220 fps) [~4.6ms]
Tris: 880.4k
Verts: 1.7M

#Dynamic Batching#
Batches: 233 (~198 fps) [~5.1ms]
Tris: 880.4k
Verts: 1.7M

#CombineMesh#
Batches: 64 (~350 fps) [~2.8ms]
Tris: 1.6M
Verts: 3.1M


I am curious if combine mesh is actually more performant than static batching?
