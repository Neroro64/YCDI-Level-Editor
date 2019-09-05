using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [Header("Public variables")]
    public Vector3 minPos, maxPos, minScale, maxScale;
    public Quaternion initRotation;
    // public float minPosScaling, maxPosScaling, minScaleScaling, maxScaleScaling;
    public int row;
    public int column;
    public int z;
    // public bool finalizeRotation;
    // public float step = 0;
    // public Quaternion startRotation;
    // public Quaternion finalRotation;
    Transform parent;
    private void Start() {;
        /*Initiating the limits and defualt rotation */
        Vector3 s = new Vector3();
        minPos = transform.localPosition * 2f;
        maxPos = transform.localPosition * 0.75f;
        if (minPos.x > maxPos.x){
            s.x = minPos.x;
            minPos.x = maxPos.x;
            maxPos.x = s.x;
        }
        if (minPos.y > maxPos.y)
        {
            s.y = minPos.y;
            minPos.y = maxPos.y;
            maxPos.y = s.y;
        }
        if (minPos.z > maxPos.z)
        {
            s.z = minPos.z;
            minPos.z = maxPos.z;
            maxPos.z = s.z;
        }

        s /= 4;
        minScale = transform.localScale - s;
        maxScale = transform.localScale + s;

        parent = transform.parent;
        updateRotation();
        updateRnC();
    }

    public void resetRot(){
        transform.rotation = initRotation;
    }
    public void updateRotation(){
        initRotation = transform.rotation;
    }
    public void updatePosition()
    {
        Vector3 vec = transform.position;
        // vec.x = Mathf.RoundToInt(vec.x);
        // vec.y = Mathf.RoundToInt(vec.y);
        // vec.z = Mathf.RoundToInt(vec.z);
        transform.position = vec;

        // minPos = transform.localPosition * 2f;
        // maxPos = transform.localPosition * 0.75f;

    }

    public void updateRnC(){
        /* Assign values to row and column identities */
        row = Mathf.RoundToInt(transform.position.y);
        column = Mathf.RoundToInt(transform.position.x);
        if (transform.eulerAngles.y == 270 || transform.eulerAngles.y == 90)
            column = Mathf.RoundToInt(transform.position.z);
    }

    public void wrapUp(){
        transform.SetParent(parent);
        updatePosition();
        // updateRotation();
        // updateRnC();
        // transform.localPosition = transform.position;
    }


    // private void FixedUpdate() {
    //     if (finalizeRotation){
    //         ControlFunctions.endRotating(gameObject, startRotation, finalRotation, ref step, out finalizeRotation);
    //         if (!finalizeRotation){
    //             updateRotation();
    //             updateRnC();
    //         }
    //     }
    // }
}
