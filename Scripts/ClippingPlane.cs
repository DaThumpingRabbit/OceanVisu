using System.Collections ;
using System.Collections.Generic ;
using UnityEngine ;

// code inspired from https://www.ronja-tutorials.com/2018/08/06/plane-clipping.html
public class ClippingPlane : MonoBehaviour {
    //Passer un pointmap avec fonction
    protected PointMap maps ;
    private InteractPlane script;
    private GameObject clip;
    protected bool clippingDisabled;

    public Plane plane ;

    void Start () {
        plane = new Plane (new Vector3 (0, 1, 0), new Vector3 (0, 0, 0)) ;
        clippingDisabled = false;        
    }

    //execute every frame
    void Update () {
        clippingDisabled = maps.GetClippingDisabled();

        if(!clippingDisabled)
        {
            Matrix4x4 cameraInverseMatrix = Camera.main.transform.worldToLocalMatrix;
            Vector3 position = cameraInverseMatrix.MultiplyPoint3x4(transform.parent.position);
            transform.position = position;

            Matrix4x4 matrix = transform.parent.localToWorldMatrix;
            Matrix4x4 expectedMatrix = new Matrix4x4();
            expectedMatrix = cameraInverseMatrix * matrix;
            transform.rotation = expectedMatrix.rotation;
            // create plane
            plane.SetNormalAndPosition(transform.up, transform.position);
            //transfer values from plane to vector4 avec inversion de la normale en z car repère caméra ingversé
            Vector4 planeRepresentation = new Vector4(plane.normal.x, plane.normal.y, -plane.normal.z, plane.distance);
            // pass vector to shader
            maps.SetClippingPlane(planeRepresentation);
        }
        
    }

    /// <summary>
    /// ////////CHANGED THIS FUNC
    /// </summary>
    /// <param name="value"></param>
    
    public void SetClippingDisabledStatus(bool value)
    {
        clippingDisabled = value;
    }

    public void SetPointMap(PointMap pM)
    {
        maps = pM;
    }

    public void Translate(Vector3 vec)
    {
        plane.Translate(vec);
        //clip.transform.Translate(vec);
    }

}
