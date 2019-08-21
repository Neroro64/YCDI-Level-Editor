using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCon : MonoBehaviour
{
    public GameObject level;
    public float rotSpeed;
    bool isRotating = false;
    Vector3 origin;
    Camera main;
    float yRot = 0;
    float xRot = 0;
    private void Start() {
        main = GetComponent<Camera>();
    }
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0)){
            isRotating = true;
            // origin = Input.mousePosition;
        }

        else if (Input.GetMouseButtonUp(0))
            isRotating = false;

    }

    void FixedUpdate()
    {
        Vector3 r = new Vector3();
        if (isRotating)
        {
            // level.transform.Rotate(Input.GetAxis("Mouse Y"), 0, 0, Space.Self);
            // level.transform.Rotate(0, -Input.GetAxis("Mouse X"), 0, Space.World);
            // xRot = Mathf.Clamp(xRot - Input.GetAxis("Mouse Y")*rotSpeed*Time.deltaTime, -90f, 90f);
            // level.transform.eulerAngles = new Vector3(xRot, level.transform.eulerAngles.y, level.transform.eulerAngles.z);
            Quaternion rot = Quaternion.Euler(Input.GetAxis("Mouse Y")*rotSpeed*Vector3.right*Time.deltaTime);
            Quaternion rot2 = Quaternion.Euler(Input.GetAxis("Mouse X")*rotSpeed*Vector3.down*Time.deltaTime);
            
            level.transform.rotation = level.transform.rotation * rot;  // Local 
            level.transform.rotation = rot2 * level.transform.rotation;    //World

            r = level.transform.rotation.eulerAngles;

            Debug.Log(r);
            if (r.x > 90f && r.x < 180f)
            {
                o = true;
                r.x = 70f;
                // if (rot2.eulerAngles.x > 0)
                //     rot2 = Quaternion.identity;
            }
            else if (r.x < 270f && r.x > 180f)
            {
              
                o = true;
                r.x = 290f;
                // if (rot2.eulerAngles.x < 360)
                //     rot2 = Quaternion.identity;
            }
            else 
            o = false;
            if (r.z >= 180f)
                r.z = 0;
            
            if (r.y > 90 && r.y < 180){
                o = true;
                r.y = 70;
            }
            else if (r.y < 270 && r.y > 180){
                o = true;
                r.y = 290;
            }
            else 
            o = false;
            
            // else if (r.x > 270f && r.x < 360f)
            //     r.x = 270f;
            // if (r.x > 90f || r.x < -90f)
            //     Debug.Log("YO WTF");
        }
    }
}