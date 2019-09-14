using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class SL : ScriptableObject{
    [SerializeField]
    public static float version;
    [SerializeField]
    public static int saves;
    public static void init(){
        // PlayerPrefs.DeleteAll();
        version = PlayerPrefs.GetFloat("Version", -1);
        if (version == -1)
            PlayerPrefs.SetFloat("Version", 1.0f);
        
        saves = PlayerPrefs.GetInt("Saves", -1);
        if (saves == -1){
            PlayerPrefs.SetInt("Saves", -1);
        }   
    }


    /*
    SAVE
    -> N: number of active platforms
    -> Followed by N indices, indicating which are enabled
    -> L: number of links
    -> for each link: 
    ->  M: material index
    ->  Followed by 2 Vector3 coordinates for each of the points
 */
    public static void save(int id, Level level, Builder builder, bool newSave){ 
        string savePath = Path.Combine(Application.persistentDataPath, "save_"+id);
        using (
			var writer = new BinaryWriter(File.Open(savePath, FileMode.Create))
		) {
            
            List<int> ps = new List<int>();
            for (int i = 0; i < level.platforms.Length; i++){
                if (level.platforms[i].enabled)
                    ps.Add(i);
            }
            writer.Write(ps.Count); // number of enabled platforms
            foreach(int i in ps)    // save the id:s of these platforms
                writer.Write(i); 
            
            
            int mat_id = 0;
            Vector3 pos;
            LineRenderer[] links = level.GetComponentsInChildren<LineRenderer>();
            writer.Write(links.Length);  // number of links
            foreach(LineRenderer lr in links){
                mat_id = int.Parse(lr.gameObject.name[lr.gameObject.name.Length - 1].ToString());
                Debug.Log(mat_id);
                writer.Write(mat_id);
                pos = lr.GetPosition(0);
                writer.Write(pos.x);
                writer.Write(pos.y);
                writer.Write(pos.z);
                pos = lr.GetPosition(1);
                writer.Write(pos.x);
                writer.Write(pos.y);
                writer.Write(pos.z);
            }
        }

        if (newSave){

            PlayerPrefs.SetInt("Saves", saves+1);
            PlayerPrefs.Save();
        }
    }

    /*
    LOAD
    -> N: number of active platforms
    -> Followed by N indices, indicating which are enabled
    -> L: number of links
    -> for each link: 
    ->  M: material index
    ->  Followed by 2 Vector3 coordinates for each of the points
 */
    public static void load(int save_id, Level level, Builder builder){
        string savePath = Path.Combine(Application.persistentDataPath, "save_"+save_id);       
        using (
			var reader = new BinaryReader(File.Open(savePath, FileMode.Open))
		) {
            int n = reader.ReadInt32();
            for (int i = 0; i < n; i++){
                int id = reader.ReadInt32();
                level.platforms[id].Enable(true);
            }
            
            int l = reader.ReadInt32();
            for (int i = 0; i < l; i++){
                int mat_id = reader.ReadInt32();
                Vector3[] poses = new Vector3[2];
                for ( int j = 0; j < 2; j++){
                    poses[j].x = reader.ReadSingle();
                    poses[j].y = reader.ReadSingle();
                    poses[j].z = reader.ReadSingle();
                }
                builder.createLine(mat_id, 2, poses);
            }
        }
    }

}