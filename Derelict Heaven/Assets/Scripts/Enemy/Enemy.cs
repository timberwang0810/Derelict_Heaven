﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float speed;
    public bool faceLeft = true;
    public bool isStationary;


    private CharacterController2D controller;

    public Vector3 spawn;

    private void Start()
    {
        if (!isStationary)
        {
            controller = GetComponent<CharacterController2D>();
        }
        spawn = transform.position;
        EnemyStart();
    }

    private void Update()
    {
        if (GameManager.S.gameState != GameManager.GameState.playing) return;
        EnemyUpdate();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.S.gameState != GameManager.GameState.playing) return;
        if (controller == null)
        {
            controller = GetComponent<CharacterController2D>();
        }
        //if (GameManager.S.gameState != GameManager.GameState.playing) return;
        if (!isStationary)
        {
            float horizontalMove = speed * Time.fixedDeltaTime;
           
            if (faceLeft) { horizontalMove *= -1.0f; }
            controller.Move(horizontalMove, false, false);
        }
        EnemyPhysicsUpdate();
    }

    public void TurnAround()
    {
        faceLeft = !faceLeft;
    }

    public abstract void ResetState();
    public abstract Form GetForm();
    protected abstract void EnemyStart();
    protected abstract void EnemyUpdate();
    protected abstract void EnemyPhysicsUpdate();

    protected virtual void EnemyCollisionEnterEvent(Collision2D collision)
    {
        if (collision.gameObject.tag == "BreakableWall")
        {
            faceLeft = !faceLeft;
        }
        else if (collision.gameObject.tag == "Player")
        {
            GameManager.S.OnLivesLost();
        }
    }

    protected IEnumerator FreezeForSeconds(float seconds)
    {
        isStationary = true;
        yield return new WaitForSeconds(seconds);
        isStationary = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyCollisionEnterEvent(collision);
    }
}