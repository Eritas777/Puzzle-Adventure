using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        auraController = aura.GetComponent<AuraController>();
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
                
                animator.SetBool("isMoving", success);
            } else {
                animator.SetBool("isMoving", false);
            }
            if(movementInput.x < 0) {
            spriteRenderer.flipX = true;
            } 
            else if (movementInput.x > 0) {
            spriteRenderer.flipX = false;
            }
    }

        private bool TryMove(Vector2 direction) {
        if(direction != Vector2.zero) {
            int count = rb.Cast(
                direction,movementfilter,castCollisions,movespeed * Time.fixedDeltaTime + collisionOffset);

            if(count == 0){
                rb.MovePosition(rb.position + direction * movespeed * Time.fixedDeltaTime);
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) FrostyRush();
        if (Input.GetKeyDown(KeyCode.C)) PoisonSplash();
        if (Input.GetKeyDown(KeyCode.X)) PoweredFrostyRush();
        if (Input.GetKeyDown(KeyCode.N)) Aura();
    }
}
