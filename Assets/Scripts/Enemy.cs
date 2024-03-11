using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health = 100;
    private Rigidbody2D enemyRB;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private GameObject hurtParticles;
    [SerializeField] TextMeshProUGUI hText;
    public void Destroyed()
    {
        Instantiate(deathParticles, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        player = FindAnyObjectByType<PlayerMovement>();
    }

    private void Update()
    {
        if(health <= 0)
        {
            Destroyed();
        }
        hText.text = health.ToString();
    }

    //Called by PlayerMovement script
    public void HitEnemy(int damageDealt, float xKnockbackDealt, float yKnockbackDealt)
    {
        Instantiate(hurtParticles, gameObject.transform.position, Quaternion.identity);
        health += -damageDealt;
        if (player.isFacingRight)
        {
            enemyRB.velocity = new Vector2(xKnockbackDealt, yKnockbackDealt);
        }
        else
        {
            enemyRB.velocity = new Vector2(-xKnockbackDealt, yKnockbackDealt);
        }
        
    }

}
