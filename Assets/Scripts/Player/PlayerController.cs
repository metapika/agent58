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

    private float thrust;
    public float bounciness;
    public float powerupDuration = 5;

    //Powerup bools
    public bool canJump;
    private Coroutine currentCoroutine = null;

    private bool hasKBPowerup;
    private bool canKnockBack;

    private bool hasSPowerup;

    //Health system

    public int maxHealth = 20;
    public int currentHealth;

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
            rb.AddForce(transform.right * thrust * Time.deltaTime);
            transform.eulerAngles = Vector3.up * 180;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(transform.right * thrust * Time.deltaTime);
            transform.eulerAngles = Vector3.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.up * thrust * Time.deltaTime);
        }

        //Speed Powerup
        if (hasSPowerup)
        {
            hasKBPowerup = false;
            thrust = 10000;
        }
        else
        {
            thrust = 6000;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DoubleJumpPowerup") && other.gameObject.GetComponent<SpriteRenderer>().enabled == true)
        {
            if (!gameObject.GetComponent<DoubleJump>())
            {
                DoubleJump dJ = gameObject.AddComponent(typeof(DoubleJump)) as DoubleJump;
            }
            other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        
        if (other.CompareTag("HealPowerup") && other.gameObject.GetComponent<SpriteRenderer>().enabled == true)
        {
            if (!gameObject.GetComponent<Heal>() && currentHealth != maxHealth)
            {
                Heal hP = gameObject.AddComponent(typeof(Heal)) as Heal;
            }
            if (currentHealth != maxHealth)
            {
                other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        
        if (other.CompareTag("KnockbackPowerup"))
        {
            glasses.color = new Color(214.0f / 255.0f, 55.0f / 255.0f, 55.0f / 255.0f);
            hasKBPowerup = true;
            hasSPowerup = false;
            other.gameObject.SetActive(false);
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(PowerupCountdownRoutine());
        }

        if (other.CompareTag("SpeedPowerup"))
        {
            glasses.color = new Color(1.0f, 244.0f / 255.0f, 0.0f);
            hasKBPowerup = false;
            hasSPowerup = true;
            other.gameObject.SetActive(false);
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(PowerupCountdownRoutine());
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

        if (collision.gameObject.GetComponent<EnemyRotate>())
        {
            TakeDamage(5);
        }

        //Bouncing from ground
        if (collision.gameObject.CompareTag("BouncyGround")) // && rb.velocity.y <= 0
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
                transform.eulerAngles = Vector3.up * 180;
            }
            else
            {
                transform.eulerAngles = Vector3.up;
            }

            rb.AddForce(transform.up * bounciness * Time.deltaTime, ForceMode2D.Impulse);
            rb.AddForce(transform.right * bounciness * Time.deltaTime, ForceMode2D.Impulse);
            //Damage from enemies
            if (collision.gameObject.CompareTag("BouncyGround"))
            {
                TakeDamage(5);
            }
        }

        //Knockback enemies
        if (collision.gameObject.CompareTag("Enemy") && hasKBPowerup)
        {
            Rigidbody2D enemyRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            enemyRigidbody.AddForce(awayFromPlayer * 1000.0f * Time.deltaTime, ForceMode2D.Impulse);
        }
        //Anti Softlock
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

    IEnumerator PowerupCountdownRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(powerupDuration);
            hasKBPowerup = false;
            hasSPowerup = false;
            glasses.color = new Color(90.0f / 255.0f, 253.0f / 255.0f, 255.0f / 255.0f);
        }
    }
}