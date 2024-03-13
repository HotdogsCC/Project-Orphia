using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PAttackCollision : MonoBehaviour
{
    [SerializeField] PlayerMovement player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            player.enemiesInPHitbox.Add(enemy);
        }
        else if (collision.TryGetComponent<DestroyableWall>(out DestroyableWall wall))
        {
            player.currentDestoryableWall = wall;
        }
        else if (collision.TryGetComponent<KeyWall>(out KeyWall kWall))
        {
            player.currentKeyWall = kWall;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            player.enemiesInPHitbox.Remove(enemy);
        }
        else if (collision.TryGetComponent<DestroyableWall>(out DestroyableWall wall))
        {
            player.currentDestoryableWall = null;
        }
    }
}
