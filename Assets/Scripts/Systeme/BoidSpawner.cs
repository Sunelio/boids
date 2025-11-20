using Unity.Rendering;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;


public class BoidSpawner : MonoBehaviour
{
    public GameObject boidPrefab;
    public int boidCount = 50;


    void Start()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        var entityManager = world.EntityManager;

        for (int i = 0; i < boidCount; i++)
        {
            // Crée une entité ECS
            Entity boid = entityManager.CreateEntity(
                typeof(LocalTransform),
                typeof(BoidVelocity),
                typeof(BoidForce)
            );

            float3 randomPos = new float3(
                UnityEngine.Random.Range(-10f, 10f),
                UnityEngine.Random.Range(0f, 5f),
                UnityEngine.Random.Range(-10f, 10f)
            );

            entityManager.SetComponentData(boid, new LocalTransform
            {
                Position = randomPos,
                Rotation = quaternion.identity,
                Scale = 1f
            });

            entityManager.SetComponentData(boid, new BoidVelocity { Value = float3.zero });
            entityManager.SetComponentData(boid, new BoidForce { Value = float3.zero });

            // Crée le GameObject visuel
            GameObject go = Instantiate(boidPrefab);
            var visualizer = go.AddComponent<BoidVisualizer>();
            visualizer.boidEntity = boid;
        }
    }
    
    
}
