using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public Form myForm = Form.original;
    public PlayerMovement controller;
    private Dictionary<Form, Action> enemyFunctions;
    private Dictionary<Form, string> formCommands = new Dictionary<Form, string>()
    {
        { Form.original, "Returned to normal!" },
        { Form.charger, "Press 'c' to charge forward!" },
        { Form.archer, "Press LMB to shoot arrows!" },
        { Form.pressurizer, "" }
    };

    private SpriteRenderer renderer;
    private Sprite originalSprite;
    private float originalRadius;

    private BoxCollider2D possessor;
    public float embodyCooldown;
    private bool changeBack = false;

    // Archer enemy variables
    private Rigidbody2D rb;
    private GameObject arrowPrefab;
    private float shotSpeed;
    private float shotCoolDown;
    private float coolDownTimer;
    private CapsuleCollider2D enemyCol;

    private bool loading = true;

    private float originalSpeed;
    
    public AnimatorOverrideController chargerAnim;
    public AnimatorOverrideController archerAnim;
    public RuntimeAnimatorController angelAnim;
    private Animator animator;

    public GameObject returnQueue;

    private GameObject camera;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        possessor = GetComponent<BoxCollider2D>();
        originalSpeed = controller.speed;
        originalSprite = renderer.sprite;
        originalRadius = GetComponent<CircleCollider2D>().radius;
        rb = GetComponent<Rigidbody2D>();
        enemyCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
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
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        possessor.enabled = true;
                        if (myForm == Form.original)
                        {  }
                    }
                    if (Input.GetKeyUp(KeyCode.LeftShift))
                    {
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
                        SoundManager.S.OnStopCurrentSound();
                        if (controller.IsPlayerMoving()) SoundManager.S.OnChargerWalkSound();
                        animator.SetBool("charge", false);
                    }
                }
            },
            {
                Form.archer,
                () =>
                {
                    coolDownTimer += Time.deltaTime;
                    if (Input.GetKeyDown(KeyCode.Mouse0)) // && coolDownTimer >= shotCoolDown) commented out for testing //TODO: Uncomment
                    {
                        animator.SetTrigger("shoot");
                        SoundManager.S.OnArrowFire();
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        Plane xy = new Plane(new Vector3(0, 1, -1), new Vector3(0, 0, transform.GetChild(2).transform.position.z));
                        float distance;
                        xy.Raycast(ray, out distance);
                        Vector2 mousePos = ray.GetPoint(distance);

                        Vector2 dir = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);
                        dir.Normalize();
                        if (mousePos.x > transform.position.x) controller.ControllerMove(1);
                        else controller.ControllerMove(-1);

                        if (arrowPrefab)
                        {
                            Vector2 instantiateLocation = new Vector2(transform.position.x + dir.x, transform.position.y + dir.y);
                            GameObject arrowObject = Instantiate(arrowPrefab, instantiateLocation, Quaternion.identity);
                            arrowObject.layer = 8;
                            arrowObject.GetComponent<Rigidbody2D>().velocity = dir * shotSpeed;
                            coolDownTimer = 0;
                        }
                    }
                }
            },
            {
                Form.pressurizer,
                () =>
                {

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
            returnQueue.GetComponent<ReturnQueueManager>().returnEnemy();
            changeValues(originalSprite, new Vector2(0, 0), new Vector2(0, 0), Form.original);
            SoundManager.S.OnStopCurrentSound();
        }
    }

    public Form GetForm()
    {
        return myForm;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Key")
        {
            GameManager.S.PlayerGotKey();
            Destroy(collision.gameObject);
        }

        if ((collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyAttack") && possessor.enabled)
        {
            Enemy enemyScript = collision.gameObject.GetComponent<Enemy>();
            possessor.enabled = false;

            Debug.Log("getting form");
            Form form = enemyScript.GetForm();
            Debug.Log("form is " + form);
            collision.gameObject.SetActive(false);

            returnQueue.GetComponent<ReturnQueueManager>().AddToQueue(form, collision.gameObject.GetComponent<Enemy>().spawn);

            changeValues(collision.gameObject.GetComponent<SpriteRenderer>().sprite,
                         collision.gameObject.GetComponent<CapsuleCollider2D>().size,
                         collision.gameObject.GetComponent<CapsuleCollider2D>().offset,
                         form);


            // Pass enemy variables to the player
            if (form == Form.archer)
            {
                rb.velocity = new Vector2(0, 0);
                rb.constraints |= RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
                Archer archerScript = collision.gameObject.GetComponent<Archer>();
                arrowPrefab = archerScript.GetArrowObject();
                shotSpeed = archerScript.shotSpeed;
                shotCoolDown = archerScript.shotCoolDown;
                coolDownTimer = shotCoolDown;
            }

            Vector3 newPos = collision.transform.position;
            transform.position = newPos;
            StartCoroutine(embodyCooldownCount());
            Destroy(collision.gameObject);
        }
    }

    IEnumerator embodyCooldownCount()
    {
        changeBack = false;
        yield return new WaitForSeconds(embodyCooldown);
        changeBack = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "BreakableWall")
        {
            if (myForm == Form.charger && controller.speed == originalSpeed * 2)
            {
                Destroy(collision.gameObject);
            }
        }
    }

    /** dont change scale, it messes with character controller
     */
    private void changeValues(Sprite s, Vector2 size, Vector2 offset, Form f)
    {
        if (f == Form.original)
        {
            rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX & ~RigidbodyConstraints2D.FreezePositionY;
            GetComponent<CircleCollider2D>().enabled = true;
            enemyCol.enabled = false;
            animator.runtimeAnimatorController = angelAnim;
            SoundManager.S.OnUnConsumeSound();
        } else
        {
            if (f == Form.charger)
            {
                animator.runtimeAnimatorController = chargerAnim;
            } else if (f == Form.archer)
            {
                animator.runtimeAnimatorController = archerAnim;
                animator.SetBool("walking", false);
            }
            GetComponent<CircleCollider2D>().enabled = false;
            enemyCol.enabled = true;
            enemyCol.size = size;
            enemyCol.offset = offset;
            SoundManager.S.OnConsumeSound();


        }
        myForm = f;
        UIManager.S.ShowPopUpForSeconds(formCommands[f], 5);
    }
}
