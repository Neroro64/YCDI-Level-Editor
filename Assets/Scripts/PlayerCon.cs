using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class PlayerCon : MonoBehaviour
{
    [Header("Public Object References")]
    public SystemController sys;
    public Camera main;
    public Level level;
    public Transform rotatorX;
    public Transform rotatorY;
    Transform rotator;

    [Header("Public variables")]
    public int rotSpeed;
    int tR, tC;
    bool isRotating = false;
    Platform[] hP = new Platform[9];
    Platform[] vP = new Platform[9];

    char dir;
    bool finalizeRotation;
    Quaternion startRot;
    Quaternion finalRot;
    float step;


    private void Update() {
        if (sys.mode == 1 && !EventSystem.current.IsPointerOverGameObject()){
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit hit;
                Ray r = main.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(r, out hit, 20, 1<<9)){  // Max distance may be reduced
                    Platform p = hit.collider.GetComponent<Platform>();
                    for (int i = 0, k=0,j = 0; j < 27; j++){
                        if (level.platforms[j].row == p.row)
                            hP[i++] = level.platforms[j];    
                        if (level.platforms[j].column == p.column)
                            vP[k++] = level.platforms[j];
                    }
                    isRotating = true;
                }
            }
            else if (Input.GetMouseButtonUp(0)){
                isRotating = false;
                Vector3 rot; 
                rotator = (dir=='H')? rotatorX : rotatorY;
                rot = rotator.rotation.eulerAngles;
                rot.x = ControlFunctions.getClosest(rot.x);
                rot.y = ControlFunctions.getClosest(rot.y);
                rot.z = ControlFunctions.getClosest(rot.z);
                startRot = rotator.rotation;
                finalRot = Quaternion.Euler(rot);
                step = 0;

                finalizeRotation = true;
            }
        }
    }

    private void FixedUpdate() {
        if (isRotating){
            float inputX = Input.GetAxis("Mouse X");
            float inputY = Input.GetAxis("Mouse Y");
            Quaternion rot = ControlFunctions.calcRot1D(inputX, inputY, rotSpeed, out dir);
            if (dir == 'H'){
                foreach (Platform p in hP){
                    p.transform.SetParent(rotatorX);
                }
                rotatorX.Rotate(rot.eulerAngles, Space.World);
            }
            else{
                foreach(Platform p in vP){
                    p.transform.SetParent(rotatorY);
                }
                rotatorY.Rotate(rot.eulerAngles, Space.World);
            }
        }
        else if (finalizeRotation){
            ControlFunctions.endRotating(rotator.gameObject, startRot, finalRot, ref step, out finalizeRotation);
            if (!finalizeRotation){
                for(int i = 0; i < 9; i++){
                    if(vP[i] == hP[i])
                        vP[i].wrapUp();
                    else{
                        vP[i].wrapUp();
                        hP[i].wrapUp();
                    }
                }
            }
        }
    }
}
