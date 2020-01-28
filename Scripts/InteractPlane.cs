using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractPlane : MonoBehaviour {
    // Start is called before the first frame update
    private float speed = 5.0f;
    bool leftIsDown ;
    bool rightIsDown ;
    bool middleIsDown ;
    Vector3 previousMousePosition ;
    GameObject[] obj;

    void Start () {
    }


    // Update is called once per frame
    void Update () {
        var mousePosition = Input.mousePosition ;

        var delta = mousePosition - previousMousePosition ;
        
        if (Input.GetKey(KeyCode.B)) {
            transform.Rotate(new Vector3(delta.y, -delta.x, 0.0f) * Time.deltaTime * speed / 2);
        }

        if (Input.GetKey(KeyCode.N)) {
            transform.Translate(new Vector3(0.0f, delta.y, 0.0f) * Time.deltaTime * speed);     
        }

        if(Input.GetKey(KeyCode.I))
        {
            transform.position = new Vector3(0, 0, 0);
            transform.rotation = new Quaternion();
        }


    previousMousePosition = mousePosition ;
    }

}