using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Options")]
    public float speed;
    public float jumpForce;

    public float acceleration; 

    private float currentSpeed;
    private float horizontal;
    private bool isFacingRight = true;

    [Header("Components")]
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;


    void Start ()
    {
        currentSpeed = 10f;
    }
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        Flip();
        Jump();
    }

    void FixedUpdate()
    {   
        if(horizontal != 0f)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, speed, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, acceleration * Time.fixedDeltaTime);
        }
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.linearVelocity= new Vector2(rb.linearVelocity.x , jumpForce);
        }

        if(Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    void Flip()
    {
        if(isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
