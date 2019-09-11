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
    public bool enabled;
    private Material m;
    private void Start() {
        level = transform.parent.gameObject.GetComponent<Level>();
        enabled = false;
        m = GetComponent<MeshRenderer>().material;
        updateRotation();
        updatePosition();
        updateRnC();
        Enable(enabled);

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

    public void Enable(bool t){
        enabled = t;
        MeshRenderer mr = this.GetComponent<MeshRenderer>();
        if (enabled){
            mr.material = m;
        }
        else
            mr.material = level.materials[1];
    }

}
