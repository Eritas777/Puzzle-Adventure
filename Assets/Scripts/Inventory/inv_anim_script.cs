using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mono.Data.Sqlite;
using System.IO;
using UnityEngine.EventSystems;

public class inv_anim_script : MonoBehaviour
{
    public Animator inv_animator;
    public Animator desc_animator;
    public TMP_Text description;
    int count;

    private EventSystem eventSystem;

    private string dbName = "URI=file:GameProject.db";

    // Start is called before the first frame update
    void Start()
    {
        eventSystem = EventSystem.current;
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        count = 0;
    }

    public void PanelClick(){
        string item_desc = "";
        count++;
        inv_animator.SetInteger("count",count);
        desc_animator.SetInteger("count",count);
        if (eventSystem != null && eventSystem.currentSelectedGameObject != null){
            string item_name = eventSystem.currentSelectedGameObject.name;
            Debug.Log(item_name);

            using(var connectionString = new SqliteConnection(dbName)){
                connectionString.Open();

                using (var command = connectionString.CreateCommand()){
                    command.CommandText = "SELECT description FROM items WHERE png_name IS '"+item_name+"';";
                    item_desc = command.ExecuteScalar().ToString();
                }
                connectionString.Close();
            }

            description.text = item_desc;}

    }


}
