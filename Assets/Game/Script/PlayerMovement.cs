using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Mouvement")]
    [SerializeField] private float vitesseDeplacement = 5f;
    [SerializeField] private float forceJump = 12f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.1f, 0.05f);
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private float mouvementHorizontal;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        mouvementHorizontal = context.ReadValue<Vector2>().x;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.AddForce(Vector2.up * forceJump, ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapBox(
            groundCheck.position,
            groundCheckSize,
            0f,
            groundLayer
        );
        
        rb.linearVelocity = new Vector2(mouvementHorizontal * vitesseDeplacement, rb.linearVelocity.y);
    }
}

