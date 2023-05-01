using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

public class Menu_script : MonoBehaviour
{
    public Animator animator;
    private string dbName = "URI=file:GameProject.db";
    private IDataReader reader;

    // Start is called before the first frame update
    void Start()
    {
            using (var connectionString = new SqliteConnection(dbName)){
            connectionString.Open();

            using (var command = connectionString.CreateCommand()){
                
                command.CommandText = "CREATE TABLE IF NOT EXISTS items (id integer NOT NULL, name text NOT NULL, description text, png_name text NOT NULL,CONSTRAINT items_pkey PRIMARY KEY(id));";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE IF NOT EXISTS players (id integer NOT NULL, name text NOT NULL, location_id integer NOT NULL, CONSTRAINT players_pkey PRIMARY KEY (id))";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE IF NOT EXISTS spells(    id integer NOT NULL,    png_name text NOT NULL,    description text,    cooldown integer NOT NULL,    name text NOT NULL,    CONSTRAINT spells_pkey PRIMARY KEY (id))";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE IF NOT EXISTS inventory(    item_count integer NOT NULL,    item_id integer,    player_id integer,    PRIMARY KEY(item_id,player_id),    FOREIGN KEY (item_id) REFERENCES items(id),    FOREIGN KEY (player_id) REFERENCES players(id))";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE IF NOT EXISTS spell_recept(    item_id integer,    spell_id integer,    PRIMARY KEY(item_id,spell_id),    FOREIGN KEY (item_id) REFERENCES items(id),    FOREIGN KEY (spell_id) REFERENCES spells(id))";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TABLE IF NOT EXISTS player_spells (spell_id	INTEGER, player_id	INTEGER, count	INTEGER, FOREIGN KEY(spell_id) REFERENCES spells(id), FOREIGN KEY(player_id) REFERENCES players(id));";
                command.ExecuteNonQuery();
            }
            connectionString.Close(); 
        }   
            AppendRowItem("Морозный камень","На вид обычный камень, на свету переливается голубым. Холоден на ощуп","FS");
            AppendRowItem("Ветренный цветок","Цветок с белыми лепестками, которые колышатся, будто на ветру","WF");
            AppendRowItem("Блуждающий цветок","Цветок с белыми лепестками. Трудно удержать в руках, так как он постоянно пытается вырваться","MF");
            AppendRowItem("Ландыш","Ландыш. Учитель говорил что он ядовитый","LA");
            AppendRowItem("Бутылек с водой","Самая обычная вода","BW");
            AppendRowItem("Трава стихийной мощи","Трава из которой выходят волны нестабильной магической энергии","PG");
            AppendRowItem("Кусок магмы","Застывший кусок магмы. Даже сейчас от него исходит тепло","PM");

            AppendRowSpell("Морозный порыв","Вызывает дуновение холодного ветра.",0,"CW");
            AppendRowSpell("Ядовитый всплеск","Выплескивает небольшое количество яда, который быстро исчезает при соприкосновении с чем-либо.",0,"PS");
            AppendRowSpell("Ветренный рывок","Позволяет сделать персонажу быстрый рывок, который будет поддерживаться ветром. Ветер настолько силён, что персонаж даже не упадёт не будь под ним опоры.",0,"WD");
            AppendRowSpell("Огненный шар","Выпускает сгусток огня, который взрывается при прикосновении",0,"FB");
            AppendRowSpell("Усиленный огненный шар","Выпускает сгусток огня, который взрывается с огромной силой при прикосновении",0,"UFB");
            AppendRowSpell("Усиленый морозный порыв","Вызывает дуновение холодного ветра. беред которым мало что может устоять целым",0,"UCW");

            AppendCraft(1,1);
            AppendCraft(2,1);
            AppendCraft(3,1);
            AppendCraft(4,2);
            AppendCraft(5,2);
            AppendCraft(3,2);
            AppendCraft(2,3);
            AppendCraft(3,3);
            AppendCraft(6,3);
            AppendCraft(7,4);
            AppendCraft(3,4);
            AppendCraft(7,5);
            AppendCraft(3,5);
            AppendCraft(6,5);
            AppendCraft(1,6);
            AppendCraft(2,6);
            AppendCraft(3,6);
            AppendCraft(6,6);
    }   

    public void StartClick(){
        animator.SetBool("active", true);
    }

    // Update is called once per frame  
    void Update()   
    {   

    }

    void AppendRowItem(string name, string description, string png){
            using (var connectionString = new SqliteConnection(dbName)){
            connectionString.Open();

            using (var command = connectionString.CreateCommand()){
                
                command.CommandText = "SELECT COUNT(*) FROM items WHERE name = '"+name+"';";
                        int result = Convert.ToInt32(command.ExecuteScalar().ToString());
                        if (result == 0){
                            command.CommandText = "INSERT INTO items (name, description, png_name) VALUES ('"+name+"', '"+description+"','"+png+"');";
                            command.ExecuteNonQuery();
                        }
            }
            connectionString.Close(); 
        }
    }

    void AppendRowSpell(string name, string description, int cooldown, string png){
            using (var connectionString = new SqliteConnection(dbName)){
            connectionString.Open();

            using (var command = connectionString.CreateCommand()){
                
                command.CommandText = "SELECT COUNT(*) FROM spells WHERE name = '"+name+"';";
                        int result = Convert.ToInt32(command.ExecuteScalar().ToString());
                        if (result == 0){
                            command.CommandText = "INSERT INTO spells (name, description, png_name, cooldown) VALUES ('"+name+"', '"+description+"','"+png+"', "+cooldown+");";
                            command.ExecuteNonQuery();
                        }
            }
            connectionString.Close(); 
        }
    }

    void AppendCraft(int itemID, int spellID){
                    using (var connectionString = new SqliteConnection(dbName)){
            connectionString.Open();

            using (var command = connectionString.CreateCommand()){
                
                command.CommandText = "SELECT COUNT(*) FROM spell_recept WHERE item_id = "+itemID+" AND spell_id = "+spellID+";";
                        int result = Convert.ToInt32(command.ExecuteScalar().ToString());
                        if (result == 0){
                            command.CommandText = "INSERT INTO spell_recept (item_id, spell_id) VALUES ("+itemID+","+spellID+");";
                            command.ExecuteNonQuery();
                        }
            }
            connectionString.Close(); 
        }
    }
}
