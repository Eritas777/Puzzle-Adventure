using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostyRushProjectile : MonoBehaviour
{

    Rigidbody2D rb;

    void Awake()
    {
       rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, float force)
    {
        rb.AddForce(direction * force);
        Destroy(gameObject, 4);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Crystal")
        {
            Destroy(gameObject);
            CrystalBarrierController barrier = other.collider.GetComponent<CrystalBarrierController>();
            barrier.GetStoned();
        }
        else if (other.gameObject.tag == "Fire")
        {
            Destroy(gameObject);
            FireWallController fireWall = other.collider.GetComponent<FireWallController>();
            fireWall.FireWentOut();
        }
        else Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
