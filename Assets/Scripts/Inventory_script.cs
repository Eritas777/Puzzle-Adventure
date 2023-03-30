using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class Inventory_script : MonoBehaviour
{
    private string dbName = "URI=file:GameProject.db";
    private IDataReader reader;
    private string playerID;
    private GameObject[] itemlist;
    string item_id;
    

    Rigidbody2D rbi;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        itemlist = GameObject.FindGameObjectsWithTag("Item");
        using (var connectionString = new SqliteConnection(dbName)){
            Debug.Log(connectionString.ToString());
            connectionString.Open();

            using (var command = connectionString.CreateCommand()){
                command.CommandText = "SELECT * FROM players;";

                using (reader = command.ExecuteReader()){
                    while (reader.Read()){
                        playerID = reader["id"].ToString();
                    }
                }
                Debug.Log(playerID);
            }
            connectionString.Close();
        }
    }

    void Update()
    {
        
        var index = 0;
        foreach (var item in itemlist)
        {
            rbi = item.GetComponent<Rigidbody2D>();
            float dins = (rbi.position-rb.position).magnitude;
            if ((dins < 1)&(Input.GetKeyDown(KeyCode.E))){
                Debug.Log("Подобрано "+item.name.ToString());
                item.transform.position = item.transform.position + new Vector3(-2000,0,0);

                using (var connectionString = new SqliteConnection(dbName)){

                    connectionString.Open();

                    using (var command = connectionString.CreateCommand()){
                        command.CommandText = "SELECT * FROM items WHERE png_name IS '"+item.name.ToString()+"';";
                        Debug.Log("Получен ID");

                        using(reader = command.ExecuteReader()){

                            while (reader.Read()){
                                item_id = reader["id"].ToString();
                            }
                        }
                    }

                    using (var command = connectionString.CreateCommand()){
                        Debug.Log("Проверка на наличие в");
                        command.CommandText = "SELECT COUNT(*) FROM inventory WHERE item_id = '"+item_id+"' AND player_id = '"+playerID+"';";
                        int result = Convert.ToInt32(command.ExecuteScalar().ToString());
                        string query;
                        if (result > 0){
                            query = "UPDATE inventory SET item_count = item_count + 1 WHERE item_id = '"+item_id+"' AND player_id = '"+playerID+"';";
                        }
                        else{
                            query = "INSERT INTO inventory (item_count, item_id, player_id) VALUES (1, '"+item_id+"', '"+playerID+"');";
                        }
                        command.CommandText = query;
                        command.ExecuteNonQuery();
                    }

                }
            }
            index++;
        }
    }
}
