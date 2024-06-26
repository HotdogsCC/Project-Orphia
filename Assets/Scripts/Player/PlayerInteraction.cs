using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public bool hasKey = false; //stores whether the key has been picked up
    public KeyWall currentKeyWall; //stores what wall is currently being faced
    [SerializeField] private PlayerMovement player; //refernce to player movement script
    private List<EnemyCorpse> enemyCorpses = new List<EnemyCorpse>(); //stores what enemy corpse is currently being collided with
    [SerializeField] private AudioSource tailSuckingSFX;

    private void Update()
    {
        Interact();

        if (!player.isTailingSucking)
        {
            tailSuckingSFX.Stop();
        }
    }
    private void Interact()
    {
        if(player.isStunned == false)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //runs when the player interacts with an unlockable wall and has a key for it
                if (currentKeyWall != null && hasKey)
                {
                    currentKeyWall.Unlocked();
                    currentKeyWall = null;
                    hasKey = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                //runs when the player consumes an enemy corpse
                if (enemyCorpses.Count > 0)
                {
                    player.IncreaseHealth(enemyCorpses[0].Consumed());
                }
                //runs when the player is trying to consume health from an enemy
                if (player.enemiesInPHitbox.Count > 0)
                {
                    //if the player isnt consuming health from the enemy, it begins. Otherwise, it stops
                    if (!player.isTailingSucking)
                    {
                        player.ResetVelocity();
                        player.isTailingSucking = true;
                        tailSuckingSFX.Play();
                        player.enemiesInPHitbox[0].BeginTailSucking();
                    }
                    else
                    {
                        player.isTailingSucking = false;
                        tailSuckingSFX.Stop();
                        player.enemiesInPHitbox[0].EndTailSucking();
                    }

                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //stores that the key has been collected, destorys key
        if (collision.tag == "key")
        {
            hasKey = true;
            collision.GetComponent<Key>().PickedUp();
        }
        //stores what corpse is currently being collided with
        if(collision.tag == "corpse")
        {
            enemyCorpses.Add(collision.GetComponent<EnemyCorpse>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //unselects enemy corpse
        if (collision.tag == "corpse")
        {
            enemyCorpses.Remove(collision.GetComponent<EnemyCorpse>());
        }
    }

    
}
