using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspSpit : MonoBehaviour
{
    private PlayerMovement player;
    [SerializeField] GameObject spit;
    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("spitting");
            GameObject spawnedSpit = Instantiate(spit, transform.position, Quaternion.identity);
            spawnedSpit.GetComponent<Spit>().SetTarget(player.transform.position);
        }
        
    }
}
