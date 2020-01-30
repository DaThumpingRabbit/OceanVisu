using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractGraph : MonoBehaviour
{
    protected GameObject tMap;
    protected TriangleMapFromLatLong map;

    protected bool isRayCasting;

    // Start is called before the first frame update
    void Start()
    {
        tMap = GameObject.Find("TrangleMapFromLatLong");
        map = tMap.GetComponent<TriangleMapFromLatLong>();
        isRayCasting = false;
    }

    // Update is called once per frame
    void Update()
    {
        var mousePosition = Input.mousePosition;

        if (Input.GetKeyDown(KeyCode.LeftControl) && !isRayCasting)
        {
            isRayCasting = true;

            PointMap[] planets = map.GetPlanets();
            for (int i = 0; i < planets.Length; i++)
            {
                planets[i].RemoveLaser();
            }

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if (hit.collider.tag == "graph")
                {
                    float distMin = 10000;
                    Vector3 pointProche = new Vector3();

                    Dictionary<Vector3, List<Vector3>> graphToSphere = hit.collider.transform.parent.GetComponent<PointGraphs>().getPointsDict();
                    foreach (Vector3 point in graphToSphere.Keys)
                    {
                        float dist = Vector3.Distance(hit.collider.transform .position + point, hit.point);
                        if (dist < distMin)
                        {
                            pointProche = point;
                            distMin = dist;
                        }
                    }
                    
                    for (int i = 0; i < planets.Length; i++)
                    {
                        planets[i].SetLaser(graphToSphere[pointProche], true);
                    }
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftControl) && isRayCasting)
        {
            isRayCasting = false;
        }



        if (Input.GetKey(KeyCode.LeftAlt))
        {

            PointMap[] planets = map.GetPlanets();

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if (hit.collider.tag == "graph")
                {
                    float distMin = 10000;
                    Vector3 pointProche = new Vector3();

                    Dictionary<Vector3, List<Vector3>> graphToSphere = hit.collider.transform.parent.GetComponent<PointGraphs>().getPointsDict();
                    foreach (Vector3 point in graphToSphere.Keys)
                    {
                        float dist = Vector3.Distance(hit.collider.transform.position + point, hit.point);
                        if (dist < distMin)
                        {
                            pointProche = point;
                            distMin = dist;
                        }
                    }

                    for (int i = 0; i < planets.Length; i++)
                    {
                        planets[i].SetLaser(graphToSphere[pointProche], true);
                    }
                }
            }
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            PointMap[] planets = map.GetPlanets();

            for (int i = 0; i < planets.Length; i++)
            {
                planets[i].RemoveLaser();
            }
        }
    }
}
