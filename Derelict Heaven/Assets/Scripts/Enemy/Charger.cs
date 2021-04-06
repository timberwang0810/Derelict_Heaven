using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : Enemy
{
    public float range;
    public float aggroTime;
    public LayerMask aimImpedeLayers;

    private float originalSpeed;
    private bool lockedOnPlayer = false;
    private Animator animator;
    private bool canAggro = true;
    private float deAggroDelayTime = 2.0f;

    public AudioSource walkAudio;
    public AudioSource runAudio;

    void Start()
    {
        walkAudio.Stop();
        runAudio.Stop();
        base.Start();
        animator = GetComponent<Animator>();
    }

    public override Form GetForm()
    {
        return Form.charger;
    }

    protected override void EnemyStart()
    {
        originalSpeed = speed;
        walkAudio.Play();
    }
    protected override void EnemyUpdate()
    {
        // Nothing yet
    }

    protected override void EnemyPhysicsUpdate()
    {
        if (!isStationary && !lockedOnPlayer && canAggro)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, (faceLeft ? Vector2.left : Vector2.right) * range, 10, aimImpedeLayers);

            if (hit.collider != null && hit.collider.gameObject.tag == "Player")
            {
                if (hit.collider.gameObject.GetComponent<Player>().GetForm() == Form.original)
                {
                    animator.SetTrigger("chargeup");
                    lockedOnPlayer = true;
                    speed *= 2;
                    walkAudio.Stop();
                    runAudio.Play();
                    StartCoroutine(FreezeForSeconds(1));
                    StartCoroutine(AggroTime(aggroTime));
                }             
            }
        }
    }

    public void SetChargeTrue()
    {
        gameObject.tag = "EnemyAttack";
        animator.SetBool("charging", true);
    }

    protected override void EnemyCollisionEnterEvent(Collision2D collision)
    {
        if (collision.gameObject.tag == "BreakableWall" && lockedOnPlayer)
        {

            animator.SetTrigger("impact");
            gameObject.tag = "Enemy";
            animator.SetBool("charging", false);
            Vector2 pushBackForce = new Vector2(faceLeft ? 3 : -3, 3);
            gameObject.GetComponent<Rigidbody2D>().AddForce(pushBackForce, ForceMode2D.Impulse);
            Destroy(collision.gameObject);

            // Wall break SFX
            SoundManager.S.OnWallBreak();

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
        gameObject.tag = "Enemy";
        animator.SetBool("charging", false);
        runAudio.Stop();
        walkAudio.Play();
        StartCoroutine(DeAggroDelay());
    }

    private IEnumerator DeAggroDelay()
    {
        canAggro = false;
        yield return new WaitForSeconds(deAggroDelayTime);
        canAggro = true;
    }
}
