using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    private PlayerController player;
    public int amount = 5;
    public int delay = 0;
    public int applyHealNTimes = 3;
    public float applyEveryNSeconds = .5f;
    
    private int appliedTimes = 0;

    public void Start() 
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        player.glasses.color = new Color(255.0f / 255.0f, 128.0f / 255.0f, 128.0f / 255.0f);
        StartCoroutine(Hps());
    }

    public IEnumerator Hps()
    {
        appliedTimes = 0;
        
        yield return new WaitForSeconds(delay);

        while(appliedTimes < applyHealNTimes)
        {
            player.Heal(amount);
            yield return new WaitForSeconds(applyEveryNSeconds);
            appliedTimes++;
        }
        player.glasses.color = new Color(90.0f / 255.0f, 253.0f / 255.0f, 255.0f / 255.0f);
        Destroy(GetComponent<Heal>());
    }
}