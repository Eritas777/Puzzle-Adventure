using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBarrierController : MonoBehaviour
{
    Animator animator;
    public bool stone = false;
    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void GetStoned()
    {
        stone = true;
        animator.SetTrigger("GetStoned");
    }

    public void StoneDestroyed()
    {
        animator.SetTrigger("StoneDestroyed");
        Destroy(gameObject, 1);
    }
}
