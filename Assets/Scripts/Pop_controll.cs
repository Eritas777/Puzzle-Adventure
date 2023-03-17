using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pop_controll : MonoBehaviour
{
    public float d;
    public float d2;
    private Transform target;
    private Transform finish;
    Rigidbody2D rbp;
    Rigidbody2D rb;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        finish = GameObject.FindGameObjectWithTag("Finish").GetComponent<Transform>();
        rbp = target.GetComponent<Rigidbody2D>();
        rb = finish.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position,new Vector3(target.position.x+d2,target.position.y+d,transform.position.z),20 *Time.deltaTime);
        float dist = (rbp.position-rb.position).magnitude;
        animator.SetFloat("Dist",dist);
    }
}
