using UnityEngine;

public class ControlFunctions{
    public static Quaternion calcRot1D(float inputX, float inputY, float speed, ref char direction){
        Quaternion rot;
        if (Mathf.Abs(inputX) > Mathf.Abs(inputY))
        {
            rot = Quaternion.Euler(-inputX * Vector3.up);
            direction = 'H';
        }
        else
        {
            rot = Quaternion.Euler(inputY * Vector3.right);
            direction = 'V';
        }

        return rot;
    }
    public static Quaternion calcRot1D(float inputX, float inputY, float speed)
    {
        Quaternion rot;
        float dir;
        if (Mathf.Abs(inputX) > Mathf.Abs(inputY))
        {
            dir = inputX * speed * Time.deltaTime;
            rot = Quaternion.Euler(-dir * Vector3.up);
        }
        else
        {
            dir = inputY * speed * Time.deltaTime;
            rot = Quaternion.Euler(dir * Vector3.right);
        }

        return rot;
    }

    public static void endRotating(GameObject g, 
        Quaternion startRotation, Quaternion finalRotation, 
        ref float step, ref bool finalizeRotation)
    { 

        g.transform.rotation = Quaternion.Lerp(startRotation, finalRotation, step);
        step += Time.deltaTime * (1 / step);
        if (step > 0.98f){
            g.transform.rotation = finalRotation;
            finalizeRotation = false;
        }
    }


    public static float getClosest(float v)// 0, 90, 180, 270
    {
        float min_diff = v;
        float diff;
        float id = 0;

        diff = Mathf.Abs(90 - v);
        if (diff < min_diff)
        {
            id = 90f;
            min_diff = diff;
        }

        diff = Mathf.Abs(180 - v);
        if (diff < min_diff)
        {
            id = 180f;
            min_diff = diff;
        }

        diff = Mathf.Abs(270 - v);
        if (diff < min_diff)
        {
            id = 270f;
            min_diff = diff;
        }

        diff = Mathf.Abs(360 - v);
        if (diff < min_diff)
        {
            id = 0f;
            min_diff = diff;
        }
        return id;
    }


}