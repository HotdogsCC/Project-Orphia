using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //USED ONLY FOR DEVELOPMENT PURPOSES, REMOVE BEFORE RELEASE

public class PlayerMovement : MonoBehaviour
{
    //Assigns references to the player gameobject and rigidbody
    private GameObject player;
    private Rigidbody2D playerRB;
    [SerializeField] BoxCollider2D feet;

    //Variables that affect the feel of the player movement
    [SerializeField] private float topSpeed = 1f;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float decceleration = 1f;
    [SerializeField] private float jumpStrength = 1f;

    //Variables that affect the feel of the dash
    [SerializeField] private float dashSpeed = 0.1f;
    [SerializeField] private float dashDistance = 1f;

    //Important data
    private Vector2 dashStartPos;
    private bool isDashing = false;
    private float dashTime = 0;
    private bool dashDirectionRight = true;
    private bool canDoubleJump;
    bool IsGrounded()
    {
        return Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.51f), Vector2.down, 0.05f);
    }

    [SerializeField] private float gravity = -9.81f; //USED ONLY FOR DEVELOPMENT PURPOSES, REMOVE BEFORE RELEASE
    [SerializeField] TextMeshProUGUI vText; //USED ONLY FOR DEVELOPMENT PURPOSES, REMOVE BEFORE RELEASE

    private void Start()
    {
        //Assigns components
        player = gameObject;
        playerRB = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - 0.51f), Vector2.down, Color.red, 0.05f);
        Physics2D.gravity = new Vector2(0, gravity); //USED ONLY FOR DEVELOPMENT PURPOSES, REMOVE BEFORE RELEASE

        //During the dash, players shouldnt be able to control the character. This ensures this.
        if (!isDashing)
        {
            dashTime = 0;
            Movement();
        }
        else
        {
            Dash();
        }

        //Dashs when Z is clicked
        if (Input.GetKeyDown(KeyCode.Z))
        {
            isDashing = true;
            dashStartPos = player.transform.position;
            Dash();
        }
    }

    private void Movement()
    {
        //Temp vars
        float newXVelocity = 0;
        Vector2 jumpVector;

        //Sets character x velocity based upon input
        if (Input.GetKey(KeyCode.RightArrow))
        {
            newXVelocity = (playerRB.velocity.x + acceleration * Time.deltaTime);
            dashDirectionRight = true;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            newXVelocity = (playerRB.velocity.x - acceleration * Time.deltaTime);
            dashDirectionRight = false;
        }

        //Jumps (this is kinda jank atm and a better solution needs to be in place for a double jump to feel good)
        if (Input.GetKeyDown(KeyCode.UpArrow) && IsGrounded())
        {
            jumpVector = new Vector2(0, jumpStrength);
            playerRB.AddForce(jumpVector);
        }

        //Slows movement if both or no keys are held
        if ((!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow) && playerRB.velocity.x != 0) || (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow)))
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

        //Sets new player velocity
        playerRB.velocity = new Vector2(Mathf.Clamp(newXVelocity, -topSpeed, topSpeed), playerRB.velocity.y);

        vText.text = "current velocity: " + Mathf.RoundToInt(playerRB.velocity.x).ToString(); //Temp, remove later. shows the velocity on screen
    }
    
    private void Dash()
    {
        //Temp var that tracks how long it has been since the dash was initiated. Dash ends when dashTime = 1. The higher dashSpeed is, the quicker this is met
        dashTime += dashSpeed * Time.deltaTime;

        //Mathf.Lerp does not work when the interpolation value is greater than 1. this clamps the value up to 1
        if (dashTime > 1)
        {
            dashTime = 1;
            isDashing = false;
        }

        //Dashs in the direction the player is moving
        if (dashDirectionRight)
        {
            player.transform.position = new Vector2(Mathf.Lerp(dashStartPos.x, dashStartPos.x + dashDistance, dashTime), dashStartPos.y);
        }
        else
        {
            player.transform.position = new Vector2(Mathf.Lerp(dashStartPos.x, dashStartPos.x - dashDistance, dashTime), dashStartPos.y);
        }
    }

}
