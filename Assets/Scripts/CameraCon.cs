using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    private int accScale = 0;
    private void Start() {
        main = GetComponent<Camera>();
    }

    void Update()
    {
        if(SystemController.isOnAndroid && sys.mode == 0){
            if (Input.touchCount > 0){
                if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)){
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                        isRotating = true;
                    else if(Input.GetTouch(0).phase == TouchPhase.Ended){
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
            }
        }
        else{
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
        if (SystemController.isOnAndroid){  // if sys.isonandroid
            // if (isScaling && Input.touchCount == 2){
            //     Touch t1 = Input.GetTouch(0);
            //     Touch t2 = Input.GetTouch(1);
                
            //     Vector2 pos1 = t1.position - t1.deltaPosition;
            //     Vector2 pos2 = t2.position - t2.deltaPosition;
            //     float prevMag = (pos1 - pos2).magnitude;
            //     float currMag = (t1.position - t2.position).magnitude;
            //     float scaling = (currMag - prevMag) * Time.deltaTime;
            //     Debug.Log(scaling);
            //     // accScale += scaling;
            //     accScale = Mathf.Clamp(accScale, 1, 1.5f); // need to tuned
            //     if (accScale >= 1.5f || accScale <= 1f)
            //         return;
            //     foreach(Platform p in level.platforms){   // Can be optimized bere
            //         p.transform.localPosition += p.transform.localPosition * scaling;
            //     }
                
            //     sys.rotSpeed = (int) ( sys.initRotSpeed * accScale);

            // }
            if (isRotating){
                float inputX = Input.GetTouch(0).deltaPosition.x;
                float inputY = Input.GetTouch(0).deltaPosition.y;
                Quaternion rot = ControlFunctions.calcRot1D(inputX, inputY, sys.rotSpeed);
                level.transform.Rotate(rot.eulerAngles, Space.World);
                
                foreach (Platform p in level.platforms)   // Can be optimized here
                {
                    p.resetRot();
                }
            }
        }
        else{
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
            // else if (isScaling){    // There is a bug when scaling up and down too fast
            //     float scaling = Input.GetAxis("Mouse X") * Time.deltaTime;
            //     accScale += scaling;
            //     accScale = Mathf.Clamp(accScale, 1, 1.5f); // need to tuned
            //     if (accScale >= 1.5f || accScale <= 1f)
            //         return;
            //     foreach(Platform p in level.platforms){   // Can be optimized bere
            //         p.transform.localPosition += p.transform.localPosition * scaling;
            //     }
            // }
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
    
    // public void scale(Slider s){
    //     float v = (s.value-accScale);
    //     accScale = s.value;

    //     foreach(Platform p in level.platforms){   // Can be optimized bere
    //         p.transform.localPosition += p.transform.localPosition * v;
    //     }
    // }

    public void scaleUp(){
        if (accScale < 2){
            foreach(Platform p in level.platforms){   // Can be optimized bere
                p.transform.localPosition += p.transform.localPosition * 0.1f;
            }
            accScale++;
        }
    }

    public void scaleDown(){
        
        if (accScale > -2){
            foreach(Platform p in level.platforms){   // Can be optimized bere
                p.transform.localPosition += p.transform.localPosition * -0.1f;
            }
            accScale--;
        }
    }
}
