using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    private float originalSpeed;
    public bool faceLeft = true;
    public bool isStationary;
    public float range;
    public float aggroTime;
    public int score;

    public LayerMask chargeTrigger;

    private bool isInTrigger = false;
    private bool lockedOnPlayer = false;

    private CharacterController2D controller;

    public Vector3 spawn;

    private void Start()
    {
        if (!isStationary)
        {
            controller = GetComponent<CharacterController2D>();
        }
        spawn = transform.position;
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

            if (!lockedOnPlayer)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, (faceLeft ? Vector2.left : Vector2.right) * range, 10, chargeTrigger);
                if (hit.collider != null && hit.collider.gameObject.tag == "Player")
                {
                    lockedOnPlayer = true;
                    speed *= 2;
                    gameObject.layer = 10; // Layer that ignores turn around trigger
                    StartCoroutine(FreezeForSeconds(1));
                    StartCoroutine(AggroTime(aggroTime));
                }
            }
        }
    }

    private IEnumerator FreezeForSeconds(float seconds)
    {
        isStationary = true;
        yield return new WaitForSeconds(seconds);
        isStationary = false;
    }

    private IEnumerator AggroTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        lockedOnPlayer = false;
        speed /= 2;
        gameObject.layer = 9;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isInTrigger && !lockedOnPlayer && (collision.gameObject.tag == "TurnAround" || collision.gameObject.tag == "BreakableWall"))
        {
            faceLeft = !faceLeft;
            isInTrigger = true;
        }
        //else if (collision.gameObject.tag == "Projectile" && !collision.gameObject.GetComponent<Projectile>().isHostile)
        //{
        //    SoundManager.S.MakeDestroyEnemySound();
        //    GameManager.S.UpdateScore(score);
        //    Destroy(collision.gameObject);
        //    Destroy(this.gameObject);
        //}

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isInTrigger = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "BreakableWall")
        {
            if (lockedOnPlayer)
            {
                Vector2 pushBackForce = new Vector2(faceLeft ? 3 : -3, 3);
                gameObject.GetComponent<Rigidbody2D>().AddForce(pushBackForce, ForceMode2D.Impulse);
                Destroy(collision.gameObject);
                // TODO: Stun state effects (stay stunned forever or for a certain time?)
                isStationary = true;
            }
        }
    }
}
