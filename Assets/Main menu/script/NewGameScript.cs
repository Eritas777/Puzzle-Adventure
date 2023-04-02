using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine.UI;

public class NewGameScript : MonoBehaviour
{
    public InputField inputField;
    private string nickname;
    private string dbName = "URI=file:GameProject.db";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame(){
            string name = inputField.text;
            using (var connectionString = new SqliteConnection(dbName)){
            connectionString.Open();

            using (var command = connectionString.CreateCommand()){
                
                command.CommandText = "SELECT COUNT(*) FROM players WHERE name = '"+name+"';";
                        int result = Convert.ToInt32(command.ExecuteScalar().ToString());
                        if (result == 0){
                            command.CommandText = "INSERT INTO players (name, location_id) VALUES ('"+name+"', 1);";
                            command.ExecuteNonQuery();
                        }
            }
            connectionString.Close(); 
        }


        SceneManager.LoadScene(1);
    }
}
