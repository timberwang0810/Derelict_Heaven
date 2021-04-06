using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float speed;
    public bool faceLeft = true;
    public bool isStationary;


    protected CharacterController2D controller;

    public Vector3 spawn;

    protected void Start()
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

        if (!isStationary && !(this is Archer))
        {
            float horizontalMove = speed * Time.fixedDeltaTime;
           
            if (faceLeft) { horizontalMove *= -1.0f; }
            //Debug.Log(horizontalMove + " " + faceLeft);
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
            TurnAround();
        }
        else if (collision.gameObject.tag == "Player" && gameObject.tag == "EnemyAttack")
        {
            if (collision.gameObject.GetComponent<Player>().GetForm() != Form.original)
            {
                TurnAround();
            }
            else
            {
                Vector2 dir = collision.gameObject.transform.position - gameObject.transform.position;
                dir.Normalize();
                GameManager.S.OnLivesLost(dir);
            }
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
