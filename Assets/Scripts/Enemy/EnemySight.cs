using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    public bool playerInSight;
    public float seeingDistance = 10f;
    public Vector3 personalLastSighting;
    private GameManager gameManager;

    private GameObject player;
    public LayerMask playerLayer;
    public GameObject spoted;
    float z;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player");
    }
    void Update()
    {
        playerInSight = false;

        Vector3 direction = transform.position;

        if(transform.localScale.x == 1)
        {
            z = (transform.position.x + seeingDistance) * transform.localScale.x;
        } else if (transform.localScale.x == -1)
        {
            z = (transform.position.x - seeingDistance);
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(new Vector2(seeingDistance * transform.localScale.x, Vector2.up.y)), seeingDistance, playerLayer);
        //Debug.DrawLine(transform.position, new Vector2(z, transform.position.y), Color.green); ;

        if (hit)
        {
            if(hit.collider.gameObject == player)
            {
                playerInSight = true;
                spoted.SetActive(true);
            }
        }
        if(playerInSight)
        {
            gameManager.GameOver();
        }
    }
}
