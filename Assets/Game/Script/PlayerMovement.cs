using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public bool isGrounded = false;

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveSpeed * Time.fixedDeltaTime, rb.linearVelocity.y);
    }

    void OnJump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }
}

