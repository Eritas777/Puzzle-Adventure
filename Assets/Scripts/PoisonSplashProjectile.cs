using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonSplashProjectile : MonoBehaviour
{
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, float force)
    {
        rb.AddForce(direction * force);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
        CrystalBarrierController barrier = other.collider.GetComponent<CrystalBarrierController>();
        if (barrier.stone) barrier.StoneDestroyed();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
