using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceUp : MonoBehaviour
{
    private PlayerController player;
    public float duration = 5;
    public float newBounciness = 3000;
    private float oldBounciness;
    
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        player.glasses.color = new Color(30.0f / 255.0f, 64.0f / 255.0f, 118.0f / 255.0f);
        StartCoroutine(PowerupCountdownRoutine());

        //Bounce Powerup
        oldBounciness = player.bounciness;
        player.bounciness = newBounciness;
    }

    IEnumerator PowerupCountdownRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(duration);
            player.glasses.color = new Color(90.0f / 255.0f, 253.0f / 255.0f, 255.0f / 255.0f);
            player.bounciness = oldBounciness;
            Destroy(GetComponent<BounceUp>());
        }
    }
}
