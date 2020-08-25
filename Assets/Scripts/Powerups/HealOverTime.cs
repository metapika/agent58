using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOverTime : MonoBehaviour
{
    private PlayerController player;
    public int amount;
    public int seconds;
    public int delay;
    public int applyHealNTimes;
    public int applyEveryNSeconds;
    
    private int appliedTimes = 0;

    public void Start() 
    {
        player = gameObject.GetComponent<PlayerController>();
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
    }
}