using NUnit.Framework;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class BoidsManager : MonoBehaviour
{
    // public Transform[] boids;
    // public float separationWeight = 1f;
    // public float alignmentWeight = 1f;
    // public float cohesionWeight = 1f;
    // public float worldSize = 5f;
    // public int boidCount => boids.Length;
    //
    // NativeArray<float3> positions;
    // NativeArray<float3> velocities;
    //
    // void Start()
    // {
    //     positions = new NativeArray<float3>(boids.Length, Allocator.Persistent);
    //     velocities = new NativeArray<float3>(boids.Length, Allocator.Persistent);
    // }
    //
    // // Update is called once per frame
    // void Update()
    // {
    //     BoidJob job = new BoidJob
    //     {
    //         positions = positions,
    //         velocities = velocities,
    //         separationWeight = separationWeight,
    //         alignmentWeight = alignmentWeight,
    //         cohesionWeight = cohesionWeight,
    //     };
    //
    //     JobHandle handle = job.Schedule(boidCount, 64);
    //
    //     handle.Complete();
    //
    //     for (int i = 0; i < boids.Length; i++)
    //     {
    //         boids[i].position = positions[i];
    //         boids[i].forward = velocities[i];
    //     }
    // }
    //
    // void OnDestroy()
    // {
    //     positions.Dispose();
    //     velocities.Dispose();
    // }
}
