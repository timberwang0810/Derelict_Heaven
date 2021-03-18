using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public float speed;
    private float horizontalMove = 0.0f;
    private bool jump = false;

    bool dir = false;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * speed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (horizontalMove > 0.0f)
        {
            dir = false;
        }
        if (horizontalMove < 0.0f)
        {
            dir = true;
        }

        if (Input.GetKey("space") && GetComponent<Player>().myForm == GameManager.Form.original)
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
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        if (jump)
        {
            Vector3 curPos = transform.position;
            curPos.y += 0.1f;
            transform.position = curPos;
        }
        jump = false;
    }
}
