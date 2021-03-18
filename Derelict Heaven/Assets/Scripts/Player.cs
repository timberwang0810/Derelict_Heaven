using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    // TODO: Put the form enum in GameManager so enemy can also access
    public GameManager.Form myForm = GameManager.Form.original;
    public PlayerMovement controller;
    private Dictionary<GameManager.Form, Action> enemyFunctions;

    private SpriteRenderer renderer;
    private Sprite originalSprite;
    private float originalRadius;

    private BoxCollider2D possessor;
    private GameObject possessing;

    private Rigidbody2D rb;
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
            possessor.enabled = false;
            possessing = collision.gameObject;
            collision.gameObject.SetActive(false);

            changeValues(collision.gameObject.GetComponent<SpriteRenderer>().sprite,
                         collision.gameObject.GetComponent<CircleCollider2D>().radius,
                         GameManager.Form.charger);

            Vector3 newPos = collision.transform.position;
            transform.position = newPos;
            StartCoroutine(UIManager.S.ShowPopUpForSeconds("Press 'c' to activate enemy ability!", 5));
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
    }

    private void returnEnemy()
    {
        GameObject newEnemy = Instantiate(GameManager.S.Charger);
        newEnemy.transform.position = possessing.GetComponent<Enemy>().spawn;
        Destroy(possessing);
    }
}
