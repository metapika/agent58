using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemy : MonoBehaviour
{
    public float duration;
    private bool gotTouched = false;
    private float turnedCount = 0;
    public GameObject question, spoted;
    private Vector3 player;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(turnedCount == 0)
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                player = collision.gameObject.transform.position;
                question.SetActive(true);
                StartCoroutine(QuickTimeKnife());
            }
        }
    }
    
    void Update()
    {
        if(gotTouched)
        {
            if(transform.position.x > player.x)
            {
                transform.localScale = new Vector3(-1f, 1, 1f);
            } else if(transform.position.x < player.x)
            {
                transform.localScale = new Vector3(1f, 1, 1f);
            }
        }
    }

    public IEnumerator QuickTimeKnife()
    {
        while (true)
        {
            yield return new WaitForSeconds(duration);
            turnedCount++;
            gotTouched = true;
            question.SetActive(false);
            spoted.SetActive(true);
        }
    }
}
