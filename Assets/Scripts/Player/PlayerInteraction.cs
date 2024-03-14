using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private bool hasKey = false;
    public KeyWall currentKeyWall;
    [SerializeField] private PlayerMovement player;
    private EnemyCorpse enemyCorpse;

    private void Update()
    {
        Interact();
    }
    private void Interact()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(currentKeyWall != null && hasKey)
            {
                currentKeyWall.Unlocked();
                currentKeyWall = null;
                hasKey = false;
            }
            if(enemyCorpse != null)
            {
                player.IncreaseHealth(enemyCorpse.Consumed());
                enemyCorpse = null;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "key")
        {
            hasKey = true;
            collision.GetComponent<Key>().PickedUp();
        }
        if(collision.tag == "corpse")
        {
            enemyCorpse = collision.GetComponent<EnemyCorpse>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "corpse")
        {
            enemyCorpse = null;
        }
    }
}
