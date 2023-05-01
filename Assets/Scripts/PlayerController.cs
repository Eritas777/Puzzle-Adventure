using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Data;
using Mono.Data.Sqlite;

public class PlayerController : MonoBehaviour
{
    private string dbName = "URI=file:GameProject.db";
    private IDataReader reader;
    private string playerID;

    public float movespeed = 1f;
    public float collisionOffset = 0.05f;

    public ContactFilter2D movementfilter;

    Vector2 movementInput;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    public GameObject frostyRush, poisonSplash, poweredFrostyRush, aura;
    AuraController auraController;
    GameObject[] magicBarriers;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        auraController = aura.GetComponent<AuraController>();
        magicBarriers = GameObject.FindGameObjectsWithTag("MagicBarrier");

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
            }
            connectionString.Close();
        }

    }

    private void FixedUpdate(){
        if(movementInput != Vector2.zero){
            bool success = TryMove(movementInput);
                if(!success) {
                    success = TryMove(new Vector2(movementInput.x, 0));
                }

                if(!success) {
                    success = TryMove(new Vector2(0, movementInput.y));
                }
                
                animator.SetBool("isMoving", success);}
            else {
                animator.SetBool("isMoving", false);
            }
            // Поворот спрайта персонажа
            if(movementInput.x < 0) {
            spriteRenderer.flipX = true;
            } 
            else if (movementInput.x > 0) {
            spriteRenderer.flipX = false;
            }
    }
    
    private bool TryMove(Vector2 direction) {
        if(direction != Vector2.zero) {
            int count = rb.Cast(direction,movementfilter,castCollisions,movespeed * Time.fixedDeltaTime + collisionOffset);
            if(count == 0){
                rb.MovePosition(rb.position + direction * movespeed * Time.fixedDeltaTime);
                return true;}
            else { return false;}
        } 
        else {return false;}
    }


    void OnMove(InputValue movementValue){
        movementInput = movementValue.Get<Vector2>();
    }

    void FrostyRush()
    {
        GameObject projectileObject = Instantiate(frostyRush, rb.position + Vector2.up * 0.5f, Quaternion.identity);
        FrostyRushProjectile projectile = projectileObject.GetComponent<FrostyRushProjectile>();
        projectile.Launch(movementInput, 300);
    }

    void PoisonSplash()
    {
        GameObject projectileObject = Instantiate(poisonSplash, rb.position + Vector2.up * 0.5f, Quaternion.identity);
        PoisonSplashProjectile projectile = projectileObject.GetComponent<PoisonSplashProjectile>();
        projectile.Launch(movementInput, 300);
    }

    void PoweredFrostyRush()
    {
        GameObject projectileObject = Instantiate(poweredFrostyRush, rb.position + Vector2.up * 0.5f, Quaternion.identity);
        PoweredFrostyRush projectile = projectileObject.GetComponent<PoweredFrostyRush>();
        projectile.Launch(movementInput, 300);
    }

    void Aura()
    {
        auraController.isWorking = true;
        auraController.SpawnAura();
        foreach (GameObject barrier in magicBarriers)
        {
            BoxCollider2D collider = barrier.GetComponent<BoxCollider2D>();
            collider.enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) if (spellCount(1) > 0) {spellUsed(1); FrostyRush();}
        if (Input.GetKeyDown(KeyCode.C)) if (spellCount(2) > 0) {spellUsed(2); PoisonSplash();}
        if (Input.GetKeyDown(KeyCode.X)) if (spellCount(6) > 0) {spellUsed(6); PoweredFrostyRush();}
        if (Input.GetKeyDown(KeyCode.N)) if (spellCount(3) > 0) {spellUsed(3); Aura();}
        if (!auraController.isWorking)
        {
            foreach (GameObject barrier in magicBarriers)
            {
                BoxCollider2D collider = barrier.GetComponent<BoxCollider2D>();
                collider.enabled = true;
            }
        }
    }

    private int spellCount(int spellID){
        int spellc = 0;
        using (var connectionString = new SqliteConnection(dbName)){
            connectionString.Open();

            using (var command = connectionString.CreateCommand()){
                command.CommandText = "SELECT count FROM player_spells WHERE spell_id = '"+spellID+"' AND player_id = '"+playerID+"'";
                var comresult = command.ExecuteScalar();
                if (comresult != null) {
                    spellc = Convert.ToInt32(comresult.ToString());
                }
                else {
                    spellc = 0;
                }
            }
            connectionString.Close();
        }
        return spellc;
    }

    private void spellUsed(int spellID){
        using (var connectionString = new SqliteConnection(dbName))
        {
            connectionString.Open();
            using (var command = connectionString.CreateCommand())
            {
                command.CommandText = "SELECT count FROM player_spells WHERE spell_id = '"+spellID+"' AND player_id = '"+playerID+"'";
                int result = Convert.ToInt32(command.ExecuteScalar().ToString());
                string query;
                if (result > 1) query = "UPDATE player_spells SET count = count-1 WHERE spell_id = '"+spellID+"' AND player_id = '"+playerID+"';";
                else query = "DELETE FROM player_spells WHERE spell_id = '"+spellID+"' AND player_id = "+playerID+";";
                command.CommandText = query;
                command.ExecuteNonQuery();
            }
            connectionString.Close();
        }
    }
}
