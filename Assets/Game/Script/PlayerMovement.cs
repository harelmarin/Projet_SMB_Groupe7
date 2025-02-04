using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Mouvement")]
    [SerializeField] private float vitesseDeplacement = 5f;
    [SerializeField] private float sprintSpeed = 8f;  // Vitesse de sprint
    [SerializeField] private float forceJump = 12f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private LayerMask groundLayer;

    [Header("Wall Slide")]
    [SerializeField] private Transform wallCheckRight;
    [SerializeField] private Transform wallCheckLeft;
    [SerializeField] private float wallSlideSpeed = 2f;
    [SerializeField] private Vector2 wallCheckSize = new Vector2(0.1f, 0.8f);

    [Header("Wall Jump")]
    [SerializeField] private float wallJumpForceX = 15f;
    [SerializeField] private float wallJumpForceY = 12f;
    private float wallJumpCooldown = 0.2f; 
    private float lastWallJumpTime;

    [Header("Respawn")]
    [SerializeField] private Vector3 respawnPoint;
    private bool isDead;

    private Rigidbody2D rb;
    private float horizontalMovement;
    private bool isGrounded;
    private bool isWallSliding;
    private bool isTouchingWall;
    private int wallDirection;
    private bool isSprinting;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Sauvegarde la position initiale comme point de respawn
        respawnPoint = transform.position;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isTouchingWall && !isGrounded && Time.time > lastWallJumpTime + wallJumpCooldown)
            {
                float jumpDirectionX = -wallDirection;
                Vector2 wallJumpForce = new Vector2(jumpDirectionX * wallJumpForceX, wallJumpForceY);
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(wallJumpForce, ForceMode2D.Impulse);
                horizontalMovement = jumpDirectionX;
                lastWallJumpTime = Time.time;
                isWallSliding = false;
            }
            else if (isGrounded && !isTouchingWall)
            {
                rb.AddForce(Vector2.up * forceJump, ForceMode2D.Impulse);
            }
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.ReadValueAsButton();
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);

        bool touchingRightWall = Physics2D.OverlapBox(wallCheckRight.position, wallCheckSize, 0f, groundLayer);
        bool touchingLeftWall = Physics2D.OverlapBox(wallCheckLeft.position, wallCheckSize, 0f, groundLayer);
        isTouchingWall = touchingRightWall || touchingLeftWall;
        wallDirection = touchingRightWall ? 1 : (touchingLeftWall ? -1 : 0);

        if (isTouchingWall && !isGrounded && rb.linearVelocity.y < 0 && Time.time > lastWallJumpTime + wallJumpCooldown)
        {
            isWallSliding = true;
            float verticalVelocity = rb.linearVelocity.y;
            if (verticalVelocity < 0)
            {
                verticalVelocity = -wallSlideSpeed;
            }
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, verticalVelocity);
        }
        else
        {
            isWallSliding = false;
        }

        float currentSpeed = isSprinting ? sprintSpeed : vitesseDeplacement;
        rb.linearVelocity = new Vector2(horizontalMovement * currentSpeed, rb.linearVelocity.y);
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }

        if (wallCheckRight != null && wallCheckLeft != null)
        {
            Gizmos.color = isTouchingWall ? Color.blue : Color.yellow;
            Gizmos.DrawWireCube(wallCheckRight.position, wallCheckSize);
            Gizmos.DrawWireCube(wallCheckLeft.position, wallCheckSize);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Vérifie si l'objet touché est sur le layer Danger
        if (collision.gameObject.layer == LayerMask.NameToLayer("Danger"))
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        transform.position = respawnPoint;
        isDead = false;
    }
}

