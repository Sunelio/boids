using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class BoidVisualizer : MonoBehaviour
{
    public Entity boidEntity;
    private EntityManager entityManager;

    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    void Update()
    {
        if (entityManager.Exists(boidEntity))
        {
            var pos = entityManager.GetComponentData<LocalTransform>(boidEntity).Position;
            transform.position = pos;
        }
    }
}
