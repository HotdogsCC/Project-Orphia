using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //USED ONLY FOR DEVELOPMENT PURPOSES, REMOVE BEFORE RELEASE
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using Unity.VisualScripting;

public class PlayerMovement : MonoBehaviour
{
    //Assigns references to the player gameobject and rigidbody
    [Header("Objects")]
    private Rigidbody2D playerRB;
    [SerializeField] GameObject doubleJumpParticles;
    [SerializeField] GameObject hurtParticles;
    [SerializeField] GameObject deathParticles;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private PostProcessVolume ppv;
    private Vignette vignette;
    [SerializeField] GameObject primaryAttackHitbox;

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
    [SerializeField] private float dashCooldown = 0.5f;

    //Variables that affect the primary attack
    [Header("Primary Attack")]
    [SerializeField] private int pAttackDamage = 10;
    [SerializeField] private int pComboFinishDamage = 15;
    [SerializeField] private float pAttackXKnockback = 1;
    [SerializeField] private float pAttackYKnockback = 1;
    [SerializeField] private float pComboFinishXKnockback = 1;
    [SerializeField] private float pComboFinishYKnockback = 1;
    [SerializeField] private float pAttackDuration = 0.3f;
    [SerializeField] private int pComboCount = 3;
    [SerializeField] private float pTimeComboIsActive = 0.5f;

    [Header("Other")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private float stunTime = 1f;
    private AudioSource aSource;
    [SerializeField] private AudioLowPassFilter lPass;
    [SerializeField] private AudioClip hitSFX;
    [SerializeField] private AudioClip missSFX;



    //Important data
    private bool isDashing = false;
    private float dashTime = 0;
    private bool canDoubleJump = true;
    private int health = 100;
    private bool canPrimaryAttack = true;
    public List<Enemy> enemiesInPHitbox;
    public DestroyableWall currentDestoryableWall;
    public bool isFacingRight = false;
    private float comboCountdown = 0;
    private int comboCount = 0;
    private float damageDealtMultiplier = 1;
    public bool isStunned = false;
    public bool isTailingSucking = false;
    private bool canDash = true;

    //checks whether the player is on the ground
    RaycastHit2D IsGrounded()
    {
        return Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 2.01f), Vector2.down, 0.051f, 32768);
    }

    //for some reason it is detecting itself in the raycast and causing issues when falling. changing the transform offset to 2.01 offet is a shit fix
    //too bad!
    bool CanJump()
    {
        if (IsGrounded())
        {
            if (IsGrounded().transform.tag == "jumpable")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
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
        aSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sharedMaterial.SetFloat("_GrayscaleAmount", 0);
        lPass.cutoffFrequency = 22000;
        //gameOverScreen = GameObject.FindGameObjectWithTag("game over");
    }
    private void Update()
    {
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - 2.01f), Vector2.down, Color.red, 0.051f);

        //If you touch the floor, you can double jump again
        if (CanJump())
        {
            canDoubleJump = true;
        }
        Physics2D.gravity = new Vector2(0, gravity); //USED ONLY FOR DEVELOPMENT PURPOSES, REMOVE BEFORE RELEASE

        //During the dash, players shouldnt be able to control the character. This ensures this.
        if (!isStunned)
        {
            //checks if the player is currently consuming health from an enemy
            if (!isTailingSucking)
            {
                //checks if the player is currently in a dash animation
                if (!isDashing)
                {
                    //Dashs when Right Click is clicked
                    if (Input.GetMouseButtonDown(1) && canDash)
                    {
                        isDashing = true;
                        canDash = false;
                        StartCoroutine(WaitAndThen(dashCooldown, "dash"));
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
            }

            Attack();
        }

        //Temp, remove later.
        if (Input.GetKeyDown(KeyCode.P))
        {
            health += -10;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            health += -1;
        }

        UpdateHealthAndRage();

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
            //Checks what direction the player is moving and sets new velocity based upon direction and decelleration 
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
                transform.localScale = new Vector3(-1, 1, 1);
                isFacingRight = true;
            }
            if (Input.GetKey(KeyCode.A))
            {
                newXVelocity = (playerRB.velocity.x - acceleration * Time.deltaTime);
                transform.localScale = new Vector3(1, 1, 1);
                isFacingRight = false;
            }
        }

        //Jumps
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            //Checks whether Orphia is on the ground. The jump will not consume a double jump if Orphia is grounded
            if (CanJump())
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
        if (!isFacingRight)
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
        float fHealth = health;
        spriteRenderer.sharedMaterial.SetFloat("_GrayscaleAmount", (-fHealth) /100 + 1);
        Debug.Log((-fHealth) / 100 + 1);
        vignette.intensity.value = (100 - health) * 0.005f;
        lPass.cutoffFrequency = health * 220;

        //Assigns health bar colour and value based upon current health
        if (health <= 0)
        {
            gameOverScreen.SetActive(true);
            Instantiate(deathParticles, playerRB.transform.position, Quaternion.identity);
            Destroy(gameObject);
            healthBar.value = 0;
        }
        else if(health < 15)
        {
            healthBarColour.color = new Color(0.2358491f, 0.007787474f, 0.007787474f);
            damageDealtMultiplier = 2;
            healthBar.value = Mathf.RoundToInt(health * 19 / 14);
        }
        else if(health < 40)
        {
            healthBarColour.color = new Color(0.7735849f, 0.0204165f, 0.0960312f);
            damageDealtMultiplier = 2;
            healthBar.value = Mathf.RoundToInt(health * 19 / 24 + 8.125f);
            if(health == 15)
            {
                healthBar.value = 20;
            }
        }
        else if(health < 60)
        {
            healthBarColour.color = new Color(0.772549f, 0.3592402f, 0.1215686f);
            damageDealtMultiplier = 1.5f;
            healthBar.value = health;
        }
        else if(health < 80)
        {
            healthBarColour.color = new Color(0.864151f, 0.7191203f, 0.09732112f);
            damageDealtMultiplier = 1.25f;
            healthBar.value = health;
        }
        else
        {
            healthBarColour.color = new Color(0.5038894f, 0.7647059f, 0.09803921f);
            damageDealtMultiplier = 1;
            healthBar.value = health;
        }
    }

    private void Attack()
    {
        //On left click
        if (Input.GetMouseButton(0) && canPrimaryAttack)
        {
            isTailingSucking = false;
            canPrimaryAttack = false;
            /*
            isStunned = true;
            //Locks player into an animation during an attack, moves based upon direction
            if (isFacingRight)
            {
                playerRB.velocity = new Vector2(pAttackSpeed, playerRB.velocity.y);
            }
            else
            {
                playerRB.velocity = new Vector2(-pAttackSpeed, playerRB.velocity.y);
            }
            */
            //Checks that there are enemies in the hitbox
            if(enemiesInPHitbox.Count > 0)
            {
                aSource.clip = hitSFX;
                
                comboCount++;
                comboCountdown = pTimeComboIsActive;
                //Checks whether the character is at the end of the combo
                if (comboCount >= pComboCount)
                {
                    comboCount = 0;
                    foreach (Enemy enemy in enemiesInPHitbox)
                    {
                        if(playerRB.velocity.x > 11)
                        {
                            enemy.HitEnemy((int)(pComboFinishDamage * damageDealtMultiplier), pComboFinishXKnockback * Mathf.Abs(playerRB.velocity.x), pComboFinishYKnockback);
                        }
                        else
                        {
                            enemy.HitEnemy((int)(pComboFinishDamage * damageDealtMultiplier), pComboFinishXKnockback * 10, pComboFinishYKnockback);
                        }
                        
                        enemy.EndTailSucking();
                    }
                }
                else
                {
                    foreach (Enemy enemy in enemiesInPHitbox)
                    {
                        if (playerRB.velocity.x > 11)
                        {
                            enemy.HitEnemy((int)(pAttackDamage * damageDealtMultiplier), pAttackXKnockback * Mathf.Abs(playerRB.velocity.x), pAttackYKnockback);
                        }
                        else
                        {
                            enemy.HitEnemy((int)(pAttackDamage * damageDealtMultiplier), pAttackXKnockback * 10, pAttackYKnockback);
                        }
                        enemy.EndTailSucking();
                    }
                }
                
            }
            else
            {
                aSource.clip = missSFX;
            }
            aSource.Play();
            //Checks whether the player is facing a wall, destroys the wall if they are
            if (currentDestoryableWall != null)
            {
                currentDestoryableWall.Destroyed();
                currentDestoryableWall = null;
            }
            StartCoroutine(WaitAndThen(pAttackDuration, "canPrimaryAttack"));
        }
    }

    //Responsible for running a function after a specified amount of time.
    private IEnumerator WaitAndThen(float seconds, string thing)
    {
        yield return new WaitForSeconds(seconds);
        switch (thing)
        {
            case "canPrimaryAttack":
                canPrimaryAttack = true;
                //isStunned = false;
                break;
            case "stun":
                isStunned = false;
                break;
            case "dash":
                canDash = true;
                break;
            default:
                Debug.LogError("Thing attached to WaitAndThen is not valid");
                break;
        }
    }

    //responsible for counting down how long a combo lasts for
    private void ComboCountdown()
    {
        comboCountdown -= Time.deltaTime;
        if(comboCountdown <= 0)
        {
            comboCountdown = 0;
            comboCount = 0;
        }
    }

    public void IncreaseHealth(int increase)
    {
        health += increase;
        if(health >= 100)
        {
            health = 100;
        }
    }

    //removes health from character and applies knockback
    public void DamageInflicted(int damageAmount, float xKnockbackInflicted, float yKnockbackInflicted, Vector2 enemyPosition)
    {
        isTailingSucking = false;
        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            enemy.EndTailSucking();
            enemy.EndStun();
        }
        health -= damageAmount;
        UpdateHealthAndRage();
        isStunned = true;
        isDashing = false;
        Instantiate(hurtParticles, gameObject.transform.position, Quaternion.identity);
        ResetVelocity();
        if (gameObject.transform.position.x >= enemyPosition.x)
        {
            playerRB.AddForce(new Vector2(xKnockbackInflicted, yKnockbackInflicted));
        }
        else
        {
            playerRB.AddForce(new Vector2(-xKnockbackInflicted, yKnockbackInflicted));
        }
        StartCoroutine(WaitAndThen(stunTime, "stun"));
        
    }

    public void ResetVelocity()
    {
        playerRB.velocity = new Vector2(0, 0);
    }
}
