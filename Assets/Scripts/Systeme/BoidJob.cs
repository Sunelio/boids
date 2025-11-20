using Unity.Transforms;
using Unity.Mathematics;
using Unity.Entities;


public struct BoidVelocity : IComponentData
{
    public float3 Value;
}

public struct BoidForce : IComponentData
{
    public float3 Value;
}

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateBefore(typeof(BoidMovementSystem))]
public partial struct BoidSeparationSystem : ISystem
{
    
    public void OnCreate(ref SystemState state) {}
    public void OnDestroy(ref SystemState state) {}
    public void OnUpdate(ref SystemState state )
    {
        float separationDistance = 5.5f;
        float deltaTime = SystemAPI.Time.DeltaTime;

        foreach (var force in SystemAPI.Query<RefRW<BoidForce>>())
        {
            force.ValueRW.Value = float3.zero;
        }
        foreach (var (transA, velA, forceA) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<BoidVelocity>, RefRW<BoidForce>>())
        {
            float3 posA = transA.ValueRO.Position;
            float3 separation = float3.zero;
            int count = 0;

            foreach (var transB in SystemAPI.Query<RefRO<LocalTransform>>())
            {
                float3 posB = transB.ValueRO.Position;
                float dist = math.distance(posA, posB);

                if (dist > 0 && dist < separationDistance)
                {
                    separation += math.normalize(posA - posB) / dist;
                    count++;
                }
            }

            if (count > 0)
                forceA.ValueRW.Value = (separation / count)*deltaTime;
        }
        
    }
}

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateBefore(typeof(BoidMovementSystem))]
public partial struct BoidAlignementSystem : ISystem
{
    public void OnCreate(ref SystemState state) {}
    public void OnDestroy(ref SystemState state) {}
    public void OnUpdate(ref SystemState state)
    {
        float neighborDistance = 15f;
        float deltaTime = SystemAPI.Time.DeltaTime;
        
        foreach (var (transA, velA, forceA) in SystemAPI
                     .Query<RefRO<LocalTransform>, RefRO<BoidVelocity>, RefRW<BoidForce>>())
        {
            float3 posA = transA.ValueRO.Position;
            float3 velAvg = float3.zero;
            int count = 0;
            
            foreach (var (transB, velB)
                     in SystemAPI.Query<RefRO<LocalTransform>, RefRO<BoidVelocity>>())
            {
                float3 posB = transB.ValueRO.Position;
                float dist = math.distance(posA, posB);

                if (dist > 0 && dist < neighborDistance)
                {
                    velAvg += velB.ValueRO.Value;
                    count++;
                }
            }

            if (count > 0)
            {
                velAvg /= count;
                float3 alignForce = (velAvg - velA.ValueRO.Value);
                forceA.ValueRW.Value += alignForce * deltaTime;
            }
        }
    }
}

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateBefore(typeof(BoidMovementSystem))]
public partial struct BoidCohesionSystem : ISystem
{
    public void OnCreate(ref SystemState state) {}
    public void OnDestroy(ref SystemState state) {}
    public void OnUpdate(ref SystemState state)
    {
        float cohesionDistance = 10f;
        float weightCoh = 1f;
        float deltaTime = SystemAPI.Time.DeltaTime;

        foreach (var (transA, forceA) in SystemAPI
                     .Query<RefRO<LocalTransform>, RefRW<BoidForce>>())
        {
            float3 posA = transA.ValueRO.Position;
            float3 posAvg = float3.zero;
            int count = 0;
            
            foreach (var transB
                     in SystemAPI.Query<RefRO<LocalTransform>>())
            {
                float3 posB = transB.ValueRO.Position;
                float dist = math.distance(posA, posB);

                if (dist > 0 && dist < cohesionDistance)
                {
                    posAvg += posB;
                    count++;
                }
            }

            if (count > 0)
            {
                posAvg /= count;
                float3 cohesionForce = (posAvg - posA)*weightCoh;
                forceA.ValueRW.Value += cohesionForce * deltaTime;
            }
        }
    }
}

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct BoidMovementSystem : ISystem
{
    public void OnCreate(ref SystemState state) {}
    public void OnDestroy(ref SystemState state) {}
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        float maxSpeed = 50f;      
        float maxForce = 60f;

        foreach (var (trans,vel,force) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<BoidVelocity>, RefRW<BoidForce>>())
        {
            float3 currVel = vel.ValueRW.Value;
            float3 applyForce = force.ValueRW.Value;
            
            if(math.length(applyForce) > maxForce)
                applyForce = math.normalize(applyForce) * maxForce;
            
            currVel += applyForce * deltaTime;
            
            if(math.length(currVel) > maxSpeed)
                currVel = math.normalize(currVel) * maxSpeed;
            
            vel.ValueRW.Value = currVel;
            
            trans.ValueRW.Position += currVel * deltaTime;
            
            force.ValueRW.Value = float3.zero;
        }
    }
}
