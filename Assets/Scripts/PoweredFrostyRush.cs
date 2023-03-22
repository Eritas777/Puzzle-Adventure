using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoweredFrostyRush : MonoBehaviour
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
        WaterController water = other.collider.GetComponent<WaterController>();
        water.Freeze();
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
