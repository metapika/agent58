using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupMoving : MonoBehaviour
{
    private GameManager gameManager;
    private float useSpeed;
    private float directionSpeed = 1.0f;
    private float origY;
    private float distance = 0.6f;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        origY = transform.position.y;
        useSpeed = -directionSpeed;
    }
    
    void Update()
    {
        if(gameManager.isGameActive)
        {
            if (origY - transform.position.y > distance)
            {
                useSpeed = directionSpeed;
            }
            else if (origY - transform.position.y < -distance)
            {
                useSpeed = -directionSpeed;
            }
            transform.Translate(0, useSpeed * Time.deltaTime, 0);
        }

    }
}
