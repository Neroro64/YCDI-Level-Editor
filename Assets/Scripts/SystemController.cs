using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemController : MonoBehaviour
{
    public int mode = 0; // 0 = view mode, 1 = rotate mode

    public void setMode(int i){
        mode = i;
    }
}
