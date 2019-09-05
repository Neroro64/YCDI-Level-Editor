using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraCon : MonoBehaviour
{
    [Header("Public Object References")]
    public Level level;
    public SystemController sys;
    public GameObject fColliders; //For checking facing direction
    [Header("Variables")]
    public float rotSpeed;
    public float scalSpeed;

    /*Private variables for rotation and scaling */
    bool isRotating = false;
    bool isScaling = false;
    bool isInverted = false;
    bool finalizeRotation = false;
    Quaternion startRotation;
    Quaternion finalRotation;
    float step;
    /*---------------------------------------------- */

    Camera main;
    private float currentZoom = 1f;
    private float currentLevel = 1f;
    private float accScale = 0;
    private void Start() {
        main = GetComponent<Camera>();
    }

    void Update()
    {
        if (sys.mode == 0 && !EventSystem.current.IsPointerOverGameObject()){
            if (Input.GetMouseButtonDown(0)){
                /*Check first the facing direction of the level */
                RaycastHit hit;
                Ray ray = main.ScreenPointToRay(main.transform.position);
                if (Physics.Raycast(ray, out hit, 10, 1<<8)) // Max distance can be reduced. 
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
                rot.x = ControlFunctions.getClosest(rot.x);
                rot.y = ControlFunctions.getClosest(rot.y);
                rot.z = ControlFunctions.getClosest(rot.z);
                startRotation = level.transform.rotation;
                finalRotation = Quaternion.Euler(rot);
                step = 0.1f;
            }
        }    
        
        if (Input.GetMouseButtonDown(1) && !finalizeRotation)
            isScaling = true;
        else if (Input.GetMouseButtonUp(1))
            isScaling = false;


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
            Quaternion rot = ControlFunctions.calcRot1D(inputX, inputY, rotSpeed);
            level.transform.Rotate(rot.eulerAngles, Space.World);
            fColliders.transform.LookAt(level.transform.forward);
            fColliders.transform.rotation = Quaternion.Euler(0, fColliders.transform.rotation.eulerAngles.y, 0);
            
            foreach (Platform p in level.platforms)   // Can be optimized here
            {
                p.resetRot();
            }
        }
        else if (isScaling){
            float scaling = Input.GetAxis("Mouse X") * scalSpeed * Time.deltaTime;
            accScale += scaling;
            accScale = Mathf.Clamp(accScale, -0.06f, 0.4f); // need to tuned
            if (accScale >= 0.4f || accScale <= -0.06f)
                return;
            Vector3 s = new Vector3();
            foreach(Platform p in level.platforms){   // Can be optimized bere
                s = p.transform.localPosition;
                s += s * scaling;
                // s.x = Mathf.Clamp(s.x, p.minPos.x, p.maxPos.x);
                // s.y = Mathf.Clamp(s.y, p.minPos.y, p.maxPos.y);
                // s.z = Mathf.Clamp(s.z, p.minPos.z, p.maxPos.z);
                p.transform.localPosition = s;
                // p.updatePosition();
                
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