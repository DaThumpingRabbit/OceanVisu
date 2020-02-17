using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointGraphs : MonoBehaviour
{
    public GameObject axe;
    protected GameObject redAxe;
    protected GameObject greenAxe;
    protected GameObject blueAxe;

    public GameObject xLabel;
    public GameObject yLabel;
    public GameObject xMinLabel;
    public GameObject yMinLabel;
    public GameObject xMaxLabel;
    public GameObject yMaxLabel;

    protected int nbTime = 0;
    protected int nbDepth = 0;
    protected int nbX = 0;
    protected int nbY = 0;
    protected int activeSlice = -1;
    protected int activeTime = 0;
    protected Vector3[][] points;
    protected Shader shader;
    /// <summary>
    /// /////////////////ADDED THIS BOOL
    /// </summary>
    protected bool hidden;

    public GameObject[][] pointGroups;

    protected Dictionary<Vector3, List<Vector3>> graphToSphere = new Dictionary<Vector3, List<Vector3>>();
    protected Dictionary<Vector3, List<Vector3>> graphToPlane = new Dictionary<Vector3, List<Vector3>>();


    public void Start()
    {
        hidden = false;
        HideOrShow(true);


        axe = Resources.Load("Axe", typeof(GameObject)) as GameObject;

        redAxe = (GameObject)Instantiate(axe, new Vector3(0, 0, 0), new Quaternion());
        greenAxe = (GameObject)Instantiate(axe, new Vector3(0, 0, 0), new Quaternion());
        blueAxe = (GameObject)Instantiate(axe, new Vector3(0, 0, 0), new Quaternion());
        
        redAxe.GetComponent<Renderer>().material = Resources.Load("RedAxe", typeof(Material)) as Material;
        greenAxe.GetComponent<Renderer>().material = Resources.Load("GreenAxe", typeof(Material)) as Material;
        blueAxe.GetComponent<Renderer>().material = Resources.Load("BlueAxe", typeof(Material)) as Material;
        
        redAxe.transform.parent = this.transform;
        greenAxe.transform.parent = this.transform;
        blueAxe.transform.parent = this.transform;
        
        redAxe.transform.localScale = new Vector3(5, 175, 5);
        greenAxe.transform.localScale = new Vector3(5, 180, 5);
        blueAxe.transform.localScale = new Vector3(5, 75, 5);
        
        redAxe.transform.Rotate(new Vector3(0, 0, -90));
        blueAxe.transform.Rotate(new Vector3(90, 0, 0));

        redAxe.transform.Translate(transform.position.x + 175, transform.position.y, transform.position.z, Space.World);
        blueAxe.transform.Translate(transform.position.x, transform.position.y, transform.position.z + 75, Space.World);
        greenAxe.transform.Translate(transform.position.x, transform.position.y + 180, transform.position.z, Space.World);
    }

    public void Update()
    {
        /*
        redAxe.transform.Translate(transform.position + new Vector3(175, 0, 0));
        blueAxe.transform.Translate(transform.position + new Vector3(0, 0, 75));
        greenAxe.transform.Translate(transform.position + new Vector3(0, 180, 0));
        */
    }

    public void SetLabels(string xName, string yName, int xMin, int xMax, int yMin, int yMax)
    {
        xLabel = new GameObject();
        xLabel.AddComponent<TextMesh>();
        xLabel.GetComponent<TextMesh>().text = xName;
        xLabel.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        xLabel.GetComponent<TextMesh>().fontSize = 400;
        xLabel.transform.Translate(transform.position.x + 110, transform.position.y - 10, transform.position.z, Space.World);
        xLabel.transform.parent = this.transform;

        yLabel = new GameObject();
        yLabel.AddComponent<TextMesh>();
        yLabel.GetComponent<TextMesh>().text = yName;
        yLabel.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        yLabel.GetComponent<TextMesh>().fontSize = 400;
        yLabel.transform.Translate(transform.position.x - 70, transform.position.y + 180, transform.position.z, Space.World);
        yLabel.transform.parent = this.transform;

        xMinLabel = new GameObject();
        xMinLabel.AddComponent<TextMesh>();
        xMinLabel.GetComponent<TextMesh>().text = xMin.ToString();
        xMinLabel.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        xMinLabel.GetComponent<TextMesh>().fontSize = 400;
        xMinLabel.transform.Translate(transform.position.x, transform.position.y - 10, transform.position.z, Space.World);
        xMinLabel.transform.parent = this.transform;

        xMaxLabel = new GameObject();
        xMaxLabel.AddComponent<TextMesh>();
        xMaxLabel.GetComponent<TextMesh>().text = xMax.ToString();
        xMaxLabel.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        xMaxLabel.GetComponent<TextMesh>().fontSize = 400;
        xMaxLabel.transform.Translate(transform.position.x + 320, transform.position.y - 10, transform.position.z, Space.World);
        xMaxLabel.transform.parent = this.transform;

        yMinLabel = new GameObject();
        yMinLabel.AddComponent<TextMesh>();
        yMinLabel.GetComponent<TextMesh>().text = yMin.ToString();
        yMinLabel.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        yMinLabel.GetComponent<TextMesh>().fontSize = 400;
        yMinLabel.transform.Translate(transform.position.x - 20, transform.position.y + 20, transform.position.z, Space.World);
        yMinLabel.transform.parent = this.transform;

        yMaxLabel = new GameObject();
        yMaxLabel.AddComponent<TextMesh>();
        yMaxLabel.GetComponent<TextMesh>().text = yMax.ToString();
        yMaxLabel.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        yMaxLabel.GetComponent<TextMesh>().fontSize = 400;
        yMaxLabel.transform.Translate(transform.position.x - 40, transform.position.y + 360, transform.position.z, Space.World);
        yMaxLabel.transform.parent = this.transform;
    }

    internal Dictionary<Vector3, List<Vector3>> getPointsDict()
    {
        return graphToSphere;
    }

    internal Dictionary<Vector3, List<Vector3>> getPointsDictPlane()
    {
        return graphToPlane;
    }

    public void SetShader(Shader shader)
    {
        this.shader = shader;
    }

    public void initPointGroups(int nbTime, int nbDepth, int nbY, int nbX)
    {
        this.nbTime = nbTime;
        activeTime = nbTime - 1;
        this.nbDepth = nbDepth;
        this.nbY = nbY;
        this.nbX = nbX;
        pointGroups = new GameObject[nbTime][];
        points = new Vector3[nbDepth][];
        for (int t = 0; t < nbTime; t++)
        {
            pointGroups[t] = new GameObject[nbDepth];
        }
    }

    public virtual void initSlice(int t, int depth, Vector3[] pPoints, int[] indices, Color[] colors, MeshTopology meshTopology)
    {
        pointGroups[t][depth] = new GameObject();
        pointGroups[t][depth].AddComponent<MeshFilter>();
        pointGroups[t][depth].AddComponent<MeshRenderer>();
        points[depth] = pPoints;
        Mesh mesh = new Mesh();
        mesh.vertices = pPoints;
        mesh.colors = colors;
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.SetIndices(indices, meshTopology, 0);
        pointGroups[t][depth].GetComponent<MeshFilter>().mesh = mesh;
        pointGroups[t][depth].GetComponent<MeshRenderer>().motionVectorGenerationMode = MotionVectorGenerationMode.Object;
        Renderer rend = pointGroups[t][depth].GetComponent<Renderer>();
        rend.material.shader = shader;
        pointGroups[t][depth].transform.parent = transform;
        pointGroups[t][depth].SetActive(false);
        
        pointGroups[t][depth].AddComponent<MeshCollider>();
        pointGroups[t][depth].GetComponent<MeshCollider>().sharedMesh = mesh;
        /*
        pointGroups[t][depth].GetComponent<MeshCollider>().convex = true;
        pointGroups[t][depth].GetComponent<MeshCollider>().isTrigger = true;
        */
        pointGroups[t][depth].tag = "graph";
    }

    public void ActivateSlice()
    {
        if (activeSlice == -1)
        {
            for (int d = 0; d < nbDepth; d++)
            {
                pointGroups[activeTime][d].SetActive(false);
            }
            activeSlice++;
            pointGroups[activeTime][activeSlice].SetActive(true);
        }
        else
        {
            pointGroups[activeTime][activeSlice].SetActive(false);
            activeSlice++;
            if (activeSlice == nbDepth)
            {
                for (int d = 0; d < nbDepth; d++)
                {
                    pointGroups[activeTime][d].SetActive(true);
                }
                activeSlice = -1;
            }
            else
            {
                pointGroups[activeTime][activeSlice].SetActive(true);
            }
        }
    }

    public void ActivateSliceInverse()
    {
        if (activeSlice == -1)
        {
            for (int d = 0; d < nbDepth; d++)
            {
                pointGroups[activeTime][d].SetActive(false);
            }
            activeSlice = nbDepth - 1;
        }
        else
        {
            pointGroups[activeTime][activeSlice].SetActive(false);
            activeSlice--;
            if (activeSlice == -1)
            {
                for (int d = 0; d < nbDepth; d++)
                {
                    pointGroups[activeTime][d].SetActive(true);
                }
            }
            else
            {
                pointGroups[activeTime][activeSlice].SetActive(true);
            }
        }
    }

    public void HideOrShow(bool newValue)
    {
        if (!newValue)
        {
            for (int d = 0; d < nbDepth; d++)
            {
                for (int t = 0; t < nbTime; t++)
                {
                    pointGroups[t][d].SetActive(false);
                }
            }
        }
        else
        {
            if (activeSlice == -1)
            {
                for (int d = 0; d < nbDepth; d++)
                {
                    pointGroups[activeTime][d].SetActive(true);
                }
            }
            else
            {
                pointGroups[activeTime][activeSlice].SetActive(true);
            }
        }
    }



    public void ResetSlice()
    {
        activeSlice = -1;
        for (int d = 0; d < nbDepth; d++)
        {
            pointGroups[activeTime][d].SetActive(true);
        }
    }

    public virtual String GetProperty(int triangleIndex)
    {
        int slice = activeSlice;
        if (activeSlice == -1)
        {
            slice = 0;
        }
        int indiceColor = (triangleIndex / 12) * 8;
        Color colorProperty = pointGroups[activeTime][slice].GetComponent<MeshFilter>().mesh.colors[indiceColor];
        for (int i = 0; i < 8; i++)
        {
            print("color [" + (indiceColor + i) + "] = " + pointGroups[activeTime][slice].GetComponent<MeshFilter>().mesh.colors[indiceColor + i]);
        }
        return (colorProperty.ToString());
    }

    public void ActivateTime()
    {
        for (int i = 0; i < nbDepth; i++)
        {
            pointGroups[activeTime][i].SetActive(false);
        }
        activeTime++;
        if (activeTime == nbTime)
        {
            activeTime = 0;
        }
        if (activeSlice == -1)
        {
            for (int i = 0; i < nbDepth; i++)
            {
                pointGroups[activeTime][i].SetActive(true);
            }
        }
        else
        {
            pointGroups[activeTime][activeSlice].SetActive(true);
        }
    }
    

    public void UpdateCollider(MeshCollider collider, Mesh mesh)
    {
        if (collider != null)
        {
            collider.sharedMesh = mesh;
            ReActivateSlice();
        }
        else
        {
            print("collider null");
        }
    }

    public void UpdateCollider(MeshCollider collider, Vector3[] points)
    {
        if (collider != null)
        {
            collider.sharedMesh.vertices = points;
            ReActivateSlice();
        }
        else
        {
            print("collider null");
        }
    }

    public void ReActivateSlice()
    {
        if (activeSlice == -1)
        {
            for (int d = 0; d < nbDepth; d++)
            {
                pointGroups[activeTime][d].SetActive(true);
            }
        }
        else
        {
            pointGroups[activeTime][activeSlice].SetActive(true);
        }
    }

    public Shader ChooseShader()
    {
        return Shader.Find("Unlit/TriangleShader");
    }

    /// <summary>
    /// ///////////////////ADDED THE TWO FUNCTIONS
    /// </summary>
    /// <returns></returns>

    public bool GetHiddenStatus()
    {
        return hidden;
    }

    public void SetHiddenStatus(bool newVal)
    {
        hidden = newVal;
    }
}
