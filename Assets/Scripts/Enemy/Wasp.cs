using System.Collections;
using System.Collections.Generic;
using TreeEditor;
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

    private void Start()
    {
        rb = enemyClass.GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
    }
    private void Update()
    {
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
        }
        else
        {
            rb.velocity = new Vector2(-moveSpeed, Mathf.Sin(Time.time * wobbleSpeed) * wobbleHeight);
        }
    }
}
