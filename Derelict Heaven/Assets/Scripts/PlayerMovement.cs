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

    // Start is called before the first frame update
    void Start()
    {
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
            GetComponent<SpriteRenderer>().flipX = false;
        }
        if (horizontalMove < 0.0f)
        {
            dir = true;
            GetComponent<SpriteRenderer>().flipX = true;
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
