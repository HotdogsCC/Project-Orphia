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
            if (player.enemiesInPHitbox.Count > 0)
            {
                if (!player.isTailingSucking)
                {
                    player.ResetVelocity();
                    player.isTailingSucking = true;
                    player.enemiesInPHitbox[0].BeginTailSucking();
                }
                else
                {
                    player.isTailingSucking = false;
                    player.enemiesInPHitbox[0].EndTailSucking();
                }
                
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
