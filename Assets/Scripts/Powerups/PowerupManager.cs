using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public GameObject doubleJump;
    public GameObject knockBack;
    public GameObject speed;
    public GameObject heal;
    private GameManager gameManager;

    void Start() 
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        if(GameObject.Find("DoubleJumpPowerup"))
        {
            doubleJump = GameObject.Find("DoubleJumpPowerup");
        } else {doubleJump = null;}

        if(GameObject.Find("KnockbackPowerup"))
        {
            knockBack= GameObject.Find("KnockbackPowerup");
        } else {knockBack = null;}

        if(GameObject.Find("SpeedPowerup"))
        {
            speed = GameObject.Find("SpeedPowerup");
        } else {speed = null;}
        if(GameObject.Find("HealPowerup"))
        {
            heal = GameObject.Find("HealPowerup");
        } else {heal = null;}
    }
    
    void Update()
    {
        if (doubleJump != null && doubleJump.activeSelf == false)
        {
            StartCoroutine(DoubleJumpRespawn(8));
        }
        if (knockBack != null && knockBack.activeSelf == false)
        {
            StartCoroutine(KnockBackRespawn(30));
        }
        if (speed != null && speed.activeSelf == false)
        {
            StartCoroutine(SpeedRespawn(15));
        }
        if (heal != null && heal.activeSelf == false )
        {
            StartCoroutine(HealRespawn(10));
        }
    }
    
    IEnumerator DoubleJumpRespawn(int duration)
    {
        yield return new WaitForSeconds(duration);
        
        if (gameManager.isGameActive)
        {
            doubleJump.SetActive(true);
        }
    }
    IEnumerator KnockBackRespawn(int duration)
    {
        yield return new WaitForSeconds(duration);
        
        if (gameManager.isGameActive)
        {
            knockBack.SetActive(true);
        }
    }
    IEnumerator SpeedRespawn(int duration)
    {
        yield return new WaitForSeconds(duration);
        
        if (gameManager.isGameActive)
        {
            speed.SetActive(true);
        }
    }
    IEnumerator HealRespawn(int duration)
    {
        yield return new WaitForSeconds(duration);
        
        if (gameManager.isGameActive)
        {
            heal.SetActive(true);
        }
    }
}