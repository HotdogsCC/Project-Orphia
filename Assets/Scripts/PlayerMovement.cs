using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //USED ONLY FOR DEVELOPMENT PURPOSES, REMOVE BEFORE RELEASE

public class PlayerMovement : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D playerRB;

    [SerializeField] private float topSpeed = 1f;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float decceleration = 1f;
    [SerializeField] private float jumpStrength = 1f;

    [SerializeField] private float gravity = -9.81f; //USED ONLY FOR DEVELOPMENT PURPOSES, REMOVE BEFORE RELEASE
    [SerializeField] TextMeshProUGUI vText;

    private void Start()
    {
        player = gameObject;
        playerRB = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        Physics2D.gravity = new Vector2(0, gravity); //USED ONLY FOR DEVELOPMENT PURPOSES, REMOVE BEFORE RELEASE

        float newXVelocity = 0;
        Vector2 jumpVector;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            newXVelocity = (playerRB.velocity.x + acceleration * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            newXVelocity = (playerRB.velocity.x - acceleration * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            jumpVector = new Vector2(0, jumpStrength);
            playerRB.AddForce(jumpVector);
        }

        if((!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow) && playerRB.velocity.x != 0) || (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow)))
        {
            if(playerRB.velocity.x > 0)
            {
                newXVelocity = (playerRB.velocity.x - decceleration * Time.deltaTime);
                if(newXVelocity < 0)
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

        playerRB.velocity = new Vector2(Mathf.Clamp(newXVelocity, -topSpeed, topSpeed), playerRB.velocity.y);
        vText.text = "current velocity: " + Mathf.RoundToInt(playerRB.velocity.x).ToString();
    }
}
