using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //USED ONLY FOR DEVELOPMENT PURPOSES, REMOVE BEFORE RELEASE
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class PlayerMovement : MonoBehaviour
{
    //Assigns references to the player gameobject and rigidbody
    [Header("Objects")]
    private Rigidbody2D playerRB;
    [SerializeField] GameObject doubleJumpParticles;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private PostProcessVolume ppv;
    private Vignette vignette;
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject primaryAttackHitbox;

    [Header("Camera Control")]
    [SerializeField] private float xOffSet = 0f;
    [SerializeField] private float yOffSet = 0f;

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

    [Header("Primary Attack")]
    [SerializeField] private int pAttackDamage = 10;
    [SerializeField] private int pComboFinishDamage = 15;
    [SerializeField] private float pAttackXKnockback = 1;
    [SerializeField] private float pAttackYKnockback = 1;
    [SerializeField] private float pComboFinishXKnockback = 1;
    [SerializeField] private float pComboFinishYKnockback = 1;
    [SerializeField] private float pAttackCooldown = 0.3f;
    [SerializeField] private int pComboCount = 3;
    [SerializeField] private float pTimeComboIsActive = 0.5f;


    //Important data
    private bool isDashing = false;
    private float dashTime = 0;
    private bool canDoubleJump = true;
    private int health = 100;
    private int rageLevel = 0;
    private bool canPrimaryAttack = true;
    public List<Enemy> enemiesInPHitbox;
    public DestroyableWall currentDestoryableWall;
    public KeyWall currentKeyWall;
    public bool isFacingRight = true;
    private float comboCountdown = 0;
    private int comboCount = 0;
    private float damageDealtMultiplier = 1;
    private bool hasKey = false;

    bool IsGrounded()
    {
        return Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.51f), Vector2.down, 0.01f);
    }

    [Header("Objects")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private Image healthBarColour;
    [SerializeField] private float gravity = -9.81f; //USED ONLY FOR DEVELOPMENT PURPOSES, REMOVE BEFORE RELEASE
    [SerializeField] TextMeshProUGUI vText; //USED ONLY FOR DEVELOPMENT PURPOSES, REMOVE BEFORE RELEASE
    [SerializeField] TextMeshProUGUI hText; //USED ONLY FOR DEVELOPMENT PURPOSES, REMOVE BEFORE RELEASE
    [SerializeField] TextMeshProUGUI rText; //USED ONLY FOR DEVELOPMENT PURPOSES, REMOVE BEFORE RELEASE
    [SerializeField] TextMeshProUGUI cText;

    private void Start()
    {
        //Assigns components
        playerRB = GetComponent<Rigidbody2D>();
        vignette = ppv.profile.GetSetting<Vignette>();
    }
    private void Update()
    {
        //If you touch the floor, you can double jump again
        if (IsGrounded())
        {
            canDoubleJump = true;
        }
        Physics2D.gravity = new Vector2(0, gravity); //USED ONLY FOR DEVELOPMENT PURPOSES, REMOVE BEFORE RELEASE

        //During the dash, players shouldnt be able to control the character. This ensures this.
        if (!isDashing)
        {
            //Dashs when Right Click is clicked
            if (Input.GetMouseButtonDown(1))
            {
                isDashing = true;
                DashCalculations();
            }
            else 
            {
                Movement();
            }
        }
        else
        {
            DashCalculations(); 
        }

        Attack();
        Interact();

        //Temp, remove later.
        if (Input.GetKeyDown(KeyCode.P))
        {
            health += -10;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            health += -1;
        }

        vText.text = "current velocity: " + Mathf.RoundToInt(playerRB.velocity.x).ToString(); //Temp, remove later. shows the velocity on screen
        hText.text = "current health: " + health.ToString(); //Temp, remove later. shows the health on screen
        rText.text = "current rage lvl: " + rageLevel.ToString(); //Temp, remove later. shows the health on screen
        cText.text = "current combo count: " + comboCount.ToString(); //Temp, remove later. shows combo count

        UpdateHealthAndRage();

        mainCamera.transform.position = new Vector3(playerRB.transform.position.x + xOffSet, yOffSet, -10);

        ComboCountdown();
    }

    private void Movement()
    {
        //Temp vars
        float newXVelocity = 0;
        Vector2 jumpVector;

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
        else
        {
            //Sets character x velocity based upon input
            if (Input.GetKey(KeyCode.D))
            {
                newXVelocity = (playerRB.velocity.x + acceleration * Time.deltaTime);
                primaryAttackHitbox.transform.localPosition = new Vector2(1, 0);
                isFacingRight = true;
            }
            if (Input.GetKey(KeyCode.A))
            {
                newXVelocity = (playerRB.velocity.x - acceleration * Time.deltaTime);
                primaryAttackHitbox.transform.localPosition = new Vector2(-1, 0);
                isFacingRight = false;
            }
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
    
    private void DashCalculations()
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

    private void UpdateHealthAndRage()
    {
        healthBar.value = health;
        //spriteRenderer.material.SetFloat("_GrayscaleAmount", 0);
        vignette.intensity.value = (100 - health) * 0.005f;

        if(health < 15)
        {
            healthBarColour.color = new Color(0.2358491f, 0.007787474f, 0.007787474f);
            rageLevel = 4;
            damageDealtMultiplier = 2;
        }
        else if(health < 40)
        {
            healthBarColour.color = new Color(0.7735849f, 0.0204165f, 0.0960312f);
            rageLevel = 3;
            damageDealtMultiplier = 2;
        }
        else if(health < 60)
        {
            healthBarColour.color = new Color(0.772549f, 0.3592402f, 0.1215686f);
            rageLevel = 2;
            damageDealtMultiplier = 1.5f;
        }
        else if(health < 80)
        {
            healthBarColour.color = new Color(0.864151f, 0.7191203f, 0.09732112f);
            rageLevel = 1;
            damageDealtMultiplier = 1.25f;
        }
        else
        {
            healthBarColour.color = new Color(0.5038894f, 0.7647059f, 0.09803921f);
            rageLevel = 0;
            damageDealtMultiplier = 1;
        }
    }

    private void Attack()
    {
        //On left click
        if (Input.GetMouseButton(0) && canPrimaryAttack)
        {
            Debug.Log("attack");
            canPrimaryAttack = false;
            if(enemiesInPHitbox.Count > 0)
            {
                comboCount++;
                comboCountdown = pTimeComboIsActive;
                if (comboCount >= pComboCount)
                {
                    comboCount = 0;
                    foreach (Enemy enemy in enemiesInPHitbox)
                    {
                        enemy.HitEnemy((int)(pComboFinishDamage * damageDealtMultiplier), pComboFinishXKnockback, pComboFinishYKnockback);
                    }
                }
                else
                {
                    foreach (Enemy enemy in enemiesInPHitbox)
                    {
                        enemy.HitEnemy((int)(pAttackDamage * damageDealtMultiplier), pAttackXKnockback, pAttackYKnockback);
                    }
                }
                
            }
            if(currentDestoryableWall != null)
            {
                currentDestoryableWall.Destroyed();
                currentDestoryableWall = null;
            }
            StartCoroutine(WaitAndThen(pAttackCooldown, "canPrimaryAttack"));
        }
    }

    private void Interact()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentKeyWall != null && hasKey)
        {
            currentKeyWall.Unlocked();
            currentKeyWall = null;
            hasKey = false;
        }
    }

    private IEnumerator WaitAndThen(float seconds, string thing)
    {
        yield return new WaitForSeconds(seconds);
        switch (thing)
        {
            case "canPrimaryAttack":
                canPrimaryAttack = true;
                break;
            default:
                Debug.LogError("Thing attached to WaitAndThen is not valid");
                break;
        }
    }

    private void ComboCountdown()
    {
        comboCountdown -= Time.deltaTime;
        if(comboCountdown <= 0)
        {
            comboCountdown = 0;
            comboCount = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "key")
        {
            hasKey = true;
            Key key = collision.GetComponent<Key>();
            key.PickedUp();
        }
    }

}
