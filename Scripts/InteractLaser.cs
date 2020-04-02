using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractLaser : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {

            var mousePosition = Input.mousePosition;
            

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out hit, 5000f))
            {
                if (hit.collider.tag == "laser")
                {
                    GameObject laser = hit.collider.gameObject.transform.parent.gameObject;
                    Camera.main.transform.position = (laser.GetComponent<LineRenderer>().GetPosition(1) - laser.GetComponent<LineRenderer>().GetPosition(0)) * 3f;
                    Camera.main.transform.LookAt(laser.GetComponent<LineRenderer>().GetPosition(0));
                }
            }
        }
    }
}
