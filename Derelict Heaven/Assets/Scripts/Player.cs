using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public Form myForm = Form.original;

    public PlayerMovement controller;

    private Dictionary<Form, Action> enemyFunctions;

    private BoxCollider2D possessor;
    public float embodyCooldown;
    public float originalFormCooldown; //can't immediately embody after turning back
    private bool changeBack = false;
    private bool enableEmbody = true;

    // Archer enemy variables
    private Rigidbody2D rb;
    private GameObject arrowPrefab;
    private float shotSpeed;
    private CapsuleCollider2D enemyCol;

    private bool loading = true;

    private float originalSpeed;
    
    public AnimatorOverrideController chargerAnim;
    public AnimatorOverrideController archerAnim;
    public AnimatorOverrideController bishopAnim;
    public RuntimeAnimatorController angelAnim;
    private Animator animator;

    public GameObject returnQueue;

    private ParticleSystem particles; 

    private GameObject pressurePlate = null;
    public GameObject embodying = null;

    // Start is called before the first frame update
    void Start()
    {
        possessor = GetComponent<BoxCollider2D>();
        originalSpeed = controller.speed;
        rb = GetComponent<Rigidbody2D>();
        enemyCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        particles = GetComponent<ParticleSystem>();
        particles.Stop();
        PopulateFunctions();
    }

    // Populate the mapping of enemy forms to its specific actions
    private void PopulateFunctions()
    {
        enemyFunctions = new Dictionary<Form, Action>
        {
            {
                Form.original,
                () =>
                {
                    if (Input.GetKey(KeyCode.LeftShift) && enableEmbody)
                    {
                        possessor.enabled = true;
                        if (!particles.isPlaying) particles.Play();
                        if (myForm == Form.original)
                        {  }
                    }
                    if (Input.GetKeyUp(KeyCode.LeftShift))
                    {
                        particles.Stop();
                        possessor.enabled = false;
                    }
                }
            },
            {
                Form.charger,
                () =>
                {
                    
                    if (Input.GetKey("c"))
                    {
                        controller.speed = originalSpeed * 2;
                        if (rb.velocity.magnitude > 0) animator.SetBool("charge", true);
                    }

                    if (Input.GetKeyDown("c"))
                    {
                        SoundManager.S.OnChargerRunSound();
                    }
                    else if (Input.GetKeyUp("c"))
                    {
                        controller.speed = originalSpeed;
                        SoundManager.S.OnStopMovementSound();
                        if (controller.IsPlayerMoving()) SoundManager.S.OnChargerWalkSound();
                        animator.SetBool("charge", false);
                    }
                }
            },
            {
                Form.archer,
                () =>
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        animator.SetTrigger("shoot");
                        SoundManager.S.OnArrowFire();
                        Vector3 rawMousePosition = Input.mousePosition;
                        rawMousePosition.z = 10;
                        Vector2 dir = Camera.main.ScreenToWorldPoint(rawMousePosition) - transform.position;
                        dir.Normalize();
              
                        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                        if (arrowPrefab)
                        {
                            Vector2 instantiateLocation = new Vector2(transform.position.x + dir.x, transform.position.y + dir.y);
                            GameObject arrowObject = Instantiate(arrowPrefab, instantiateLocation, Quaternion.Euler(new Vector3(0f, 0f, angle)));
                            arrowObject.GetComponent<SpriteRenderer>().flipX = true;
                            arrowObject.layer = 17;
                            arrowObject.GetComponent<Rigidbody2D>().velocity = dir * shotSpeed;
                        }
                    }
                }
            },
            {
                Form.pressurizer,
                () =>
                {
                    if (Input.GetKeyDown("c") && !animator.GetBool("activate"))
                    {
                        if (pressurePlate != null)
                        {
                            rb.velocity = Vector3.zero;
                            rb.angularVelocity = 0;
                            transform.position = pressurePlate.transform.position;
                            SoundManager.S.OnPressurizerUseSound();
                            animator.SetBool("activate", true);
                            pressurePlate.GetComponent<PressurePlate>().Activate();

                        }
                    }
                }
            }
        };
        loading = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (loading || GameManager.S.gameState != GameManager.GameState.playing || GameManager.S.IsInvincible()) return;
        
        enemyFunctions[myForm].Invoke();
        if (myForm != Form.original && Input.GetKeyDown(KeyCode.LeftShift) && changeBack)
        {
            controller.speed = originalSpeed;
            if (myForm == Form.pressurizer)
            {
                if (animator.GetBool("activate"))
                {
                    returnQueue.GetComponent<ReturnQueueManager>().deleteEnemy();
                    GameManager.S.SpawnUsedBishop(transform.position);
                } else
                {
                    returnQueue.GetComponent<ReturnQueueManager>().returnEnemy();
                }
            } else
            {
                returnQueue.GetComponent<ReturnQueueManager>().returnEnemy();
            }
            animator.SetBool("activate", false);
            changeValues(new Vector2(0, 0), new Vector2(0, 0), Form.original);
            SoundManager.S.OnStopMovementSound();
            SoundManager.S.OnUnConsumeSound();
            StartCoroutine(originalFormCount());
        }
    }

    public Form GetForm()
    {
        return myForm;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PressurePlate")
        {
            pressurePlate = null;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PressurePlate")
        {
            pressurePlate = collision.gameObject;
        }

        if ((collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyAttack") && possessor.enabled)
        {
            embodying = collision.gameObject;
            animator.SetBool("embody", true);
        }
    }

    public void stopParticles()
    {
        particles.Stop();
    }

    public void embodyEnemy()
    {
        if (embodying == null) return;

        Enemy enemyScript = embodying.GetComponent<Enemy>();
        possessor.enabled = false;

        Form form = enemyScript.GetForm();

        embodying.SetActive(false);

        returnQueue.GetComponent<ReturnQueueManager>().AddToQueue(form, embodying.GetComponent<Enemy>().spawn);

        // Last check to see if player has died
        if (GameManager.S.gameState != GameManager.GameState.playing) return;

        changeValues(embodying.GetComponent<CapsuleCollider2D>().size,
                     embodying.GetComponent<CapsuleCollider2D>().offset,
                     form);

        // Pass enemy variables to the player
        if (form == Form.archer)
        {
            rb.velocity = new Vector2(0, 0);
            rb.constraints |= RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            Archer archerScript = embodying.GetComponent<Archer>();
            arrowPrefab = archerScript.GetArrowObject();
            shotSpeed = archerScript.shotSpeed;
            UIManager.S.ShowAimingCursor();
        }

        Vector3 newPos = embodying.transform.position;
        transform.position = newPos;
        StartCoroutine(embodyCooldownCount());
        Destroy(embodying);
        embodying = null;
        animator.SetBool("embody", false);
        particles.Stop();
    }

    IEnumerator embodyCooldownCount()
    {
        changeBack = false;
        yield return new WaitForSeconds(embodyCooldown);
        changeBack = true;
    }

    IEnumerator originalFormCount()
    {
        enableEmbody = false;
        yield return new WaitForSeconds(originalFormCooldown);
        enableEmbody = true;
        UIManager.S.HideAimingCursor();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "BreakableWall")
        {
            if (myForm == Form.charger && controller.speed == originalSpeed * 2)
            {
                // Wall break SFX
                collision.gameObject.GetComponent<Animator>().SetTrigger("open");
                SoundManager.S.OnWallBreak();
            }
        }
    }

    // Change form
    private void changeValues(Vector2 size, Vector2 offset, Form f)
    {
        if (f == Form.original)
        {
            rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX & ~RigidbodyConstraints2D.FreezePositionY;
            GetComponent<CircleCollider2D>().enabled = true;
            enemyCol.enabled = false;
            animator.runtimeAnimatorController = angelAnim;
            gameObject.transform.GetChild(2).gameObject.SetActive(false);
        } else
        {
            gameObject.transform.GetChild(2).gameObject.SetActive(true);
            if (f == Form.charger)
            {
                animator.runtimeAnimatorController = chargerAnim;
            } else if (f == Form.archer)
            {
                animator.runtimeAnimatorController = archerAnim;
                animator.SetBool("walking", false);
            } else if (f == Form.pressurizer)
            {
                animator.runtimeAnimatorController = bishopAnim;
            }
            GetComponent<CircleCollider2D>().enabled = false;
            enemyCol.enabled = true;
            enemyCol.size = size;
            enemyCol.offset = offset;
            SoundManager.S.OnConsumeSound();


        }
        myForm = f;
        if (GameManager.S.formCommands[f] == null) UIManager.S.ShowPopUpForSeconds("Return to Normal!", 5);
        else UIManager.S.ShowPopUpImageForSeconds(GameManager.S.formCommands[f], 5);
    }
}
