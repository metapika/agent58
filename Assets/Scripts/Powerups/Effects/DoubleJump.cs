﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : MonoBehaviour
{
    private PlayerController player;
    public float duration = 5;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        player.glasses.color = new Color(87.0f / 255.0f, 207.0f / 255.0f, 144.0f / 255.0f);
        StartCoroutine(PowerupCountdownRoutine());
    }

    void Update()
    {
        //Double-Jump
        if (Input.GetKeyDown(KeyCode.Space) && player.canDJump && !player.isGrounded)
        {
            player.canDJump = false;
            player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpForce);
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(duration);
            player.glasses.color = new Color(90.0f / 255.0f, 253.0f / 255.0f, 255.0f / 255.0f);
            Destroy(GetComponent<DoubleJump>());
        }
    }
}
