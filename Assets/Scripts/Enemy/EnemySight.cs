using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    public bool playerInSight;
    public int seeingDistance = 10;
    public Vector3 personalLastSighting;

    private GameObject player;
    public LayerMask playerLayer;
    public GameObject spoted;

    void Awake()
    {
        player = GameObject.Find("Player");
    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject == player)
        {
            playerInSight = false;

            Vector3 direction = transform.position;
            
            RaycastHit2D hit = Physics2D.Raycast(transform.position + transform.up, transform.TransformDirection, seeingDistance, playerLayer;
            
            if(hit)
            {
                if(hit.collider.gameObject == player)
                {
                    playerInSight = true;
                    spoted.SetActive(true);
                }
            }
        }
    }
}
