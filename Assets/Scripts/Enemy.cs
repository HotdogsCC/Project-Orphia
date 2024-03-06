using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health = 100;
    private Rigidbody2D enemyRB;

    private void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
    }

    //Called by PlayerMovement script
    public void HitEnemy(int damageDealt, float knockbackDealt)
    {
        health += -damageDealt;
        enemyRB.AddForce(new Vector2(knockbackDealt, 0));
    }

}
