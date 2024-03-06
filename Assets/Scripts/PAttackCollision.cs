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
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            player.enemiesInPHitbox.Remove(enemy);
        }
    }
}
