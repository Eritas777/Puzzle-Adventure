using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inv_positioning : MonoBehaviour
{
    public float d;
    public float d2;
    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position,new Vector3(target.position.x+d2,target.position.y+d,transform.position.z),10 *Time.deltaTime);
    }
}
