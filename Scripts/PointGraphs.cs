using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointGraphs : MonoBehaviour
{
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


    public void Start()
    {
        hidden = false;
        HideOrShow(true);
    }

    public void Update()
    {

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
        print("Slice Changed : " + activeSlice + " at time " + activeTime);
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
        print("Slice Changed : " + activeSlice + " at time " + activeTime);
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
        print("Slice Changed : " + activeSlice + " at time " + activeTime);
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
        print("colorProperty = " + colorProperty);
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
        print("Time Changed : " + activeTime + " for slice " + activeSlice);
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
