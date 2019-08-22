using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCon : MonoBehaviour
{
    public GameObject level;
    public float rotSpeed;
    public float scalSpeed;
    bool isRotating = false;
    bool isScaling = false;
    Camera main;
    Platform[] platforms;
    float sc;
    private void Start() {
        main = GetComponent<Camera>();
        platforms = level.GetComponentsInChildren<Platform>();
    }
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
            isRotating = true;
        else if (Input.GetMouseButtonUp(0))
            isRotating = false;
        
        else if (Input.GetMouseButtonDown(1))
            isScaling = true;
        else if (Input.GetMouseButtonUp(1))
            isScaling = false;

    }

    void FixedUpdate()
    {
        if (isRotating)
        {
            level.transform.Rotate(Input.GetAxis("Mouse Y"), 0, 0, Space.Self);
            level.transform.Rotate(0, -Input.GetAxis("Mouse X"), 0, Space.World);

            // Quaternion rot = Quaternion.Euler(Input.GetAxis("Mouse Y")*rotSpeed*Vector3.right*Time.deltaTime);
            // Quaternion rot2 = Quaternion.Euler(Input.GetAxis("Mouse X")*rotSpeed*Vector3.down*Time.deltaTime);
            
            // level.transform.rotation = level.transform.rotation * rot;  // Local 
            // level.transform.rotation = rot2 * level.transform.rotation;    //World

            Vector3 r = level.transform.rotation.eulerAngles;
            if (r.x > 80 && r.x < 180)
                r.x = Mathf.Clamp(r.x, 0, 80);
            else if (r.x < 275 && r.x > 180)
                r.x = Mathf.Clamp(r.x, 275, 360);

            if (r.y > 90 && r.y < 180)
                r.y = Mathf.Clamp(r.y, 0, 90);
            else if (r.y < 270 && r.y > 180)
                r.y = Mathf.Clamp(r.y, 270, 360);

            r.z=0; // Resets z-axis rotation.

            level.transform.rotation = Quaternion.Euler(r);
        }
        else if (isScaling){
            float scaling = Input.GetAxis("Mouse X") * scalSpeed * Time.deltaTime;
            // sc += scaling;
            // if (sc < 0.4f || sc > 1.6f)
            //     scaling = 0;
            Vector3 s = new Vector3();
            foreach(Platform t in platforms){
                // t.pTransform.position += t.pTransform.position*scaling;
                // t.pTransform.localScale += t.pTransform.localScale * scaling * 0.1f;
                s = t.pTransform.localPosition;
                s += s*scaling;
                s.x = Mathf.Clamp(s.x, t.minPos.x, t.maxPos.x);
                s.y = Mathf.Clamp(s.y, t.minPos.y, t.maxPos.y);
                s.z = Mathf.Clamp(s.z, t.minPos.z, t.maxPos.z);
                t.pTransform.localPosition = s;
                
            }
        }
    }
}