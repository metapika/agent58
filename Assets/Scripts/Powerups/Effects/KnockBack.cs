using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    private PlayerController player;
    public float thrust = 1000000;
    public float duration = 5;
    public bool knockedBack;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        player.glasses.color = new Color(214.0f / 255.0f, 55.0f / 255.0f, 55.0f / 255.0f);
        StartCoroutine(PowerupCountdownRoutine());
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Knockback enemies
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Rigidbody2D enemyRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
            
            enemyRigidbody.AddForce(awayFromPlayer.normalized * thrust * Time.deltaTime);
            knockedBack = true;
        }
        if(knockedBack)
        {
            player.glasses.color = new Color(90.0f / 255.0f, 253.0f / 255.0f, 255.0f / 255.0f);
            Destroy(GetComponent<KnockBack>());
        }
    }
    IEnumerator PowerupCountdownRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(duration);
            player.glasses.color = new Color(90.0f / 255.0f, 253.0f / 255.0f, 255.0f / 255.0f);
            Destroy(GetComponent<KnockBack>());
        }
    }
}
