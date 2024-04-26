using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspSpit : MonoBehaviour
{
    private PlayerMovement player;
    private GameObject parent;
    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("spitting");
            Debug.Log(Vector2.Angle(transform.position, player.transform.position));
        }
        
    }
}
