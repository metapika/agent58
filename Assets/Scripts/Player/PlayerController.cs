using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    //Detect Game Components
    public Rigidbody2D rb;
    public SpriteRenderer glasses;
    private GameManager gameManager;
    public HealthBar healthBar;
    private Heal HPS;
    public float maxVelocity;
    private Ray ray;

    public float thrust;
    public float bounciness = 1000;
    public float powerupDuration = 5;
    public bool canJump, canGrab;

    //Health system
    public int maxHealth = 20;
    public int currentHealth;

    //Walljumping
    public Transform wallBouncePoint;
    public LayerMask whatisGround;

    void Start()
    {
        //Debug
        HPS = GetComponent<Heal>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        rb = gameObject.GetComponent<Rigidbody2D>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        //Limit velocity
        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            TakeDamage(15);
        }

        //Set the healthbar to the current health
        if (healthBar.healthint != currentHealth)
        {
            healthBar.SetHealth(currentHealth);
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = 20;
        }

        //Die when die
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            gameManager.GameOver();
        }

        //Left-Right, duck
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(-transform.right * thrust * Time.deltaTime);
            transform.localScale = new Vector3(-2.5f, 2.5f, 2.5f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(transform.right * thrust * Time.deltaTime);
            transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.up * thrust * Time.deltaTime);
        }

        //Walljumping

        canGrab = Physics2D.OverlapCircle(wallBouncePoint.position, .2f, whatisGround);

        if(canGrab)
        {
            if(transform.localScale.x == 2.5f)
            {
                transform.localScale = new Vector3(-2.5f, 2.5f, 2.5f);
                rb.AddForce(transform.up * bounciness/2 * Time.deltaTime, ForceMode2D.Impulse);
                rb.AddForce(-transform.right * bounciness/2 * Time.deltaTime, ForceMode2D.Impulse);
            }
            else if (transform.localScale.x == -2.5f)
            {
                transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
                rb.AddForce(transform.up * bounciness/2 * Time.deltaTime, ForceMode2D.Impulse);
                rb.AddForce(transform.right * bounciness/2 * Time.deltaTime, ForceMode2D.Impulse);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //DoubleJump Effects
        if (other.CompareTag("DoubleJumpPowerup") && other.gameObject.GetComponent<SpriteRenderer>().enabled == true)
        {
            if(!GetComponent<DoubleJump>())
            {
                gameObject.AddComponent(typeof(DoubleJump));
                other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        //Healing Effects
        if (other.CompareTag("HealPowerup") && other.gameObject.GetComponent<SpriteRenderer>().enabled == true)
        {
            if(!GetComponent<Heal>() && currentHealth != maxHealth)
            {
                gameObject.AddComponent(typeof(Heal));
                other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        //Knockback Effects
        if (other.CompareTag("KnockbackPowerup") && other.gameObject.GetComponent<SpriteRenderer>().enabled == true)
        {
            if(!GetComponent<KnockBack>())
            {
                gameObject.AddComponent(typeof(KnockBack));
                other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        //Speedup Effects
        if (other.CompareTag("SpeedPowerup") && other.gameObject.GetComponent<SpriteRenderer>().enabled == true)
        {
            if(!GetComponent<SpeedUp>())
            {
                gameObject.AddComponent(typeof(SpeedUp));
                other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        //Bounce Effects
        if(other.CompareTag("BouncePowerup") && other.gameObject.GetComponent<SpriteRenderer>().enabled == true)
        {
            if(!GetComponent<BounceUp>())
            {
                gameObject.AddComponent(typeof(BounceUp));
                other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        //Die when going out of screen
        if (other.CompareTag("Death"))
        {
            Destroy(gameObject);
            gameManager.GameOver();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Parameters
        Vector3 wallPosition;
        Vector3 playerPosition = transform.position;
        bool leftSide;

        //Damage from enemies
        if (collision.gameObject.GetComponent<EnemyRotate>())
        {
            //TakeDamage(5);
        }

        //Bouncing from ground
        if (collision.gameObject.CompareTag("BouncyGround"))
        {
            canJump = true;
            rb.AddForce(transform.up * bounciness * Time.deltaTime, ForceMode2D.Impulse);
        }

        //Bouncing from walls
        if (collision.gameObject.CompareTag("Wall"))
        {
            wallPosition = collision.transform.gameObject.transform.position;

            //Define which side of the wall

            leftSide = wallPosition.x > playerPosition.x;

            //Bounce in the opposite way of the wall
            if (leftSide)
            {
                transform.localScale = new Vector3(-2.5f, 2.5f, 2.5f);
                rb.AddForce(transform.up * bounciness * Time.deltaTime, ForceMode2D.Impulse);
                rb.AddForce(-transform.right * bounciness * Time.deltaTime, ForceMode2D.Impulse);
            }
            else
            {
                transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
                rb.AddForce(transform.up * bounciness * Time.deltaTime, ForceMode2D.Impulse);
                rb.AddForce(transform.right * bounciness * Time.deltaTime, ForceMode2D.Impulse);
            }

            
        }
        //Anti Softlock                         NEEDS FIX !!!!!!
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Rigidbody2D enemyRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            enemyRigidbody.AddForce(awayFromPlayer * 10000.0f * Time.deltaTime);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        healthBar.SetHealth(currentHealth);
    }
}