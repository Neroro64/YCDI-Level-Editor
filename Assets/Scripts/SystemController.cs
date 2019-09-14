using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SystemController : MonoBehaviour
{
    [SerializeField]
    public static bool isOnAndroid;
    private static bool inMenu = true;
    private static int saveID;
    private static bool newFile = false;
    [Header("Menu")]
    public Dropdown options;
    
    [Header("SCENE SPECIFIC")]
    public int mode;
    public int rotSpeed;
    public int initRotSpeed;
    public Text debugOut;
    public Text filename;
    public Level level;
    public Builder builder;
    
    public void setMode(int i){
        mode = i;
        switch(mode){
            case 0: debugOut.text = "VIEW"; break; 
            case 1: debugOut.text = "ROT"; break; 
            case 2: debugOut.text = "EDIT"; break; 
        }
    }
    
    IEnumerator LOADING(){
        yield return new WaitForSeconds(.5f);
        SL.load(saveID, level, builder);
    }

    private void Start() {
        if (inMenu){
            if (Application.platform == RuntimePlatform.Android)
                isOnAndroid = true;
                
            SL.init();
            List<Dropdown.OptionData> opts = new List<Dropdown.OptionData>();
            for (int i = 0; i <= SL.saves; i++){
                opts.Add(new Dropdown.OptionData("Save_"+i));
            }        
            options.AddOptions(opts);
        }
        else{
            initRotSpeed = rotSpeed;
            if (newFile){
                saveID = SL.saves+1;
            }
            else
            {
                StartCoroutine(LOADING());
            }
        }

        filename.text = "Save: " + saveID;        
    }
    public void restart(){
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }

    public void startingNew(){
        newFile = true;
        inMenu = false;
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void loadSave(){
        saveID = options.value;
        newFile = false;
        inMenu = false;
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void save(){
        SL.save(saveID, level, builder, newFile);
    }

    public void returnToMenu(){
        // save();
        inMenu = true;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }




}
