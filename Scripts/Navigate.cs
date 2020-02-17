using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigate : MonoBehaviour {
    // GameObject begin ;

    // Start is called before the first frame update
    void Start () {
        // begin = GameObject.CreatePrimitive (PrimitiveType.Sphere) ;
        // begin.transform.parent = Camera.main.transform ;
        // begin.transform.localScale = new Vector3 (0.01f, 0.01f, 0.01f) ;
    }


    // Update is called once per frame
    void Update () {
        float rotateHorizontal = Input.GetAxis ("Horizontal") ;
		float moveFrontBack = Input.GetAxis ("Vertical") ;
        transform.Translate (0.0f, 0.0f, moveFrontBack) ;
        transform.Rotate (0.0f, rotateHorizontal, 0.0f) ;
		var moveRight = Input.GetKey (KeyCode.Y) ;//Y. 
        if (moveRight) {
            transform.Translate (20f, 0.0f, 0.0f) ;
        }
		var moveLeft = Input.GetKey (KeyCode.R) ;//T 
        if (moveLeft) {
            transform.Translate (-20f, 0.0f, 0.0f) ;
        }
		var moveUp = Input.GetKey (KeyCode.PageUp) ;
        if (moveUp) {
            transform.Translate (0.0f, 20f, 0.0f) ;
        }
		var moveDown = Input.GetKey (KeyCode.PageDown) ;
        if (moveDown) {
            transform.Translate (0.0f, -20f, 0.0f) ;
        }
		var rotateHeadingRight = Input.GetKey (KeyCode.H) ;
        if (rotateHeadingRight) {
            transform.Rotate (0.0f, 1f, 0.0f) ;
        }
		var rotateHeadingLeft = Input.GetKey (KeyCode.F) ;//G
        if (rotateHeadingLeft) {
            transform.Rotate (0.0f, -1f, 0.0f) ;
        }
		var rotatePitchDown = Input.GetKey (KeyCode.G) ;//O
        if (rotatePitchDown) {
            transform.Rotate (1f, 0.0f, 0.0f) ;
        }
		var rotatePitchUp = Input.GetKey (KeyCode.T) ;//P
        if (rotatePitchUp) {
            transform.Rotate (-1f, 0.0f, 0.0f) ;
        }
		var rotateRollLeft = Input.GetKey (KeyCode.O) ;//E
        if (rotateRollLeft) {
            transform.Rotate (0.0f, 0.0f, 0.5f) ;
        }
		var rotateRollRight = Input.GetKey (KeyCode.P) ;//R
        if (rotateRollRight) {
            transform.Rotate (0.0f, 0.0f, -0.5f) ;
        }

        var mousePosition = Input.mousePosition ;

        Vector3 point = new Vector3 () ;
        point = Camera.main.ScreenToWorldPoint (new Vector3 (mousePosition.x, mousePosition.y, Camera.main.nearClipPlane)) ;
        // begin.transform.position = point ;
		var informationRequired = Input.GetKeyDown (KeyCode.F1) ;
        if (informationRequired) {
            print ("informationRequired at " + point) ;
            RaycastHit hit ;
            if (Physics.Raycast (transform.position, point - transform.position, out hit, Mathf.Infinity)) {
                Debug.Log ("Did Hit") ;
                Debug.Log ("Target Position: " + hit.collider.gameObject.transform.position);
                Debug.Log ("Hit point : " + hit.point) ;
                PointMap pointMap = hit.collider.gameObject.GetComponentInParent<PointMap> () ;
                if (pointMap != null) {
                    SphereCollider sc = pointMap.GetComponent<SphereCollider> () ;
                    sc.enabled = false ;
                    if (Physics.Raycast (transform.position, point - transform.position, out hit, Mathf.Infinity)) {
                        Debug.Log ("Did Hit again") ;
                        Debug.Log ("Target Position: " + hit.collider.gameObject.transform.position);
                        Debug.Log ("Hit point : " + hit.point) ;
                        Debug.Log ("Hit triangle index : " + hit.triangleIndex) ;
                        pointMap = hit.collider.gameObject.GetComponentInParent<PointMap> () ;
                        if (pointMap != null) {
                            print (pointMap.GetProperty (hit.triangleIndex)) ;
                        } else {
                            Debug.Log ("Did not Hit this time") ;
                        }
                    } else {
                        Debug.Log ("But not on a PointMap") ;
                    }
                    sc.enabled = true ;
                }
            } else {
                Debug.Log ("Did not Hit") ;
            }
        }

    }

}
