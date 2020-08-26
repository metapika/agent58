using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour
{
    private PlayerController player;
    public float duration = 5;
    public float newSpeed = 15000;
    private float oldSpeed;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        player.glasses.color = new Color(1.0f, 244.0f / 255.0f, 0.0f);
        StartCoroutine(PowerupCountdownRoutine());

        //Speed Powerup
        oldSpeed = player.thrust;
        player.thrust = newSpeed;
    }
    IEnumerator PowerupCountdownRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(duration);
            player.glasses.color = new Color(90.0f / 255.0f, 253.0f / 255.0f, 255.0f / 255.0f);
            player.thrust = oldSpeed;
            Destroy(GetComponent<SpeedUp>());
        }
    }
}
