using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //Parameters
    private Rigidbody2D rb;
    private GameObject player;
    private GameManager gameManager;
    
    public float bounciness;
    public float thrust = 5.0f;

    void Start()
    {
        //Declare the Player's Rigidbody
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player");
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Bounce of the ground
        if (collision.gameObject.CompareTag("BouncyGround"))
        {
            rb.AddForce(transform.up * bounciness * Time.deltaTime, ForceMode2D.Impulse);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Death"))
        {
            Destroy(gameObject);
        }
    }
}
