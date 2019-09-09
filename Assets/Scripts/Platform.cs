using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [Header("Public variables")]
    public Level level;
    public Vector3 initPosition;
    public Quaternion initRotation;
    public int row;
    public int column;
    public int z;
    // Transform parent;
    bool enabled = false;
    private void Start() {
        level = transform.parent.gameObject.GetComponent<Level>();
        updateRotation();
        updatePosition();
        updateRnC();
        Enable();
    }

    public void resetRot(){
        transform.rotation = initRotation;
    }
    public void updateRotation(){
        initRotation = transform.rotation;
    }
    public void updatePosition()
    {
        initPosition = transform.localPosition;

    }

    public void updateRnC(){
        /* Assign values to row and column identities */
        row = Mathf.RoundToInt(transform.position.y);
        column = Mathf.RoundToInt(transform.position.x);
        if (transform.eulerAngles.y == 270 || transform.eulerAngles.y == 90)
            column = Mathf.RoundToInt(transform.position.z);
    }

    public void wrapUp(){
        transform.SetParent(level.transform);
    }

    public void Enable(){
        enabled = !enabled;
        MeshRenderer mr = this.GetComponent<MeshRenderer>();
        if (enabled)
            mr.material = level.materials[0];
        else
            mr.material = level.materials[1];
        
    }

}
