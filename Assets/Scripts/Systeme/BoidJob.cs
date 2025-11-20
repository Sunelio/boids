using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public struct BoidJob : IJobParallelFor
{
    public NativeArray<float3> positions;
    public NativeArray<float3> velocities;

    public float separationWeight;
    public float alignmentWeight;
    public float cohesionWeight;

    public void Execute(int i)
    {
        float3 pos = positions[i];
        float3 vel = velocities[i];

        float3 separation = float3.zero;
        float3 alignment = float3.zero;
        float3 cohesion = float3.zero;

        int count = positions.Length;
        for (int j = 0; j < count; j++)
        {
            if (i == j) continue;

            float3 toOther = positions[j] - pos;
            float distSq = math.lengthsq(toOther);

            if (distSq < 2f) separation -= toOther;
            if (distSq < 4f) alignment += velocities[j];
            if (distSq < 4f) cohesion += positions[j];
        }

        vel += separation * separationWeight +
               alignment * alignmentWeight +
               cohesion * cohesionWeight;

        vel = math.normalize(vel);

        positions[i] = pos + vel * 0.05f;
        velocities[i] = vel;
    }
}
