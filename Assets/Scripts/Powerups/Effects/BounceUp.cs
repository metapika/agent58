using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceUp : MonoBehaviour
{
    private PlayerController player;
    public float duration = 5;
    public float newJumpForce = 3000;
    private float oldJumpForce;
    
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        player.glasses.color = new Color(30.0f / 255.0f, 64.0f / 255.0f, 118.0f / 255.0f);
        StartCoroutine(PowerupCountdownRoutine());

        //Bounce Powerup
        oldJumpForce = player.jumpForce;
        player.jumpForce = newJumpForce;
    }

    IEnumerator PowerupCountdownRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(duration);
            player.glasses.color = new Color(90.0f / 255.0f, 253.0f / 255.0f, 255.0f / 255.0f);
            player.jumpForce = oldJumpForce;
            Destroy(GetComponent<BounceUp>());
        }
    }
}
