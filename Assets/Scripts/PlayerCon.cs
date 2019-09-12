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
    bool isRotating = false;
    Platform[] hP = new Platform[9];
    Platform[] vP = new Platform[9];
    Platform[] zP = new Platform[9];

    char dir = '0';
    bool finalizeRotation;
    bool isInside;
    Quaternion startRot;
    Quaternion finalRot;
    float step;
    
    private Vector2 initPos;
    private void Update() {
        if (sys.mode == 1 && !EventSystem.current.IsPointerOverGameObject()){
            if (Input.GetMouseButtonDown(0) && !finalizeRotation) {
                RaycastHit hit;
                Ray r = main.ScreenPointToRay(Input.mousePosition);
                initPos = main.ScreenToViewportPoint(Input.mousePosition);
                if(Physics.Raycast(r, out hit, 20, 1<<9)){  // Max distance may be reduced
                    Platform p = hit.collider.GetComponent<Platform>();
                    p.updateRnC();
                    
                    for (int i = 0, k=0, l=0,j = 0; j < 27; j++){
                        level.platforms[j].updateRnC();
                        if (i < 9 && level.platforms[j].row == p.row)
                            hP[i++] = level.platforms[j];    
                        if (k < 9 && level.platforms[j].column == p.column)
                            vP[k++] = level.platforms[j];
                        if (l < 9 && level.platforms[j].z == p.z)
                            zP[l++] = level.platforms[j];
                    }
                    isRotating = true;
                    isInside = true;
                }
            }
            else if (Input.GetMouseButtonUp(0) && isRotating){
                isRotating = false;
                Vector3 rot; 
                rotator = (dir=='H')? rotatorX : rotatorY;
                rot = rotator.rotation.eulerAngles;
                rot.x = ControlFunctions.getClosest(rot.x);
                rot.y = ControlFunctions.getClosest(rot.y);
                rot.z = ControlFunctions.getClosest(rot.z);
                startRot = rotator.rotation;
                finalRot = Quaternion.Euler(rot);
                step = 0.1f;
                dir = '0';
                finalizeRotation = true;
            } 
        }
        if (finalizeRotation)
            {
                ControlFunctions.endRotating(rotator.gameObject, startRot, finalRot, ref step, ref finalizeRotation);
                for (int i = 0; i < 9; i++)
                {
                    vP[i].resetRot();
                    hP[i].resetRot();
                }
                if (!finalizeRotation)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        vP[i].wrapUp();
                        hP[i].wrapUp();
                    }
                }
            }
    }

    private void FixedUpdate() {
        if (isRotating){
            float inputX = Input.GetAxis("Mouse X");
            float inputY = Input.GetAxis("Mouse Y");
            Quaternion rot;
            if (dir=='0' || (isInside && 
            ControlFunctions.isInsideRadius(
                main.ScreenToViewportPoint(
                    Input.mousePosition), initPos, 2f))){
                rot = ControlFunctions.calcRot1D(inputX, inputY, sys.rotSpeed, ref dir);
                rotatorX.transform.rotation = hP[0].transform.localRotation;
                rotatorY.transform.rotation = vP[0].transform.localRotation;
            }
            else if (dir == 'H'){
                rot = Quaternion.Euler(-inputX*sys.rotSpeed*Time.deltaTime*Vector3.up);
                foreach (Platform p in hP){
                    p.transform.SetParent(rotatorX);
                }
                rotatorX.Rotate(rot.eulerAngles, Space.World);
                
                isInside = false;
            }
            else{
                rot = Quaternion.Euler(inputY*sys.rotSpeed*Time.deltaTime*Vector3.right);
                foreach(Platform p in vP){
                    p.transform.SetParent(rotatorY);
                }
                rotatorY.Rotate(rot.eulerAngles, Space.World);
                
                isInside = false;
            }
            for (int i = 0; i < 9; i++){
                vP[i].resetRot();
                hP[i].resetRot();
            }
        }
    }
}
