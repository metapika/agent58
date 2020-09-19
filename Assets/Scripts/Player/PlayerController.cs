using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    //Public
    public Rigidbody2D rb;
    public SpriteRenderer glasses;
    public float quickTimeDuration = 5;
    //Private
    private GameManager gameManager;
    private HealthBar healthBar;
    private Heal HPS;
    private GameObject enemy;
    //Health system
    private int maxHealth = 20;
    public int currentHealth;
    //Movement
    public bool canDJump = true;
    public float moveSpeed, jumpForce;
    public float bounciness = 1000;
    public Transform wallBouncePoint;
    public Transform groundCheckPoint;
    public LayerMask whatisGround;
    public bool isGrabbing;
    public bool isGrounded;
    private GameObject ground;
    public float wallPoint;
    //QT

    void Start()
    {
        //Debug
        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        HPS = GetComponent<Heal>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        rb = gameObject.GetComponent<Rigidbody2D>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        ground = GameObject.Find("Ground");
    }

    void FixedUpdate()
    {
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

        //Left-Right movement
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, rb.velocity.y);

        //Check if grounded
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, .2f, whatisGround);
        
        //Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        //Enable doubleJumping
        if(isGrounded)
        {
            canDJump = true;
        }
        
        //Flip direction
        if (rb.velocity.x > 0)
        {
            transform.localScale = Vector3.one;
        }
        else if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1f, 1, 1f);
        }

        //Walljumping
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(new Vector2(wallPoint * transform.localScale.x, Vector2.up.y)), wallPoint, whatisGround);
        
        if (hit)
        {
            if(hit.collider.gameObject == ground & !isGrounded)
            {
                if ((transform.localScale.x == 1f && Input.GetAxisRaw("Horizontal") > 0) || (transform.localScale.x == -1f && Input.GetAxisRaw("Horizontal") < 0))
                {
                    isGrabbing = true;
                }
            }
        }
        else
        {
            isGrabbing = false;
        }
        if (isGrabbing)
        {
            if (Input.GetButtonDown("Jump"))
            {
                rb.velocity = new Vector2(-Input.GetAxisRaw("Horizontal") * moveSpeed*5, jumpForce);
                transform.localScale = new Vector3(-Input.GetAxisRaw("Horizontal"), 0, 0);
                isGrabbing = false;
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
        Vector3 playerPosition = transform.position;

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
    
    public void QuickTimeKnife()
    {

    }
    public IEnumerator QuickTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(quickTimeDuration);
            if(Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("Start of the quick time event");
                //Destroy(other.gameObject);
            }
        }
    }
}