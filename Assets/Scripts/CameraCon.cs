using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraCon : MonoBehaviour
{
    [Header("Public Object References")]
    public Level level;
    public SystemController sys;
    [Header("Variables")]

    /*Private variables for rotation and scaling */
    bool isRotating = false;
    bool isScaling = false;
    bool finalizeRotation = false;
    Quaternion startRotation;
    Quaternion finalRotation;
    float step;
    /*---------------------------------------------- */

    Camera main;
    private float currentZoom = 1f;
    private float currentLevel = 1f;
    private float accScale = 1f;
    private void Start() {
        main = GetComponent<Camera>();
    }

    void Update()
    {
        if (sys.mode == 0 && !EventSystem.current.IsPointerOverGameObject()){
            if (Input.GetMouseButtonDown(0)){
                isRotating = true;
            }
            else if (Input.GetMouseButtonUp(0)){
                isRotating = false;
                finalizeRotation = true;
                Vector3 rot = level.transform.rotation.eulerAngles;
                rot.x = ControlFunctions.getClosest(rot.x);
                rot.y = ControlFunctions.getClosest(rot.y);
                rot.z = ControlFunctions.getClosest(rot.z);
                startRotation = level.transform.rotation;
                finalRotation = Quaternion.Euler(rot);
                step = 0.1f;
            }
        }    
        
        if (Input.GetMouseButtonDown(1) && !finalizeRotation){
            isScaling = true;
        }
        else if (Input.GetMouseButtonUp(1)){
            isScaling = false;
            sys.rotSpeed = (int) ( sys.initRotSpeed * accScale);
        }


        if (finalizeRotation)
        {
            ControlFunctions.endRotating(level.gameObject, startRotation, finalRotation, ref step, ref finalizeRotation);
            foreach (Platform p in level.platforms)
            {
                p.resetRot();
            }
        }

    }

    void FixedUpdate()
    {
        if (isRotating){
            float inputX = Input.GetAxis("Mouse X");
            float inputY = Input.GetAxis("Mouse Y");
            Quaternion rot = ControlFunctions.calcRot1D(inputX, inputY, sys.rotSpeed);
            level.transform.Rotate(rot.eulerAngles, Space.World);
            
            foreach (Platform p in level.platforms)   // Can be optimized here
            {
                p.resetRot();
            }
        }
        else if (isScaling){    // There is a bug when scaling up and down too fast
            float scaling = Input.GetAxis("Mouse X") * Time.deltaTime;
            accScale += scaling;
            accScale = Mathf.Clamp(accScale, 1, 1.5f); // need to tuned
            if (accScale >= 1.5f || accScale <= 1f)
                return;
            foreach(Platform p in level.platforms){   // Can be optimized bere
                p.transform.localPosition += p.transform.localPosition * scaling;
            }
        }
      
    }
    public void zoom(float v){
        transform.Translate(0, 0, (v - currentZoom)*5);
        currentZoom = v;
    }
    public void move(float v){
        transform.Translate(0, (v-currentLevel)*5, 0);
        currentLevel = v;
    }
}
