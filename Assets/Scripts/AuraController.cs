using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AuraController : MonoBehaviour
{
    private Transform player;
    SpriteRenderer sr;
    public bool isWorking;
    float auraTimer = 3;
    float timeAura;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(player.position.x, player.position.y + 0.2f, transform.position.z), 20 * Time.deltaTime);
        if (isWorking)
        {
            timeAura -= Time.deltaTime;
            if (timeAura <= 0) isWorking = false;
            MagicBarrierController.colider.enabled = false;
        }
        Debug.Log(timeAura);
        if (!isWorking)
        {
            sr.enabled = false;
            MagicBarrierController.colider.enabled = true;
        }
    }

    public void SpawnAura()
    {
        sr.enabled = true;
        timeAura = auraTimer;
    }
}
