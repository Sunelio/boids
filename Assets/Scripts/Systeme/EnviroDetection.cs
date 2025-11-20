using System.Collections.Generic;
using UnityEngine;

public class EnviroDetection : MonoBehaviour
{
    public GameObject prefabPoint;
    public List<GameObject> points;
    public int turnfraction;
    public int numPoints;
    public float distDetection;
   
    void Start()
    {
        GenerateFieldOfRay();
    }

    
    void Update()
    {
        
    }

    private void plotPoint(float x, float y , float z , Color color)
    {
        Physics.Raycast(this.transform.position, new Vector3(x, y, z), distDetection);
        Debug.DrawRay(this.transform.position, new Vector3(x, y, z), color);
    }
    private void GenerateFieldOfRay()
    {
        for (int i = 0; i < numPoints; i++)
        {
            float t = i / (numPoints - 1f);
            float inclination = Mathf.Acos(1 - 2 * t);
            float azimut = 2 * Mathf.PI * turnfraction * i;

            float x = Mathf.Sin(inclination) * Mathf.Cos(azimut);
            float y = Mathf.Sin(inclination) * Mathf.Sin(azimut);
            float z = Mathf.Cos(inclination);

             plotPoint(x,y,z, Color.red);
        }
    }
    
}
