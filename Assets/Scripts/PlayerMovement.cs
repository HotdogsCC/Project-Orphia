using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //USED ONLY FOR DEVELOPMENT PURPOSES, REMOVE BEFORE RELEASE

public class PlayerMovement : MonoBehaviour
{
    //Assigns references to the player gameobject and rigidbody
    [Header("Objects")]
    private Rigidbody2D playerRB;
    [SerializeField] GameObject doubleJumpParticles;

    //Variables that affect the feel of the player movement
    [Header("Movement")]
    [SerializeField] private float topSpeed = 1f;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float decceleration = 1f;
    [SerializeField] private float jumpStrength = 1f;

    //Variables that affect the feel of the dash
    [Header("Dash")]
    [SerializeField] private float dashSpeed = 0.1f;
    [SerializeField] private float dashDuration = 1f;

    //Important data
    private bool isDashing = false;
    private float dashTime = 0;
    private bool canDoubleJump = true;
    private int health = 100;

    bool IsGrounded()
    {
        return Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.51f), Vector2.down, 0.005f);
    }

    [Header("Other")]
    [SerializeField] private float gravity = -9.81f; //USED ONLY FOR DEVELOPMENT PURPOSES, REMOVE BEFORE RELEASE
    [SerializeField] TextMeshProUGUI vText; //USED ONLY FOR DEVELOPMENT PURPOSES, REMOVE BEFORE RELEASE
    [SerializeField] TextMeshProUGUI hText; //USED ONLY FOR DEVELOPMENT PURPOSES, REMOVE BEFORE RELEASE

    private void Start()
    {
        //Assigns components
        playerRB = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        //If you touch the floor, you can double jump again
        if (IsGrounded())
        {
            canDoubleJump = true;
        }
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - 0.51f), Vector2.down, Color.red, 0.005f);
        Physics2D.gravity = new Vector2(0, gravity); //USED ONLY FOR DEVELOPMENT PURPOSES, REMOVE BEFORE RELEASE

        //During the dash, players shouldnt be able to control the character. This ensures this.
        if (!isDashing)
        {
            //Dashs when Space is clicked
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isDashing = true;
                Dash();
            }
            else
            {
                Movement();
            }
        }
        else
        {
            Dash(); 
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            health += -10;
        }

        vText.text = "current velocity: " + Mathf.RoundToInt(playerRB.velocity.x).ToString(); //Temp, remove later. shows the velocity on screen
        hText.text = "current health: " + health.ToString(); //Temp, remove later. shows the health on screen
    }

    private void Movement()
    {
        //Temp vars
        float newXVelocity = 0;
        Vector2 jumpVector;

        //Sets character x velocity based upon input
        if (Input.GetKey(KeyCode.D))
        {
            newXVelocity = (playerRB.velocity.x + acceleration * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            newXVelocity = (playerRB.velocity.x - acceleration * Time.deltaTime);
        }

        //Jumps
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (IsGrounded())
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, 0);
                jumpVector = new Vector2(0, jumpStrength);
                playerRB.AddForce(jumpVector);
            }
            else if (canDoubleJump)
            {
                Instantiate(doubleJumpParticles, playerRB.transform.position, Quaternion.identity);
                canDoubleJump = false;
                playerRB.velocity = new Vector2(playerRB.velocity.x, 0);
                jumpVector = new Vector2(0, jumpStrength);
                playerRB.AddForce(jumpVector);
            }
        }

        //Slows movement if both or no keys are held
        if ((!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) && playerRB.velocity.x != 0) || (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A)))
        {
            //Checks what direction the player is moving
            if (playerRB.velocity.x > 0)
            {
                newXVelocity = (playerRB.velocity.x - decceleration * Time.deltaTime);
                if (newXVelocity < 0)
                {
                    newXVelocity = 0;
                }
            }
            else
            {
                newXVelocity = (playerRB.velocity.x + decceleration * Time.deltaTime);
                if (newXVelocity > 0)
                {
                    newXVelocity = 0;
                }
            }
        }

        //Sets new player velocity, targetting top speed
        if (playerRB.velocity.x > 10)
        {
            float tempVel = playerRB.velocity.x - decceleration * Time.deltaTime;
            if (tempVel < 10)
            {
                playerRB.velocity = new Vector2(10, playerRB.velocity.y);
            }
            else
            {
                playerRB.velocity = new Vector2(tempVel, playerRB.velocity.y);
            }
        }
        else if(playerRB.velocity.x < -10)
        {
            float tempVel = playerRB.velocity.x + decceleration * Time.deltaTime;
            if (tempVel > -10)
            {
                playerRB.velocity = new Vector2(-10, playerRB.velocity.y);
            }
            else
            {
                playerRB.velocity = new Vector2(tempVel, playerRB.velocity.y);
            }
        }
        else
        {
            playerRB.velocity = new Vector2(Mathf.Clamp(newXVelocity, -topSpeed, topSpeed), playerRB.velocity.y);
        }
    }
    
    private void Dash()
    {
        //Temp var that tracks how long it has been since the dash was initiated. Dash ends when dashTime = 1. The higher dashSpeed is, the quicker this is met
        dashTime += dashSpeed * Time.deltaTime;

        //Mathf.Lerp does not work when the interpolation value is greater than 1. this clamps the value up to 1
        if (dashTime >= 1)
        {
            dashTime = 0;
            isDashing = false;
            playerRB.velocity = new Vector2(topSpeed, 0);
        }

        //Dashs in the direction the player is moving
        if (Input.GetKey(KeyCode.A))
        {
            playerRB.velocity = new Vector2(-dashDuration, 0);
        }
        else
        {
            playerRB.velocity = new Vector2(dashDuration, 0);
        }
    }

   
}
