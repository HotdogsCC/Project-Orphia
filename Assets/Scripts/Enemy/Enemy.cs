using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{

    // Meant to be used for assigning health values to enemy corpses, but there isnt really any enemy variety rn
    public enum EnemyType
    {
        Small, Large, Boss
    }

    [SerializeField] private int health = 100;
    [SerializeField] private int damage = 25;
    [SerializeField] private float xKnockbackDealt = 5;
    [SerializeField] private float yKnockbackDealt = 5;
    [SerializeField] EnemyType enemyType = EnemyType.Small;
    private Rigidbody2D enemyRB;
    private PlayerMovement player;
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private GameObject hurtParticles;
    [SerializeField] private GameObject healthSuckParticles;
    [SerializeField] private GameObject breakFreeParts;
    [SerializeField] private GameObject corpse;
    [SerializeField] TextMeshProUGUI hText;
    private GameObject currentHSParticles;
    public bool isTailSucking = false;
    public bool isStunned = false;
    private int stunCount = 0;

    //Runs when the enemy is killed
    public void Destroyed()
    {
        player.isStunned = false;
        player.isTailingSucking = false;
        GameObject corpseGO = Instantiate(corpse, gameObject.transform.position, Quaternion.identity);
        corpseGO.GetComponent<Rigidbody2D>().velocity = enemyRB.velocity;
        corpseGO.GetComponent<EnemyCorpse>().SetEnemyType(enemyType);
        corpseGO = null;
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
            Destroy(currentHSParticles);
            Destroyed();
        }
        hText.text = health.ToString();
        
    }

    //Called by PlayerMovement script
    public void HitEnemy(int damageDealt, float xKnockbackDealt, float yKnockbackDealt)
    {
        isStunned = true;
        stunCount++;
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

    // Runs when the tail sucking action begins
    public void BeginTailSucking()
    {
        isTailSucking = true;
        isStunned = true;
        currentHSParticles = Instantiate(healthSuckParticles, gameObject.transform);
        TailSucking();
    }

    //Called every 0.5 seconds while tail sucking is active, loops from WaitAndThen Coroutine
    private void TailSucking()
    {
        if (isTailSucking)
        {
            HealthConsumed();

            // 20% chance of enemy breaking free each time health is consumed, checked every 0.5 seconds
            if (Random.Range(1, 10) <= 2)
            {
                EndTailSucking();
                EndStun();
                Instantiate(breakFreeParts, gameObject.transform.position, Quaternion.identity);
                player.DamageInflicted(damage, xKnockbackDealt, yKnockbackDealt, gameObject.transform.position);
            }
            else
            {
                StartCoroutine(WaitAndThen(0.5f, "tailSuck"));
            }
        }
    }

    public void EndTailSucking()
    {
        isTailSucking = false;
        Destroy(currentHSParticles);
    }
    public void EndStun()
    {
        isStunned = false;
    }

    //Called during tail sucking, just adds health to the player and removes some from the enemy. Runs every 0.05 seconds
    private void HealthConsumed()
    {
        player.IncreaseHealth(1);
        health -= 1;
        StartCoroutine(WaitAndThen(0.05f, "healthConsumed"));
    }

    //Used for a variety of methods that requires waiting for amounts of time
    private IEnumerator WaitAndThen(float seconds, string thing)
    {
        yield return new WaitForSeconds(seconds);
        switch (thing)
        {
            case "tailSuck":
                TailSucking();
                break;
            case "healthConsumed":
                if (isTailSucking)
                {
                    HealthConsumed();
                }
                break;
            case "stun":
                if (stunCount == 0)
                {
                    isStunned = false;
                }
                break;
            default:
                Debug.LogError("Thing attached to WaitAndThen is not valid");
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Deals damage to the player if collided with
        if (collision.transform.tag == "Player")
        {
            EndTailSucking();
            player.DamageInflicted(damage, xKnockbackDealt, yKnockbackDealt, gameObject.transform.position);
        }

        //Enemy is stunned until touching the ground, and then awakens 0.5 seconds later
        if (collision.transform.tag == "jumpable")
        {
            stunCount = 0;
            StartCoroutine(WaitAndThen(0.5f, "stun"));
        }
    }

}
