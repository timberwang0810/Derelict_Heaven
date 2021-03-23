using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public float speed;
    private float horizontalMove = 0.0f;
    private bool jump = false;
    private bool isMoving = false;

    bool dir = false;
    private Rigidbody2D rb;
    private Animator animator;
    private GameObject floater;
    private Player playerScript;

    public AudioSource ChargerWalking;
    public AudioSource ChargerRunning;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerScript = GetComponent<Player>();
        floater = gameObject.transform.GetChild(3).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.S.gameState != GameManager.GameState.playing || GameManager.S.IsInvincible()) return;
        horizontalMove = Input.GetAxisRaw("Horizontal") * speed;
        if (GetComponent<Player>().myForm == Form.archer) horizontalMove = 0;
        if (horizontalMove != 0) animator.SetBool("walking", true);
        if (horizontalMove !=0 && GetComponent<Player>().myForm == Form.charger) { }
        else animator.SetBool("walking", false);

        floater.SetActive(controller.CheckGrounded());

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            SoundManager.S.OnJumpSound(playerScript.myForm);
            
        }

        if (horizontalMove > 0.0f)
        {
            dir = false;
        }
        if (horizontalMove < 0.0f)
        {
            dir = true;
        }

        if (Input.GetKey("space") && GetComponent<Player>().myForm == Form.original)
        {
            rb.gravityScale = 0.3f;
        }
        if (Input.GetKeyUp("space") || rb.velocity.y > 0)
        {
            rb.gravityScale = 2;
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.S.gameState != GameManager.GameState.playing || GameManager.S.IsInvincible()) return;
        if (GetComponent<Player>().myForm == Form.archer) return;
        float move = horizontalMove * Time.fixedDeltaTime;
        if (move != 0 && !isMoving)
        {
            SoundManager.S.OnWalkSound(playerScript.myForm);
            isMoving = true;
        }
        else if (move == 0 && isMoving)
        {
            SoundManager.S.OnStopWalkSound(playerScript.myForm);
            isMoving = false;
        }
        controller.Move(move, false, jump);

        if (jump)
        {
            Vector3 curPos = transform.position;
            curPos.y += 0.1f;
            transform.position = curPos;
        }
        jump = false;
    }

    public void ControllerMove(int move)
    {
        controller.Move(move, false, false);
    }
}
