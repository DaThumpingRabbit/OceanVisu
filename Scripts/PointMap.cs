using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointMap : MonoBehaviour {
    protected int nbTime = 0 ;
    protected int nbDepth = 0 ;
    protected int nbX = 0 ;
    protected int nbY = 0 ;
    protected int activeSlice = -1 ;
    protected int activeTime = 0 ;
    protected Vector3 [][] spherePoints ;
    protected Vector3 [][] planePoints ;
    protected Shader shader ;
    protected bool clippingDisabled = false; 
    /// <summary>
    /// /////////////////ADDED THIS BOOL
    /// </summary>
    protected bool hidden;

    public GameObject [][] pointGroups ;

    protected GameObject innerSphere ;

    public void Start () {
        innerSphere = GameObject.CreatePrimitive (PrimitiveType.Sphere) ;
        innerSphere.transform.parent = transform ;
        innerSphere.transform.localScale = new Vector3 (300, 300, 300) ;        
        innerSphere.transform.localPosition = new Vector3 (0, 0, 0) ;
        innerSphere.SetActive(true);
        innerSphere.GetComponent<MeshRenderer>().material = Resources.Load("InnerSphere", typeof(Material)) as Material;
        hidden = true;
        Hide(true);
    }

    public void Update () {
        
    }

    public void SetShader (Shader shader) {
        this.shader = shader ;
    }

    public void initPointGroups (int nbTime, int nbDepth, int nbY, int nbX) {
        this.nbTime = nbTime ;
        activeTime = nbTime - 1 ;
        this.nbDepth = nbDepth ;
        this.nbY = nbY ;
        this.nbX = nbX ;
        pointGroups = new GameObject [nbTime][] ;
        spherePoints = new Vector3 [nbDepth][] ;
        planePoints = new Vector3 [nbDepth][] ;
         for (int t = 0 ; t < nbTime ; t++) {
            pointGroups [t] = new GameObject [nbDepth] ;
       }
    }

    public virtual void initSlice (int t, int depth, Vector3 [] sPoints, Vector3 [] pPoints, int [] indices, Color [] colors, MeshTopology meshTopology) {
        pointGroups [t][depth] = new GameObject () ;
        pointGroups [t][depth].AddComponent<MeshFilter> () ;
        pointGroups [t][depth].AddComponent<MeshRenderer> () ;
        spherePoints [depth] = sPoints ;
        planePoints [depth] = pPoints ;
        Mesh mesh = new Mesh () ;
        mesh.vertices = sPoints ;
        mesh.colors = colors ;
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32 ;
        mesh.SetIndices (indices, meshTopology, 0) ;
        pointGroups [t][depth].GetComponent<MeshFilter> ().mesh = mesh ;
        pointGroups [t][depth].GetComponent<MeshRenderer> ().motionVectorGenerationMode = MotionVectorGenerationMode.Object ;
        Renderer rend = pointGroups [t][depth].GetComponent<Renderer> ();
        rend.material.shader = shader ;
        pointGroups [t][depth].transform.parent = transform ;
        pointGroups [t][depth].SetActive (false) ;
    }

    public void ActivateSlice () {
        if (activeSlice == -1) {
            for (int d = 0 ; d < nbDepth ; d++) {
                pointGroups [activeTime][d].SetActive (false) ;
            }             
            activeSlice ++ ;
            pointGroups [activeTime][activeSlice].SetActive (true) ;
        } else {
            pointGroups [activeTime][activeSlice].SetActive (false) ;
            activeSlice ++ ;
            if (activeSlice == nbDepth) {
                for (int d = 0 ; d < nbDepth ; d++) {
                    pointGroups [activeTime][d].SetActive (true) ;
                }
                activeSlice = -1 ;
            } else {
                pointGroups [activeTime][activeSlice].SetActive (true) ;
            }
        }
        print ("Slice Changed : " + activeSlice + " at time " + activeTime) ;
    }

    public void ActivateSliceInverse () {
        if (activeSlice == -1) {
            for (int d = 0 ; d < nbDepth ; d++) {
                pointGroups [activeTime][d].SetActive (false) ;
            }             
            activeSlice = nbDepth-1 ;
        } else {
            pointGroups [activeTime][activeSlice].SetActive (false) ;
            activeSlice -- ;
            if (activeSlice == -1) {
                for (int d = 0 ; d < nbDepth ; d++) {
                    pointGroups [activeTime][d].SetActive (true) ;
                }
            } else {
                pointGroups [activeTime][activeSlice].SetActive (true) ;
            }
        }
        print ("Slice Changed : " + activeSlice + " at time " + activeTime) ;
    }

    public void Hide(bool newValue)
    {
        if (newValue)
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



    public void ResetSlice () {
        activeSlice = -1 ;
        for (int d = 0 ; d < nbDepth ; d++) {
            pointGroups [activeTime][d].SetActive (true) ;
        }
        print ("Slice Changed : " + activeSlice + " at time " + activeTime) ;
    }

    public virtual String GetProperty (int triangleIndex) {
        int slice = activeSlice ;
        if (activeSlice == -1) {
            slice = 0 ;
        }
        int y = triangleIndex / (2 * (nbX -1)) ;
        int x = (triangleIndex - y * (2 * (nbX -1))) / 2 ;
        int reste = triangleIndex - y * (2 * (nbX -1)) - 2 * x ;
        print ("x = " + x + " ; y = " + y + " ; z = " + reste) ;
        Color colorProperty = new Color () ;
        Color colorProperty1 ;
        Color colorProperty2 ;
        Color colorProperty3 ;
        if (reste == 0) {
            colorProperty1 = pointGroups [activeTime][slice].GetComponent<MeshFilter>().mesh.colors [(y - 1) * nbX + x - 1] ;
            colorProperty2 = pointGroups [activeTime][slice].GetComponent<MeshFilter>().mesh.colors [y * nbX + x - 1] ;
            colorProperty3 = pointGroups [activeTime][slice].GetComponent<MeshFilter>().mesh.colors [y * nbX + x] ;
        } else {
            colorProperty1 = pointGroups [activeTime][slice].GetComponent<MeshFilter>().mesh.colors [(y - 1) * nbX + x - 1] ;
            colorProperty2 = pointGroups [activeTime][slice].GetComponent<MeshFilter>().mesh.colors [y * nbX + x] ;
            colorProperty3 = pointGroups [activeTime][slice].GetComponent<MeshFilter>().mesh.colors [(y - 1) * nbX + x] ;
            colorProperty = (colorProperty1 + colorProperty2 + colorProperty3) / 3 ;
        }
        colorProperty = (colorProperty1 + colorProperty2 + colorProperty3) / 3 ;
        print ("colorProperty1 = " + colorProperty1 + " ; colorProperty2 = " + colorProperty2 + " ; colorProperty3 = " + colorProperty3) ;
        return (colorProperty.ToString ()) ;
    }

    public void ActivateTime () {
        for (int i = 0 ; i < nbDepth ; i++) {
            pointGroups [activeTime][i].SetActive (false) ;
        }
        activeTime ++ ;
        if (activeTime == nbTime) {
            activeTime = 0 ;
        }
        if (activeSlice == -1) {
            for (int i = 0 ; i < nbDepth ; i++) {
               pointGroups [activeTime][i].SetActive (true) ;
            }
        } else {
            pointGroups [activeTime][activeSlice].SetActive (true) ;
        }
        print ("Time Changed : " + activeTime + " for slice " + activeSlice) ;
    }
    public void ChangeToSphere () {
        for (int t = 0 ; t < nbTime ; t++) {
            for (int d = 0 ; d < nbDepth ; d++) {
                pointGroups [t][d].GetComponent<MeshFilter>().mesh.vertices = spherePoints [d] ;
                //UpdateCollider (pointGroups [t][d].GetComponent<MeshCollider> (), spherePoints [d]) ;
                //UpdateCollider (pointGroups [t][d].GetComponent<MeshCollider> (), pointGroups [t][d].GetComponent<MeshFilter>().mesh) ;
            }
        }
        innerSphere.SetActive (false) ;
    }

    public void ChangeToPlane () {
        for (int t = 0 ; t < nbTime ; t++) {
            for (int d = 0 ; d < nbDepth ; d++) {
                pointGroups [t][d].GetComponent<MeshFilter>().mesh.vertices = planePoints [d] ;
                //UpdateCollider (pointGroups [t][d].GetComponent<MeshCollider> (), planePoints [d]) ;
                //UpdateCollider (pointGroups [t][d].GetComponent<MeshCollider> (), pointGroups [t][d].GetComponent<MeshFilter>().mesh) ;
            }
        }
        innerSphere.SetActive (false) ;
    }

    public void UpdateCollider (MeshCollider collider, Mesh mesh) {
        if (collider != null) {
            collider.sharedMesh = mesh ;
            ReActivateSlice () ;
        } else {
            print ("collider null") ;
        }
    }

    public void UpdateCollider (MeshCollider collider, Vector3 [] points) {
        if (collider != null) {
            collider.sharedMesh.vertices = points ;
            ReActivateSlice () ;
        } else {
            print ("collider null") ;
        }
    }

    public void ReActivateSlice () {
        if (activeSlice == -1) {
            for (int d = 0 ; d < nbDepth ; d++) {
                pointGroups [activeTime][d].SetActive (true) ;
            }             
        } else {
            pointGroups [activeTime][activeSlice].SetActive (true) ;
        }
    }


    public void SetClippingPlane(Vector4 vector)
    {
        for (int i = 0; i < nbDepth; i++)
        {
            pointGroups[activeTime][i].GetComponent<Renderer>().material.SetVector("_Plane", vector);
        }
    }

    public void EnableClippingPlane(bool value)
    {
        if (!value)
        {
            for (int i = 0; i < nbDepth; i++)
            {
                pointGroups[activeTime][i].GetComponent<Renderer>().material.shader = Shader.Find("Unlit/TriangleShader");
            }
            clippingDisabled = true;
        }
        else
        {
            for (int i = 0; i < nbDepth; i++)
            {
                pointGroups[activeTime][i].GetComponent<Renderer>().material.shader = Shader.Find("Unlit/TriangleShaderForClipping");
            }
            clippingDisabled = false;
        }
    }

    public bool GetClippingDisabled()
    {
        return clippingDisabled;
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
