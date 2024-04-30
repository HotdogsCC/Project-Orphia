using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        rb = enemyClass.GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        rb.velocity = new Vector2(-moveSpeed, Mathf.Sin(Time.time * wobbleSpeed) * wobbleHeight);
    }
}
