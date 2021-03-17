using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : Enemy
{
    public float range;
    public float aggroTime;
    public LayerMask chargeTrigger;

    private float originalSpeed;
    private bool lockedOnPlayer = false;

    protected override void EnemyStart()
    {
        originalSpeed = speed;
        Debug.Log("original speed: " + originalSpeed);
    }
    protected override void EnemyUpdate()
    {
        // Nothing yet
    }

    protected override void EnemyPhysicsUpdate()
    {
        if (!isStationary && !lockedOnPlayer)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, (faceLeft ? Vector2.left : Vector2.right) * range, 10, chargeTrigger);
            if (hit.collider != null && hit.collider.gameObject.tag == "Player")
            {
                lockedOnPlayer = true;
                speed *= 2;
                StartCoroutine(FreezeForSeconds(1));
                StartCoroutine(AggroTime(aggroTime));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ResetState();
    }
    protected override void EnemyCollisionEnterEvent(Collision2D collision)
    {
        if (collision.gameObject.tag == "BreakableWall" && lockedOnPlayer)
        {
            Vector2 pushBackForce = new Vector2(faceLeft ? 3 : -3, 3);
            gameObject.GetComponent<Rigidbody2D>().AddForce(pushBackForce, ForceMode2D.Impulse);
            Destroy(collision.gameObject);
            // TODO: Stun state effects (stay stunned forever or for a certain time?)
            isStationary = true;
        }
        else
        {
            base.EnemyCollisionEnterEvent(collision);
        }
    }
    private IEnumerator AggroTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ResetState();
    }

    public override void ResetState()
    {
        lockedOnPlayer = false;
        speed = originalSpeed;
    }
}
