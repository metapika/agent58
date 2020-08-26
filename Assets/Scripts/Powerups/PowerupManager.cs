using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public GameObject doubleJump;
    public GameObject knockBack;
    public GameObject speed;
    public GameObject heal;
    public GameObject bounce;
    private GameManager gameManager;

    private Coroutine DJcr = null;
    private Coroutine KBcr = null;
    private Coroutine Scr = null;
    private Coroutine Hcr = null;
    private Coroutine Bcr = null;

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
        if(GameObject.Find("BouncePowerup"))
        {
            bounce = GameObject.Find("BouncePowerup");
        } else {bounce = null;}
    }
    
    void Update()
    {
        if (doubleJump != null && doubleJump.GetComponent<SpriteRenderer>().enabled == false)
        {
            if (DJcr != null)
            {
                StopCoroutine(DJcr);
            }
            StartCoroutine(DoubleJumpRespawn(2));
        }
        if (knockBack != null && knockBack.GetComponent<SpriteRenderer>().enabled == false)
        {
            if (KBcr != null)
            {
                StopCoroutine(KBcr);
            }
            StartCoroutine(KnockBackRespawn(2));
        }
        if (speed != null && speed.GetComponent<SpriteRenderer>().enabled == false)
        {
            if (Scr != null)
            {
                StopCoroutine(Scr);
            }
            StartCoroutine(SpeedRespawn(15));
        }
        if (heal != null && heal.GetComponent<SpriteRenderer>().enabled == false)
        {
            if (Hcr != null)
            {
                StopCoroutine(Hcr);
            }
            StartCoroutine(HealRespawn(10));
        }
        if (bounce != null && bounce.GetComponent<SpriteRenderer>().enabled == false)
        {
            if (Bcr != null)
            {
                StopCoroutine(Bcr);
            }
            StartCoroutine(BounceUpRespawn(2));
        }
    }
    
    IEnumerator DoubleJumpRespawn(int duration)
    {
        SpriteRenderer spriteD = doubleJump.GetComponent<SpriteRenderer>();
        yield return new WaitForSeconds(duration);
        
        if (gameManager.isGameActive)
        {
            spriteD.enabled = true;
        }
    }
    IEnumerator KnockBackRespawn(int duration)
    {
        yield return new WaitForSeconds(duration);
        
        if (gameManager.isGameActive)
        {
            knockBack.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    IEnumerator SpeedRespawn(int duration)
    {
        yield return new WaitForSeconds(duration);
        
        if (gameManager.isGameActive)
        {
            speed.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    IEnumerator HealRespawn(int duration)
    {
        yield return new WaitForSeconds(duration);
        
        if (gameManager.isGameActive)
        {
            heal.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    IEnumerator BounceUpRespawn(int duration)
    {
        yield return new WaitForSeconds(duration);
        
        if (gameManager.isGameActive)
        {
            bounce.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}