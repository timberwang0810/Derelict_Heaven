using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
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
    private GameObject possessing;

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
    public AnimatorController angelAnim;
    private Animator animator;

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
                    else if (Input.GetKeyUp("c"))
                    {
                        controller.speed = originalSpeed;
                        animator.SetBool("charge", false);
                    }
                }
            },
            {
                Form.archer,
                () =>
                {
                    coolDownTimer += Time.deltaTime;
                    if (Input.GetKeyDown(KeyCode.Mouse0) && coolDownTimer >= shotCoolDown)
                    {
                        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        Vector2 dir = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);
                        dir.Normalize();
                        if (arrowPrefab)
                        {
                            Vector2 instantiateLocation = new Vector2(transform.position.x + dir.x, transform.position.y + dir.y);
                            GameObject arrowObject = Instantiate(arrowPrefab, instantiateLocation, Quaternion.identity);
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
        if (loading || GameManager.S.gameState != GameManager.GameState.playing) return;
        enemyFunctions[myForm].Invoke();
        if (myForm != Form.original && Input.GetKeyDown(KeyCode.LeftShift))
        {
            returnEnemy();
            changeValues(originalSprite, new Vector2(0, 0), new Vector2(0, 0), Form.original);  
            possessing = null;
        }
    }

    public Form GetForm()
    {
        return myForm;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && possessor.enabled)
        {
            Enemy enemyScript = collision.gameObject.GetComponent<Enemy>();
            possessor.enabled = false;
            possessing = collision.gameObject;
            collision.gameObject.SetActive(false);
            Form form = enemyScript.GetForm();

            changeValues(collision.gameObject.GetComponent<SpriteRenderer>().sprite,
                         collision.gameObject.GetComponent<CapsuleCollider2D>().size,
                         collision.gameObject.GetComponent<CapsuleCollider2D>().offset,
                         form);

            // Pass enemy variables to the player
            if (form == Form.archer)
            {
                Archer archerScript = collision.gameObject.GetComponent<Archer>();
                arrowPrefab = archerScript.GetArrowObject();
                shotSpeed = archerScript.shotSpeed;
                shotCoolDown = archerScript.shotCoolDown;
                coolDownTimer = shotCoolDown;
            }

            Vector3 newPos = collision.transform.position;
            transform.position = newPos;
        }
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
            GetComponent<CircleCollider2D>().enabled = true;
            enemyCol.enabled = false;
            animator.runtimeAnimatorController = angelAnim;
        } else
        {
            if (f == Form.charger)
            {
                animator.runtimeAnimatorController = chargerAnim;
            }
            GetComponent<CircleCollider2D>().enabled = false;
            enemyCol.enabled = true;
            enemyCol.size = size;
            enemyCol.offset = offset;
        }
        myForm = f;
        UIManager.S.ShowPopUpForSeconds(formCommands[f], 5);
    }

    private void returnEnemy()
    {
        GameObject newEnemy;
        if (myForm == Form.charger)
        {
            newEnemy = Instantiate(GameManager.S.Charger);
        } else if (myForm == Form.archer)
        {
            newEnemy = Instantiate(GameManager.S.Archer);
        } else
        {
            throw new Exception("no enemy of this type");
        }
        
        newEnemy.transform.position = possessing.GetComponent<Enemy>().spawn;
        Destroy(possessing);
    }
}
