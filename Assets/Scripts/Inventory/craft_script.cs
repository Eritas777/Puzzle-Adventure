using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mono.Data.Sqlite;
using System.IO;
using UnityEngine.EventSystems;
using System.Data;

public class craft_script : MonoBehaviour
{

    private string dbName = "URI=file:GameProject.db";
    private IDataReader reader;
    
    private int playerID;

    private EventSystem eventSystem;
    private GameObject[] objectsWithTag;
    private TMP_Text[] recept_items;
    public TMP_Text s_name;

    public GameObject create_button;

    // Start is called before the first frame update
    void Start()
    {
        eventSystem = EventSystem.current;
        create_button.SetActive(false);
        objectsWithTag = GameObject.FindGameObjectsWithTag("RItem");
        recept_items = new TMP_Text[objectsWithTag.Length];
        for (int i = 0; i < objectsWithTag.Length; i++)
        {
            recept_items[i] = objectsWithTag[i].GetComponent<TMP_Text>();
        }

        using (var connectionString = new SqliteConnection(dbName)){
            Debug.Log(connectionString.ToString());
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
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void spellclick(){
        string spell_name = "";
        create_button.SetActive(false);
        for(int i = 0; i < objectsWithTag.Length; i++){
            recept_items[i].text = "";
            objectsWithTag[i].SetActive(false);
        }
        if (eventSystem != null && eventSystem.currentSelectedGameObject != null){
            int item_idc = 0;
            string spell_png_name = eventSystem.currentSelectedGameObject.name;
                using (var connectionString = new SqliteConnection(dbName)){
                    connectionString.Open();
                    using (var command = connectionString.CreateCommand()){
                        command.CommandText = "SELECT name FROM spells WHERE png_name IS '"+spell_png_name+"';";
                        spell_name = command.ExecuteScalar().ToString();
                    }
                    s_name.text = spell_name;
                    Debug.Log(s_name.text);

                    using (var command = connectionString.CreateCommand()){
                        command.CommandText = "SELECT i.name AS i_name, s.name FROM items i JOIN spell_recept sr ON i.id = sr.item_id JOIN spells s ON s.id = sr.spell_id WHERE s.png_name = '"+spell_png_name+"';";
                        using(reader = command.ExecuteReader()){
                            while (reader.Read()){
                                objectsWithTag[item_idc].SetActive(true);
                                recept_items[item_idc].text = reader["i_name"].ToString();
                                item_idc++;
                            }
                        }
                    }
                    connectionString.Close();
                }

                using (var connectionString = new SqliteConnection(dbName)){
                    connectionString.Open();
                        using (var command = connectionString.CreateCommand()){
                            command.CommandText = "SELECT COUNT() FROM inventory i INNER JOIN spell_recept sr ON sr.item_id = i.item_id INNER JOIN spells s ON s.id = sr.spell_id WHERE s.png_name = '"+spell_png_name+"' AND player_id = '"+playerID+"';";
                            int result = Convert.ToInt32(command.ExecuteScalar().ToString());
                            Debug.Log(result);
                            if(result == item_idc) create_button.SetActive(true);
                    }
                    connectionString.Close();
                }

            }
    }

    public void spell_craft(){
        foreach (var tmp_t in recept_items)
        {
            if (tmp_t.text != ""){

                using (var connectionString = new SqliteConnection(dbName))
                {
                    connectionString.Open();
                    using (var command = connectionString.CreateCommand())
                    {
                        command.CommandText = "SELECT id FROM items WHERE name = '"+tmp_t.text+"'";
                        var iID = Convert.ToInt32(command.ExecuteScalar().ToString());
                        command.CommandText = "SELECT item_count FROM inventory WHERE item_id = '"+iID+"' AND player_id = '"+playerID+"'";
                        int result = Convert.ToInt32(command.ExecuteScalar().ToString());
                        string query;
                        if (result > 1) query = "UPDATE inventory SET item_count = item_count-1 WHERE item_id = '"+iID+"' AND player_id = '"+playerID+"';";
                        else query = "DELETE FROM inventory WHERE item_id = '"+iID+"' AND player_id = "+playerID+";";
                        command.CommandText = query;
                        command.ExecuteNonQuery();
                    }
                    connectionString.Close();
                }

            }
        }
        using (var connectionString = new SqliteConnection(dbName))
        {
            connectionString.Open();
            using (var command = connectionString.CreateCommand())
            {
                command.CommandText = "SELECT id FROM spells WHERE name = '"+s_name.text.ToString()+"'";
                var sID = Convert.ToInt32(command.ExecuteScalar().ToString());
                Debug.Log("ID заклинания - " + sID.ToString());

                command.CommandText = "SELECT COUNT(*) FROM player_spells WHERE spell_id = '"+sID+"' AND player_id = '"+playerID+"';";
                int result1 = Convert.ToInt32(command.ExecuteScalar().ToString());
                string query;
                if (result1 > 0){
                    query = "UPDATE player_spells SET count = count + 1 WHERE spell_id = '"+sID+"' AND player_id = '"+playerID+"';";
                }
                else{
                    query = "INSERT INTO player_spells (count, spell_id, player_id) VALUES (1, '"+sID+"', '"+playerID+"');";
                }
                command.CommandText = query;
                command.ExecuteNonQuery();
            }
            connectionString.Close();
        }


    }
}
