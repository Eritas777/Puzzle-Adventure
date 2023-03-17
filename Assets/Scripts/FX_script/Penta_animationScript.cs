using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Penta_animationScript : MonoBehaviour
{
    public GameObject player;
    Rigidbody2D rbp;
    Rigidbody2D rb;
    Animator animator;
    int scenecount;

    // Start is called before the first frame update
    void Start()
    {
        rbp = player.GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        scenecount = SceneManager.sceneCount;
    }

    // Update is called once per frame
    void Update()
    {
        //float dins = Vector2.Distance(player.transform.position, rb.position);
        float dins = (rbp.position-rb.position).magnitude;
        animator.SetFloat("distance",dins);
        if ((dins < 1)&(Input.GetKeyDown(KeyCode.E))){
            int sceneindx = SceneManager.GetActiveScene().buildIndex;
            if (sceneindx == scenecount){
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene(sceneindx+1);
            }
        }
    }
}
