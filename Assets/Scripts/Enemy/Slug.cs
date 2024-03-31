using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slug : MonoBehaviour
{
    [SerializeField] private Enemy enemyClass;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float xJumpForce = 1;
    [SerializeField] private float yJumpForce = 1;
    [SerializeField] private float timeBeforeJump = 0.2f;
    [SerializeField] private float jumpCooldown = 5f;
    [SerializeField] private bool movingRight = true;
    private bool canJump = true;
    [SerializeField] private RectTransform healthDisplay;

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
                healthDisplay.localScale = new Vector3(-0.01f, 0.01f, 0.01f);
            }
            else
            {
                enemyClass.GetComponent<Rigidbody2D>().velocity = new Vector2(-moveSpeed, enemyClass.GetComponent<Rigidbody2D>().velocity.y);
                transform.localScale = new Vector3(1, 1, 1);
                healthDisplay.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            }
        }

    }

    public void Jump()
    {
        if (!enemyClass.isStunned && canJump)
        {
            canJump = false;
            enemyClass.isStunned = true;
            StartCoroutine(WaitAndThen(timeBeforeJump, "jump"));
            StartCoroutine(WaitAndThen(jumpCooldown, "jumpCooldown"));
        }
    }

    private IEnumerator WaitAndThen(float time, string thing)
    {
        yield return new WaitForSeconds(time);
        switch (thing)
        {
            case "jump":
                //enemyClass.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                if (movingRight)
                {
                    enemyClass.GetComponent<Rigidbody2D>().AddForce(new Vector2(xJumpForce * 10000, yJumpForce * 10000));
                }
                else
                {
                    enemyClass.GetComponent<Rigidbody2D>().AddForce(new Vector2(-xJumpForce * 10000, yJumpForce * 10000));
                }
                break;
            case "jumpCooldown":
                canJump = true;
                break;
        }
    }
}
