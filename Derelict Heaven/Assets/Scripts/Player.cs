using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        { GameManager.Form.archer, "Press LMB to shoot arrows!" }
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

    private bool loading = true;

    private float originalSpeed;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        possessor = GetComponent<BoxCollider2D>();
        originalSpeed = controller.speed;
        originalSprite = renderer.sprite;
        originalRadius = GetComponent<CircleCollider2D>().radius;
        rb = GetComponent<Rigidbody2D>();
        PopulateFunctions();
    }

    // Populate the mapping of enemy forms to its specific actions
    private void PopulateFunctions()
    {
        enemyFunctions = new Dictionary<GameManager.Form, Action>();
        enemyFunctions.Add(GameManager.Form.original, () => {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                possessor.enabled = true;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                possessor.enabled = false;
            }
        });
        enemyFunctions.Add(GameManager.Form.charger, () => {
            if (Input.GetKey("c"))
            {
                controller.speed = originalSpeed * 2;
            }
            else if (Input.GetKeyUp("c"))
            {
                controller.speed = originalSpeed;
            }
        });
        enemyFunctions.Add(GameManager.Form.archer, () => {
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
        });
        loading = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (loading) return;
        enemyFunctions[myForm].Invoke();
        if (myForm != GameManager.Form.original && Input.GetKeyDown(KeyCode.LeftShift))
        {
            changeValues(originalSprite, originalRadius, GameManager.Form.original);
            returnEnemy();
            possessing = null;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && possessor.enabled)
        {
            Debug.Log("possessing");
            Enemy enemyScript = collision.gameObject.GetComponent<Enemy>();
            possessor.enabled = false;
            possessing = collision.gameObject;
            collision.gameObject.SetActive(false);
            GameManager.Form form = enemyScript.GetForm();

            changeValues(collision.gameObject.GetComponent<SpriteRenderer>().sprite,
                         collision.gameObject.GetComponent<CircleCollider2D>().radius,
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
    private void changeValues(Sprite s, float rad, GameManager.Form f)
    {
        renderer.sprite = s;
        GetComponent<CircleCollider2D>().radius = rad;
        myForm = f;
        UIManager.S.ShowPopUpForSeconds(formCommands[f], 5);
    }

    private void returnEnemy()
    {
        GameObject newEnemy = Instantiate(GameManager.S.Charger);
        newEnemy.transform.position = possessing.GetComponent<Enemy>().spawn;
        Destroy(possessing);
    }
}
