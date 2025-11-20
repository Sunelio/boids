using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour
{
    private Vector3 velocity;
    public BoidsManager manager;
    private int index;

    void Start()
    {
        manager = GetComponentInParent<BoidsManager>();
        if (!manager)
            Debug.LogError("Cant find boids Manager");
        else
        {
            index = manager.listBoids.Count;
            manager.listBoids.Add(this);
        }
        velocity = new Vector3(Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3));
    }

    void Update()
    {
        DirCalc();
    }

    private bool isClose(Boids a, float range)
    {
        float distSqrt = (a.transform.position - transform.position).sqrMagnitude;
        return distSqrt < range * range;
    }
    public Vector3 SeparationVector(Boids a)
    {
        Vector3 diff = transform.position - a.transform.position;
        float distSqr = diff.sqrMagnitude;
        return diff.normalized / Mathf.Max(distSqr, 0.0001f);
    }

    public Vector3 alignementVector(Boids a)
    {
        return a.velocity;
    }

    public Vector3 cohesionVector(Boids a)
    {
        return a.transform.position; 
    }

    public Vector3 AccelCalc(Vector3 separation, Vector3 alignement, Vector3 cohesion)
    {
        return manager.poidSeparation * separation +
           manager.poidalignement * alignement +
           manager.poidCohesion * cohesion;
    }

    public Vector3 BorderRepulsion()
    {
        float limite = manager.limite;
        float strenght = manager.poidRepulsion;
        Vector3 pos = transform.position;
        Vector3 steer = Vector3.zero;

        if (pos.x > limite) steer.x -= strenght;
        else if (pos.x < -limite) steer.x += strenght;

        if (pos.y > limite) steer.y -= strenght;
        else if (pos.y < -limite) steer.y += strenght;

        if (pos.z > limite) steer.z -= strenght;
        else if (pos.z < -limite) steer.z += strenght;

        return steer;
    }

    public void DirCalc()
    {
        int count = 0;
        Vector3 sep = Vector3.zero;
        Vector3 ali = Vector3.zero;
        Vector3 coh = Vector3.zero;

        foreach (Boids a in manager.listBoids)
        {
            if (a == this) continue;
            if (!isClose(a, manager.friendRange)) continue;

            count++;

            if (isClose(a, manager.avoidrange))
            {
                sep += SeparationVector(a);
            }

            ali += alignementVector(a);
            coh += cohesionVector(a);
        }

        if (count > 0)
        {
            sep = sep.normalized;
            ali = (ali / count) - velocity;
            coh = ((coh / count) - transform.position);
        }

        Vector3 accel = AccelCalc(sep, ali.normalized, coh.normalized);

        accel += BorderRepulsion();

        velocity += accel * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, manager.max);
        transform.position += velocity * Time.deltaTime;
    }
}
