using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using System;

public class Player : MonoBehaviour
{
    public GameManager.Form myForm = GameManager.Form.original;
    public PlayerMovement controller;
    private Dictionary<GameManager.Form, Action> enemyFunctions;
    private Dictionary<GameManager.Form, string> formCommands = new Dictionary<GameManager.Form, string>()
    {
        { GameManager.Form.original, "Returned to normal!" },
        { GameManager.Form.charger, "Press 'c' to charge forward!" },
        { GameManager.Form.archer, "Press LMB to shoot arrows!" },
        { GameManager.Form.pressurizer, "" }
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
        enemyFunctions = new Dictionary<GameManager.Form, Action>
        {
            {
                GameManager.Form.original,
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
                GameManager.Form.charger,
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
                GameManager.Form.archer,
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
                GameManager.Form.pressurizer,
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
        if (loading) return;
        enemyFunctions[myForm].Invoke();
        if (myForm != GameManager.Form.original && Input.GetKeyDown(KeyCode.LeftShift))
        {
            returnEnemy();
            changeValues(originalSprite, new Vector2(0, 0), new Vector2(0, 0), GameManager.Form.original);  
            possessing = null;
        }
    }

    public GameManager.Form GetForm()
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
            GameManager.Form form = enemyScript.GetForm();

            changeValues(collision.gameObject.GetComponent<SpriteRenderer>().sprite,
                         collision.gameObject.GetComponent<CapsuleCollider2D>().size,
                         collision.gameObject.GetComponent<CapsuleCollider2D>().offset,
                         form);

            // Pass enemy variables to the player
            if (form == GameManager.Form.archer)
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
            if (myForm == GameManager.Form.charger && controller.speed == originalSpeed * 2)
            {
                Destroy(collision.gameObject);
            }
        }
    }

    /** dont change scale, it messes with character controller
     */
    private void changeValues(Sprite s, Vector2 size, Vector2 offset, GameManager.Form f)
    {
        if (f == GameManager.Form.original)
        {
            GetComponent<CircleCollider2D>().enabled = true;
            enemyCol.enabled = false;
            animator.runtimeAnimatorController = angelAnim;
        } else
        {
            if (f == GameManager.Form.charger)
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
        if (myForm == GameManager.Form.charger)
        {
            newEnemy = Instantiate(GameManager.S.Charger);
        } else if (myForm == GameManager.Form.archer)
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
