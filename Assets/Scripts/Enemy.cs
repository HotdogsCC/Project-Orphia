using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
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
    [SerializeField] private GameObject corpse;
    [SerializeField] TextMeshProUGUI hText;
    private bool isTailSucking = false;
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

    public void BeginTailSucking()
    {
        isTailSucking = true;
        TailSucking();
    }

    private void TailSucking()
    {
        if (isTailSucking)
        {
            HealthConsumed();
            if (Random.Range(1, 10) <= 2)
            {
                isTailSucking = false;
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
    }


    private void HealthConsumed()
    {
        player.IncreaseHealth(1);
        health -= 1;
        StartCoroutine(WaitAndThen(0.05f, "healthConsumed"));
    }

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
            default:
                Debug.LogError("Thing attached to WaitAndThen is not valid");
                break;
        }
    }

}
