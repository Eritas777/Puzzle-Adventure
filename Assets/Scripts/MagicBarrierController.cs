using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBarrierController : MonoBehaviour
{
    public static BoxCollider2D colider;
    // Start is called before the first frame update
    void Start()
    {
        colider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
