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

    [Header("Public variables")]
    public int rotSpeed;
    int tR, tC;
    bool isRotating = false;
    Platform[] hP = new Platform[9];
    Platform[] vP = new Platform[9];
    char dir;

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
                if (dir == 'H'){
                    foreach(Platform p in hP){
                        rot = p.transform.rotation.eulerAngles;
                        rot.x = ControlFunctions.getClosest(rot.x);
                        rot.y = ControlFunctions.getClosest(rot.y);
                        rot.z = ControlFunctions.getClosest(rot.z);
                        p.startRotation = p.transform.rotation;
                        p.finalRotation = Quaternion.Euler(rot);
                        p.step = 0;
                        p.finalizeRotation = true;
                    }
                }
                else{
                    foreach (Platform p in vP)
                    {
                        rot = p.transform.rotation.eulerAngles;
                        rot.x = ControlFunctions.getClosest(rot.x);
                        rot.y = ControlFunctions.getClosest(rot.y);
                        rot.z = ControlFunctions.getClosest(rot.z);
                        p.startRotation = p.transform.rotation;
                        p.finalRotation = Quaternion.Euler(rot);
                        p.step = 0;
                        p.finalizeRotation = true;
                    }
                }
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
                    p.transform.Rotate(rot.eulerAngles, Space.World);
                }
            }
            else{
                foreach(Platform p in vP){
                    p.transform.Rotate(rot.eulerAngles, Space.World);
                }
            }
        }
    }
}
