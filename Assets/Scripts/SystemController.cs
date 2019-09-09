using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemController : MonoBehaviour
{
    public int mode;
    public int rotSpeed;
    public int initRotSpeed;
    
    public void setMode(int i){
        mode = i;
    }

    private void Start() {
        initRotSpeed = rotSpeed;    
    }
}
