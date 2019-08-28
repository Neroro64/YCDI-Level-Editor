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
    bool finalizeRotation = false;
    Quaternion startRotation;
    Quaternion finalRotation;
    float step;
    Camera main;
    Platform[] platforms;
    private float currentZoom = 1f;
    private float currentLevel = 1f;
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
        else if (Input.GetMouseButtonUp(0)){
            isRotating = false;
            finalizeRotation = true;
            Vector3 rot = level.transform.rotation.eulerAngles;
            rot.x = getClosest(rot.x);
            rot.y = getClosest(rot.y);
            rot.z = getClosest(rot.z);
            startRotation = level.transform.rotation;
            finalRotation = Quaternion.Euler(rot);
            step = 0;
        }
        
        else if (Input.GetMouseButtonDown(1))
            isScaling = true;
        else if (Input.GetMouseButtonUp(1))
            isScaling = false;

    }

    void FixedUpdate()
    {
        if (isRotating)
        {
            float inputX = Input.GetAxis("Mouse X");
            float inputY = Input.GetAxis("Mouse Y");
            Quaternion rot;
            float dir;
            if (Mathf.Abs(inputX) > Mathf.Abs(inputY)){
                dir = inputX * rotSpeed * Time.deltaTime;
                rot = Quaternion.Euler(-dir*Vector3.up);
            }
            else{
                dir = inputY * rotSpeed * Time.deltaTime;
                rot = Quaternion.Euler(dir*Vector3.right);
            }

            level.transform.Rotate(rot.eulerAngles, Space.World);
            fColliders.transform.LookAt(level.transform.forward);
            fColliders.transform.rotation = Quaternion.Euler(0, fColliders.transform.rotation.eulerAngles.y, 0);
            
            foreach (Platform p in platforms)
            {
                p.transform.rotation = p.initRotation;
            }
    
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
        else if (finalizeRotation){
            endRotating();
            foreach (Platform p in platforms)
            {
                p.transform.rotation = p.initRotation;
            }
        }
        

    }

    private void endRotating(){ // 0, 90, 180, 270
        level.transform.rotation = Quaternion.Slerp(startRotation, finalRotation, step);
        step += Time.deltaTime*2;
        if (step >= 1)
            finalizeRotation = false;
    }

    private float getClosest(float v){
        float min_diff = v;
        float diff;
        float id = 0;
        
        diff = Mathf.Abs(90-v);
        if (diff < min_diff){
            id = 90f;
            min_diff = diff;
        }

        diff = Mathf.Abs(180-v);
        if (diff < min_diff)
        {
            id = 180f;
            min_diff = diff;
        }

        diff = Mathf.Abs(270 - v);
        if (diff < min_diff)
        {
            id =  270f;
            min_diff = diff;
        }

        diff = Mathf.Abs(360 - v);
        if (diff < min_diff)
        {
            id = 0f;
            min_diff = diff;
        }
        return id;
    }

    public void zoom(float v){
        transform.Translate(0, 0, (v - currentZoom)*5);
        currentZoom = v;
        isRotating = false;
    }
    public void move(float v){
        transform.Translate(0, (v-currentLevel)*5, 0);
        currentLevel = v;
        isRotating = false;
    }
}