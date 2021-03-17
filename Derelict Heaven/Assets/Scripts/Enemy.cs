using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float speed;
    public bool faceLeft = true;
    public bool isStationary;
    public int score;
    private bool isInTrigger = false;

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
        EnemyUpdate();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (GameManager.S.gameState != GameManager.GameState.playing) return;
        if (!isStationary)
        {
            float horizontalMove = speed * Time.fixedDeltaTime;

            if (faceLeft) { horizontalMove *= -1.0f; }

            controller.Move(horizontalMove, false, false);
        }
        EnemyPhysicsUpdate();
    }

    public abstract void ResetState();
    protected abstract void EnemyStart();
    protected abstract void EnemyUpdate();
    protected abstract void EnemyPhysicsUpdate();
    protected virtual void EnemyTriggerEvent(Collider2D collision)
    {
        if (!isInTrigger && (collision.gameObject.tag == "TurnAround"))
        {
            faceLeft = !faceLeft;
            isInTrigger = true;
        }
    }
    protected virtual void EnemyCollisionEnterEvent(Collision2D collision)
    {
        if (collision.gameObject.tag == "BreakableWall")
        {
            faceLeft = !faceLeft;
            isInTrigger = true;
        }
    }

    protected virtual void EnemyTriggerExitEvent(Collider2D collision)
    {
        isInTrigger = false;
    }

    protected IEnumerator FreezeForSeconds(float seconds)
    {
        isStationary = true;
        yield return new WaitForSeconds(seconds);
        isStationary = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyTriggerEvent(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        EnemyTriggerExitEvent(collision);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyCollisionEnterEvent(collision);
    }
}
