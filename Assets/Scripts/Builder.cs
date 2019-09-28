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

    ArrayList links = new ArrayList();
   
    private void Update() {
        if (sys.mode == 2 && !EventSystem.current.IsPointerOverGameObject(0)){
            if (SystemController.isOnAndroid){
                if (Input.touchCount == 1){
                    Touch t = Input.GetTouch(0);
                    if (t.phase == TouchPhase.Began)
                    {
                        Ray ray = main.ScreenPointToRay(t.position);
                        switch(mode){
                        case 0: addLine(ray); break;
                        case 1: delLine(ray); break;
                        case 2: addPlat(ray); break;
                        case 3: recolor(ray); break;
                        case 4: delPlat(ray); break;
                    }

                    }
                }
            }
            else{
                if (Input.GetMouseButtonDown(0)){
                    Ray ray = main.ScreenPointToRay(Input.mousePosition);
                    switch(mode){
                        case 0: addLine(ray); break;
                        case 1: delLine(ray); break;
                        case 2: addPlat(ray); break;
                        case 3: recolor(ray); break;
                        case 4: delPlat(ray); break;
                    }
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
            if (g.tag == "Platform"){
                Platform p = g.GetComponent<Platform>();
                p.Enable(false);
            }
        }
    }
    private void addPlat(Ray ray){
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 20, 1<<9)){
            GameObject g = hit.collider.gameObject;
            if (g.tag=="Platform"){
                Platform p = g.GetComponent<Platform>();
                if (!p.enabled)
                    p.Enable(true);

            }
        }
    }
     private void delPlat(Ray ray){
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 20, 1<<9)){
            GameObject g = hit.collider.gameObject;
            if (g.tag=="Platform"){
                Platform p = g.GetComponent<Platform>();
                if(p.enabled){
                    p.Enable(false);
                    checkLinks();
                }

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
        mode = (mode+1) % 5;
        if (mode == 0)
            debugOut.text = "ADD LINE";
        else if (mode == 1) 
            debugOut.text = "DEL LINE";
        else if (mode == 2) 
            debugOut.text = "PLATFORM"; 
        else if (mode == 3) 
            debugOut.text = "COLORING"; 
        else if (mode == 4) 
            debugOut.text = "SOLV"; 
        
    }

    void checkLinks(){
        List<GameObject> lg = new List<GameObject>();
        foreach(GameObject link in links){
            Link l = link.GetComponent<Link>();
            if (l.endPoints == null)
                Debug.Log("ENDPOINT NULL");
            else if (l.endPoints[0] == null)
                Debug.Log("ENDPOINT 1 NULL");
            else if (l.endPoints[1] == null){
                Debug.Log("ENDPOINT 2 NULL");
                Debug.Log(link.name);
            }
            if (!(l.endPoints[0].enabled || l.endPoints[1].enabled)){
                lg.Add(link);                
            }
        }
        foreach(GameObject link in lg){
            links.Remove(link);
            Destroy(link);
        }
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

          Platform g1, g2;
        g1 = g2 = null;
        int d = (int) (poses[0].x + 2) / 2;
        int e = (int) Mathf.Abs((poses[0].z - 2) / 2);
        int f = (int) Mathf.Abs((poses[0].y - 2) / 2);
        
        int id1 = d+e*3+f*9;
        d = (int) (poses[1].x + 2) / 2;
        e = (int) Mathf.Abs((poses[1].z - 2) / 2);
        f = (int) Mathf.Abs((poses[1].y - 2) / 2);
        int id2 = d+e*3+f*9;
        foreach(Platform p in level.platforms){
            if (p.id == id1)
                g1 = p;
            else if (p.id == id2)
                g2 = p;
            
            if (g1 != null && g2 != null)
                break;
        }

        if (g2 == null){
            Debug.Log(id1 + ", "+id2);
        }
        link.GetComponent<Link>().endPoints[0] = g1;
        link.GetComponent<Link>().endPoints[1] = g2;

        links.Add(link);
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

        // BoxCollider b = link.AddComponent<BoxCollider>();
        // createBox(b, pos0, pos1);              
        lr.useWorldSpace = false;
        numLink++;

        Platform g1, g2;
        g1 = g2 = null;
        int d = (int) (pos0.x + 2) / 2;
        int e = (int) Mathf.Abs((pos0.z - 2) / 2);
        int f = (int) Mathf.Abs((pos0.y - 2) / 2);
        
        int id1 = d+e*3+f*9;
        d = (int) (pos1.x + 2) / 2;
        e = (int) Mathf.Abs((pos1.z - 2) / 2);
        f = (int) Mathf.Abs((pos1.y - 2) / 2);
        int id2 = d+e*3+f*9;
        foreach(Platform p in level.platforms){
            if (p.id == id1)
                g1 = p;
            else if (p.id == id2)
                g2 = p;
            
            if (g1 != null && g2 != null)
                break;
        }

        if (g2 == null){
            Debug.Log(id1 + ", "+id2);
        }
        link.GetComponent<Link>().endPoints[0] = g1;
        link.GetComponent<Link>().endPoints[1] = g2;

        links.Add(link);
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
    
    public void generate(){
        ArrayList edges = Level_Gen.build();
        foreach (int[][] e in edges){
            int x,y,z;
            y = which(e[0][0], 1);
            z = which(e[0][1], 2);
            x = which(e[0][2], 0);
            Vector3 start = new Vector3(x,y,z);
            y = which(e[1][0], 1);
            z = which(e[1][1], 2);
            x = which(e[1][2], 0);
            Vector3 end = new Vector3(x,y,z);
            createLine(start, end);
        }
    }
    
    int which(int i, int c){
        switch(i){
            case 0: return (c == 0)? -2:2;
            case 1: return 0;
            case 2: return (c == 0)? 2:-2;
            default: return -1;
        }
    }

}
