using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slug : MonoBehaviour
{
    [SerializeField] private Enemy enemyClass;
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool movingRight = true;
    [SerializeField] private GameObject floor;

    // Update is called once per frame
    void Update()
    {
        //Moves the enemy in a direction at a predetermined speed
        if (!enemyClass.isStunned)
        {
            if (movingRight)
            {
                enemyClass.GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed, enemyClass.GetComponent<Rigidbody2D>().velocity.y);
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                enemyClass.GetComponent<Rigidbody2D>().velocity = new Vector2(-moveSpeed, enemyClass.GetComponent<Rigidbody2D>().velocity.y);
                transform.localScale = new Vector3(1, 1, 1);
            }
        }

    }
}
