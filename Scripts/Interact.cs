using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour {
    // Start is called before the first frame update
    private float speed = 5.0f;
    bool leftIsDown ;
    bool rightIsDown ;
    bool middleIsDown ;
    Vector3 previousMousePosition ;
    protected bool independent = true;

    void Start ()
    {

    }


    // Update is called once per frame
    void Update () {
        var leftUp = Input.GetMouseButtonUp (0) ;
        if (leftUp) {
            //print ("left button up") ;
            leftIsDown = false ;
        }
        var middleUp = Input.GetMouseButtonUp (2) ;
        if (middleUp) {
            //print ("middle button up") ;
            middleIsDown = false ;
        }
        var rightUp = Input.GetMouseButtonUp (1) ;
        if (rightUp) {
            //print ("right button up") ;
            rightIsDown = false ;
        }
        var mousePosition = Input.mousePosition ;

        var delta = mousePosition - previousMousePosition ;

        GameObject[] obj = GameObject.FindGameObjectsWithTag("planet");
        GameObject[] clipP = GameObject.FindGameObjectsWithTag("ClippingPlane");

        if (rightIsDown) {
            for (int i = 0; i < obj.Length; i++)
            {
                obj[i].transform.Rotate(new Vector3(delta.y, -delta.x, 0.0f) * Time.deltaTime * speed / 2,Space.World);
            }
            
            /*
            Matrix4x4 cameraMatrix = camera.transform.localToWorldMatrix ;
            Matrix4x4 cameraInverseMatrix = camera.transform.worldToLocalMatrix ;
            Matrix4x4 matrix = transform.localToWorldMatrix ;
            Matrix4x4 localRotationMatrix = new Matrix4x4 () ;
            Matrix4x4 expectedRotationMatrix = new Matrix4x4 () ;
            Quaternion localRotation = new Quaternion () ;
            localRotation = Quaternion.Euler (delta.y, -delta.x, 0.0f) ;
            localRotationMatrix.SetTRS (new Vector3 (), localRotation, new Vector3 (1.0f, 1.0f, 1.0f))  ;
            expectedRotationMatrix = cameraMatrix * localRotationMatrix * cameraInverseMatrix * matrix ;
            transform.rotation = expectedRotationMatrix.rotation ;
            */
        }
        if (leftIsDown) {
            for (int i=0; i < obj.Length; i++)
            {
                obj[i].transform.Translate(new Vector3(delta.x*2, delta.y*2, 0.0f) * Time.deltaTime * speed,Space.World);
            }
            
            /*
            Matrix4x4 cameraMatrix = camera.transform.localToWorldMatrix ;
            Matrix4x4 cameraInverseMatrix = camera.transform.worldToLocalMatrix ;
            Vector3 localPositionInCameraFrame = cameraInverseMatrix.MultiplyPoint3x4 (transform.position) ;
            Vector3 expectedLocalPosition = localPositionInCameraFrame + new Vector3 (delta.x, delta.y, 0.0f) ;
            Vector3 expectedNewPosition = cameraMatrix.MultiplyPoint3x4 (expectedLocalPosition) ;
            transform.position = expectedNewPosition ;
            */
        }
        if (middleIsDown) {
            if(!independent)
            {
                for (int i = 0; i < obj.Length; i++)
                {
                    obj[i].transform.Translate(new Vector3(0.0f, 0.0f, delta.y) * Time.deltaTime * speed,Space.World);
                    clipP[i].transform.Translate(new Vector3(0.0f, 0.0f, delta.y) * Time.deltaTime * speed, Space.World);
                }
            }
            else
            {
                transform.Translate(new Vector3(0.0f, 0.0f, delta.y) * Time.deltaTime * speed, Space.World);
            }
            

            /*
            Matrix4x4 cameraMatrix = camera.transform.localToWorldMatrix ;
            Matrix4x4 cameraInverseMatrix = camera.transform.worldToLocalMatrix ;
            Vector3 localPositionInCameraFrame = cameraInverseMatrix.MultiplyPoint3x4 (transform.position) ;
            Vector3 expectedLocalPosition = localPositionInCameraFrame + new Vector3 (0.0f, 0.0f, delta.y) ;
            Vector3 expectedNewPosition = cameraMatrix.MultiplyPoint3x4 (expectedLocalPosition) ;
            transform.position = expectedNewPosition ;
            */
        }
        previousMousePosition = mousePosition ;
    }

    void OnMouseOver () {
        var leftDown = Input.GetMouseButtonDown (0) ;
        if (leftDown) {
            //print ("left button down") ;
            leftIsDown = true ;
        }
        var middleDown = Input.GetMouseButtonDown (2) ;
        if (middleDown) {
            //print ("middle button down") ;
            middleIsDown = true ;
        }
        var rightDown = Input.GetMouseButtonDown (1) ;
        if (rightDown) {
            //print ("right button down") ;
            rightIsDown = true ;
        }
    }

    public void SetIndependentMovement(bool newValue)
    {
        Debug.Log(independent);
        independent = newValue;
    }

}
