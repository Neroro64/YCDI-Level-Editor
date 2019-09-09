using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Builder : MonoBehaviour
{
    public SystemController sys;
    public GameObject Link;
    public Camera main;
    public Level level;

    GameObject[] nodes = new GameObject[2];
    [SerializeField]
    Material[] materials;
    int counter = 0;
    // List<LineRenderer> links = new List<LineRenderer>();
    private int mode = 0; // 0 = line, 1 =  platform, 2 = change color
    private void Update() {
        if (sys.mode == 2 && !EventSystem.current.IsPointerOverGameObject()){
            if (Input.GetMouseButtonDown(0)){
                Ray ray = main.ScreenPointToRay(Input.mousePosition);
                switch(mode){
                    case 0: addLine(ray); break;
                    case 1: addPlat(ray); break;
                    case 2: recolor(ray); break;
                }
            }
            else if (Input.GetMouseButtonDown(1)){
                Ray r = main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(r, out hit, 20, 1<<9)){
                    string t = hit.collider.gameObject.tag;
                    if (t=="Link")
                        Destroy(hit.collider.gameObject);
                    else if (t == "Platform")
                        hit.collider.gameObject.GetComponent<Platform>().Enable(); 
                }
            }
        }
        else
        {
            counter = 0;
        }
    }

    private void addLine(Ray ray){
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 20, 1<<9))
        {
            GameObject g = hit.collider.gameObject;
            Debug.Log(g.name);
            Debug.Log(g.transform.localPosition);
            if (g.tag == "Platform"){
                nodes[counter] = g;
                if (counter == 1){
                    GameObject link;
                    LineRenderer lr;

                    Vector3 pos0 =  nodes[0].transform.localPosition;
                    Vector3 pos1 =  nodes[1].transform.localPosition;
                    int which = 0;
                    if ((lr = g.GetComponent<LineRenderer>()) != null){
                        link = g;
                        which = 1;
                    }
                    else if ((lr = nodes[0].GetComponent<LineRenderer>()) != null)
                        link = nodes[0];
                    else{
                        link = Instantiate<GameObject>(Link, level.transform);
                        link.AddComponent<LineRenderer>();
                    }
                    if (lr == null){
                        lr = link.GetComponent<LineRenderer>();
                        lr.useWorldSpace = true;
                        lr.positionCount = 2;
                        lr.SetPosition(0,pos0);
                        lr.SetPosition(1, pos1);
                    }
                    else{
                        lr.useWorldSpace = true;
                        lr.positionCount += 1;
                        lr.SetPosition(lr.positionCount-1, nodes[which].transform.localPosition);
                    }

                    link.AddComponent<BoxCollider>();
                    BoxCollider b = link.GetComponent<BoxCollider>();
                    Vector3 v = new Vector3();
                    v.x = (pos1.x + pos0.x) / 2;
                    v.y = (pos1.y + pos0.y) / 2;
                    v.z = (pos1.z + pos0.z) / 2;
                    b.center = v;
                    v.x = v.z = .5f;
                    v.y = 2f;
                    b.size = v;

                    lr.useWorldSpace = false;
                    counter = 0;
                }
                else{
                    counter++;
                } 
            }
        }
        
    }
    private void addPlat(Ray ray){
   
        
    }
    private void recolor(Ray ray){
    }

}
