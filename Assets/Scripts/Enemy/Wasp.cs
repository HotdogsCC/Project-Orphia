using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;

public class Wasp : MonoBehaviour
{
    [SerializeField] private Enemy enemyClass;
    [SerializeField] private float wobbleHeight = 1f;
    [SerializeField] private float wobbleSpeed = 1f;
    [SerializeField] private float moveSpeed = 5;

    [SerializeField] public bool movingRight = false;
    public bool agro = true;
    private Rigidbody2D rb;
    [SerializeField] PlayerMovement player;
    private SpriteRenderer sprite;

    private void Start()
    {
        rb = enemyClass.GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        sprite = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (!enemyClass.isStunned && !enemyClass.isTailSucking)
        {
            enemyClass.GetComponent<Rigidbody2D>().gravityScale = 0;
            if (agro)
            {
                if (player.transform.position.x > transform.position.x)
                {
                    movingRight = true;
                }
                else
                {
                    movingRight = false;
                }
            }

            if (movingRight)
            {
                rb.velocity = new Vector2(moveSpeed, Mathf.Sin(Time.time * wobbleSpeed) * wobbleHeight);
                sprite.flipX = false;
            }
            else
            {
                rb.velocity = new Vector2(-moveSpeed, Mathf.Sin(Time.time * wobbleSpeed) * wobbleHeight);
                sprite.flipX = true;
            }

            if(transform.position.y < 5)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + 5);
            }
            if (transform.position.y > 11)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + 5);
            }
        }
        else
        {
            enemyClass.GetComponent<Rigidbody2D>().gravityScale = 1;
        }
        
    }
}
