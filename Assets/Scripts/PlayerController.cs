using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    //Detect Game Components
    private Rigidbody2D rb;
    public SpriteRenderer glasses;
    private GameManager gameManager;
    public HealthBar healthBar;
    private HealOverTime HPS;
    public float maxVelocity;
    private Ray ray;

    private float thrust;
    public float bounciness;
    public float powerupDuration = 5;

    //Powerup bools
    private bool hasDJPowerup;
    private bool canJump;
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
        HPS = GetComponent<HealOverTime>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        rb = gameObject.GetComponent<Rigidbody2D>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        ray = new Ray(transform.position, transform.forward);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward,200,1<<10);
        var boundaryObject = hit.collider?.gameObject;


        var tilemap = boundaryObject?.GetComponent<Tilemap>();


        if (tilemap != null)
        {
            var tilePos = new Vector3Int((int)hit.point.x, (int)hit.point.y, 0);
            Debug.Log(tilemap?.GetTile(tilePos));

        }

        //Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
        //if (Physics.Raycast(ray, out hit))
        //{
        //    if (hit.collider != null)
        //    {
        //        RaycastReturn = hit.collider.gameObject.name;
        //        FoundObject = GameObject.Find(RaycastReturn);
        //        Destroy(FoundObject);
        //        Debug.Log("did hit");
        //    }
        //}
        //Debug.DrawRay(transform.position+Vector3.forward, transform.forward, Color.green, 2, false);
        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
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

        //Left-Right Movement
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

        //Double-Jump Mechanic
        if (Input.GetKeyDown(KeyCode.Space) && hasDJPowerup && canJump)
        {
            canJump = false;
            rb.AddForce(Vector3.up * bounciness * Time.deltaTime, ForceMode2D.Impulse);
        }

        //Speed Powerup
        if (hasSPowerup)
        {
            hasKBPowerup = false;
            hasDJPowerup = false;
            thrust = 10000;
        }
        else
        {
            thrust = 6000;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("DoubleJumpPowerup"))
        {
            glasses.color = new Color(87.0f / 255.0f, 207.0f / 255.0f, 144.0f / 255.0f);
            hasDJPowerup = true;
            hasKBPowerup = false;
            hasSPowerup = false;
            other.gameObject.SetActive(false);

            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(PowerupCountdownRoutine());
        }

        if (other.CompareTag("Death"))
        {
            Destroy(gameObject);
            gameManager.GameOver();
        }

        if (other.CompareTag("KnockbackPowerup"))
        {
            glasses.color = new Color(214.0f / 255.0f, 55.0f / 255.0f, 55.0f / 255.0f);
            hasKBPowerup = true;
            hasDJPowerup = false;
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
            hasDJPowerup = false;
            hasSPowerup = true;
            other.gameObject.SetActive(false);
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(PowerupCountdownRoutine());
        }

        if (other.CompareTag("HealPowerup"))
        {
            if (currentHealth != maxHealth)
            {
                StartCoroutine(HPS.Hps());
            }
            other.gameObject.SetActive(false);
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
            hasDJPowerup = false;
            hasKBPowerup = false;
            hasSPowerup = false;
            glasses.color = new Color(90.0f / 255.0f, 253.0f / 255.0f, 255.0f / 255.0f);
        }
    }
}