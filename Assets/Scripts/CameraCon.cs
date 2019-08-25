using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCon : MonoBehaviour
{
    public GameObject level;
    public GameObject fColliders;
    public float rotSpeed;
    public float scalSpeed;
    bool isRotating = false;
    bool isScaling = false;
    bool isInverted = false;
    Camera main;
    Platform[] platforms;
    float sc;
    private void Start() {
        main = GetComponent<Camera>();
        platforms = level.GetComponentsInChildren<Platform>();
    }
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0)){

            RaycastHit hit;
            Ray ray = main.ScreenPointToRay(main.transform.position);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "xyz") isInverted = false;
                else isInverted = true;
            }
            else 
                isInverted = false;
            isRotating = true;
        }
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
            Vector3 r;
            Transform t = level.transform;
            Quaternion rot_x, rot_y;
            float rot;
            if (isInverted){
                rot = Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;
                rot_x = Quaternion.Euler(0, -rot, 0);
                
                r = t.rotation.eulerAngles;
                rot = Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime;
                if (r.y > 0 && r.y < 180)
                    rot_y = Quaternion.Euler(0,0,rot);
                    // t.Rotate(0, 0, Input.GetAxis("Mouse Y"), Space.Self);
                else
                    rot_y = Quaternion.Euler(0,0,-rot);
                    // t.Rotate(0, 0, -Input.GetAxis("Mouse Y"), Space.Self);
                // t.Rotate(0, -Input.GetAxis("Mouse X"), 0, Space.World);
                // fColliders.transform.Rotate(0, -rot, 0, Space.World);
                fColliders.transform.rotation *= rot_x;

                t.rotation = t.rotation * rot_y;
                t.rotation = rot_x * t.rotation;
                r = t.rotation.eulerAngles;
                r.x = Mathf.Clamp(r.x, 0, 1); // Fix the third axis rotation.

                t.rotation = Quaternion.Euler(r);

                // rot_y = Quaternion.Euler(0, 0, -rot);
                // foreach (Platform p in platforms)
                // {
                //     p.transform.rotation *= rot_y;
                //     // p.transform.rotation = rot_x * p.transform.rotation;
                // }
            }
            else{
                rot = Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;
                rot_x = Quaternion.Euler(0, -rot, 0);
                
                r = t.rotation.eulerAngles;
                rot = Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime;
                if (r.y > 90 && r.y < 270)
                    rot_y = Quaternion.Euler(-rot, 0, 0);
                    // t.Rotate(-Input.GetAxis("Mouse Y"), 0, 0, Space.Self);
                else
                    rot_y = Quaternion.Euler(rot, 0, 0);
                    // t.Rotate(Input.GetAxis("Mouse Y"), 0, 0, Space.Self);
                // t.Rotate(0, -Input.GetAxis("Mouse X"), 0, Space.World);
                // fColliders.transform.Rotate(0, -Input.GetAxis("Mouse X"), 0, Space.World);
                fColliders.transform.rotation *= rot_x;

                t.rotation *= rot_y;
                t.rotation = rot_x * t.rotation;
                r = t.rotation.eulerAngles;

                r.z = Mathf.Clamp(r.z, 0, 1); // Fix the third axis rotation.

                t.rotation = Quaternion.Euler(r);


                // rot_y = Quaternion.Euler(-rot, 0, 0);
                // foreach (Platform p in platforms)
                // {
                //     p.transform.rotation *= rot_y;
                //     // p.transform.rotation = rot_x * p.transform.rotation;
                // }
            }
            // Debug.Log(platforms[0].transform.rotation);
            foreach(Platform p in platforms)
                p.transform.rotation = p.initRotation;
            // rot_y.eulerAngles = -rot_y.eulerAngles;
            // rot_x.eulerAngles = -rot_x.eulerAngles;
    
        }
        else if (isScaling){
            float scaling = Input.GetAxis("Mouse X") * scalSpeed * Time.deltaTime;
            Vector3 s = new Vector3();
            foreach(Platform p in platforms){
                s = p.pTransform.localPosition;
                s += s*scaling;
                s.x = Mathf.Clamp(s.x, p.minPos.x, p.maxPos.x);
                s.y = Mathf.Clamp(s.y, p.minPos.y, p.maxPos.y);
                s.z = Mathf.Clamp(s.z, p.minPos.z, p.maxPos.z);
                p.pTransform.localPosition = s;
                
            }
        }
    }
}