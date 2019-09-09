using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
    public Platform[] platforms;

    public Material[] materials;
    private void Start() {
        platforms = GetComponentsInChildren<Platform>();    
    }
}