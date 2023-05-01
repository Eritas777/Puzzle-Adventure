using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mono.Data.Sqlite;
using System.IO;
using UnityEngine.EventSystems;
using System.Data;

public class inv_anim_script : MonoBehaviour
{
    public Animator inv_animator;
    public Animator desc_animator;
    public TMP_Text description;
    int count;
    private int playerID;

    private GameObject[] NameobjectsWithTag;
    private TMP_Text[] item_names;

    private GameObject[] countobjectsWithTag;
    private TMP_Text[] item_count;

    private EventSystem eventSystem;

    private string dbName = "URI=file:GameProject.db";
    private IDataReader reader;

    // Start is called before the first frame update
    void Start()
    {
        NameobjectsWithTag = GameObject.FindGameObjectsWithTag("IName");
        item_names = new TMP_Text[NameobjectsWithTag.Length];
        countobjectsWithTag = GameObject.FindGameObjectsWithTag("counter");
        item_count = new TMP_Text[countobjectsWithTag.Length];

        for (int i = 0; i < NameobjectsWithTag.Length; i++)
        {
            item_names[i] = NameobjectsWithTag[i].GetComponent<TMP_Text>();
        }

        for (int i = 0; i < countobjectsWithTag.Length; i++)
        {
            item_count[i] = countobjectsWithTag[i].GetComponent<TMP_Text>();
        }

        using (var connectionString = new SqliteConnection(dbName)){
            connectionString.Open();

            using (var command = connectionString.CreateCommand()){
                command.CommandText = "SELECT * FROM players;";

                using (reader = command.ExecuteReader()){
                    while (reader.Read()){
                        playerID = Convert.ToInt32(reader["id"].ToString());
                    }
                }
            }
            connectionString.Close();
        } 

        eventSystem = EventSystem.current;
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        using (var connectionString = new SqliteConnection(dbName)){
            connectionString.Open();

            using (var command = connectionString.CreateCommand()){
                for (int i = 0; i < countobjectsWithTag.Length; i++)
                {
                    Debug.Log("i = " + i + ", item name = " + item_names[i].text);
                    command.CommandText = "SELECT id FROM items WHERE name = '"+item_names[i].text+"'";
                    var iID = Convert.ToInt32(command.ExecuteScalar().ToString());
                    command.CommandText = "SELECT item_count FROM inventory WHERE item_id = '"+iID+"' AND player_id = '"+playerID+"'";
                    var comresult = command.ExecuteScalar();
                    if (comresult != null){
                        var result = Convert.ToInt32(comresult.ToString());
                        Debug.Log(result);
                        if (result > 0) item_count[i].text = "Количество: " + result.ToString();
                    }
                    else item_count[i].text = "Количество: 0";
                }
            }

            connectionString.Close();
        } 

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
