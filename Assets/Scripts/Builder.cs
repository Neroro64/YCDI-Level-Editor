using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Builder : MonoBehaviour
{
    public SystemController sys;
    public GameObject Link;
    public Camera main;
    public Level level;
    public Text debugOut;

    GameObject[] nodes = new GameObject[2];
    [SerializeField]
    Material[] link_materials;
    [SerializeField]
    Material[] plat_materials;
    int counter = 0;
    int mat_counter = 1;
    int numLink = 0;
    private int mode = 0; // 0 = line, 1 =  platform, 2 = change color
    private void Update() {
        if (sys.mode == 2 && !EventSystem.current.IsPointerOverGameObject()){
            if (Input.GetMouseButtonDown(0)){
                Ray ray = main.ScreenPointToRay(Input.mousePosition);
                switch(mode){
                    case 0: addLine(ray); break;
                    case 1: delLine(ray); break;
                    case 2: addPlat(ray); break;
                    case 3: recolor(ray); break;
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
            Platform p = g.GetComponent<Platform>();
            if (p != null && p.enabled){
                nodes[counter] = g;
                if (counter == 1){
                    Vector3 pos0 =  nodes[0].transform.localPosition;
                    Vector3 pos1 =  nodes[1].transform.localPosition;

                    createLine(pos0, pos1);
                    counter = 0;
                }
                else{
                    counter++;
                } 
            }
        }
        
    }
    private void delLine(Ray ray){
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 20, 1<<9)){
            GameObject g = hit.collider.gameObject;
            if (g.tag=="Link"){

                Destroy(g);
            }
        }
    }
    private void addPlat(Ray ray){
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 20, 1<<9)){
            GameObject g = hit.collider.gameObject;
            if (g.tag=="Platform"){
                Platform p = g.GetComponent<Platform>();
                p.Enable(!p.enabled);
            }
        }
    }
    private void recolor(Ray ray){
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 20, 1<<9))
        {
            GameObject g = hit.collider.gameObject;

            if (g.tag == "Link"){
                g.GetComponent<LineRenderer>().material = link_materials[mat_counter];
                g.name = g.name.Substring(0,7) + mat_counter;
            }
            else if (g.tag == "Platform")
                g.GetComponent<MeshRenderer>().material = plat_materials[mat_counter];

            mat_counter =( mat_counter + 1) % 3;
        }
    }

    public void switchMode(){
        mode = (mode+1) % 4;
        if (mode == 0)
            debugOut.text = "ADD LINE";
        else if (mode == 1) 
            debugOut.text = "DEL LINE";
        else if (mode == 2) 
            debugOut.text = "PLATFORM"; 
        else if (mode == 3) 
            debugOut.text = "COLORING"; 
    }

    public void createLine(int mat_id, int posC, Vector3[] poses){
        GameObject link = Instantiate<GameObject>(Link, level.transform);
        link.name = "Link_" + numLink + "_" + mat_id;        
        link.AddComponent<LineRenderer>();
        LineRenderer lr = link.GetComponent<LineRenderer>();
        lr.material = link_materials[mat_id];
        lr.useWorldSpace = true;
        lr.positionCount = posC;
        lr.SetPositions(poses);

        int n_b = (poses.Length % 2) * 2;
        int i, j;
        i = j = 0;
        do{
            BoxCollider b = link.AddComponent<BoxCollider>();
            createBox(b, poses[j], poses[++j]);              
            i++;
        }while(i < n_b);
        lr.useWorldSpace = false;
        numLink++;
    }
    public void createLine(Vector3 pos0, Vector3 pos1){
        GameObject link = Instantiate<GameObject>(Link, level.transform);
        link.name = "Link_" + numLink + "_0";
        link.AddComponent<LineRenderer>();
        LineRenderer lr = link.GetComponent<LineRenderer>();
        lr.material = link_materials[0];
        lr.useWorldSpace = true;
        lr.positionCount = 2;
        lr.SetPosition(0, pos0);
        lr.SetPosition(1, pos1);

        BoxCollider b = link.AddComponent<BoxCollider>();
        createBox(b, pos0, pos1);              
        lr.useWorldSpace = false;
        numLink++;

    }
    void appendLine(){

    }
    void createBox(BoxCollider b, Vector3 pos0, Vector3 pos1){
        Vector3 v = new Vector3();
        v.x = (pos1.x + pos0.x) / 2;
        v.y = (pos1.y + pos0.y) / 2;
        v.z = (pos1.z + pos0.z) / 2;
        b.center = v;

        v.x = Mathf.Abs(pos0.x - pos1.x);
        if (v.x == 0)
            v.x = 0.5f;
        
        v.y = Mathf.Abs(pos0.y - pos1.y);
        if (v.y == 0)
            v.y = 0.5f;
            
        v.z = Mathf.Abs(pos0.z - pos1.z);
        if (v.z == 0)
            v.z = 0.5f;
        b.size = v;
    }
    

}
