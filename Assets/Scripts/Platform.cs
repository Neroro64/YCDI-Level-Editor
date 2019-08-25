using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Vector3 minPos, maxPos, minScale, maxScale;
    public Quaternion initRotation;
    // public float minPosScaling, maxPosScaling, minScaleScaling, maxScaleScaling;
    public Transform pTransform;

    private void Start() {;
        Vector3 s = new Vector3();
        minPos = transform.position * 2f;
        maxPos = transform.position * 0.75f;
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

        pTransform = transform;
        initRotation = transform.rotation;
    }
}
