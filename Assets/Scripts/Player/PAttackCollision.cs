using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.tvOS;

public class PAttackCollision : MonoBehaviour
{
    [SerializeField] PlayerMovement player; //reference to main player control script
    [SerializeField] PlayerInteraction interaction; //reference to player interaction script

    //occurs when anything enters the primary attack hitbox
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            //stores what enemies is in the hitbox
            player.enemiesInPHitbox.Add(enemy);
        }
        if (collision.TryGetComponent<DestroyableWall>(out DestroyableWall wall))
        {
            //stores what wall is being faced
            player.currentDestoryableWall = wall;
        }
        if (collision.TryGetComponent<KeyWall>(out KeyWall kWall))
        {
            //stores what unlockable wall is being faced
            interaction.currentKeyWall = kWall;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            //removes the enemy from the list of enemies in the hitbox
            player.enemiesInPHitbox.Remove(enemy);
        }
        if (collision.TryGetComponent<DestroyableWall>(out DestroyableWall wall))
        {
            //removes wall from the list of walls in the hitbox
            player.currentDestoryableWall = null;
        }
        if (collision.TryGetComponent<KeyWall>(out KeyWall kWall))
        {
            // removes wall from the list of walls in the hitbox
            interaction.currentKeyWall = null;
        }
    }
}
