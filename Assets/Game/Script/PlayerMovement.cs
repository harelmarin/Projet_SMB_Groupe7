using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Mouvement")]
    [SerializeField] private float vitesseDeplacement = 5f;
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
    private float wallJumpCooldown = 0.2f; // Temps minimum entre les wall jumps
    private float lastWallJumpTime;

    private Rigidbody2D rb;
    private float mouvementHorizontal;
    private bool isGrounded;
    private bool isWallSliding;
    private bool isTouchingWall;
    private int wallDirection;

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
        if (context.performed)
        {
            if (isTouchingWall && !isGrounded && Time.time > lastWallJumpTime + wallJumpCooldown)
            {
                // Force le saut dans la direction opposée au mur avec une force plus importante
                float jumpDirectionX = -wallDirection; // -1 pour droite, 1 pour gauche
                Vector2 wallJumpForce = new Vector2(jumpDirectionX * wallJumpForceX, wallJumpForceY);
                
                // Reset complet de la vélocité
                rb.linearVelocity = Vector2.zero;
                
                // Applique la force de saut
                rb.AddForce(wallJumpForce, ForceMode2D.Impulse);
                
                // Désactive temporairement le contrôle du joueur pour forcer la direction
                mouvementHorizontal = jumpDirectionX; // Force le mouvement dans la direction du saut
                
                lastWallJumpTime = Time.time;
                isWallSliding = false;
                
                Debug.Log($"Wall Jump! Direction: {jumpDirectionX}, Force: {wallJumpForce}");
            }
            else if (isGrounded && !isTouchingWall)
            {
                rb.AddForce(Vector2.up * forceJump, ForceMode2D.Impulse);
            }
        }
    }

    private void FixedUpdate()
    {
        // Ground Check
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);

        // Wall Checks
        bool touchingRightWall = Physics2D.OverlapBox(wallCheckRight.position, wallCheckSize, 0f, groundLayer);
        bool touchingLeftWall = Physics2D.OverlapBox(wallCheckLeft.position, wallCheckSize, 0f, groundLayer);
        isTouchingWall = touchingRightWall || touchingLeftWall;
        wallDirection = touchingRightWall ? 1 : (touchingLeftWall ? -1 : 0);

        // Debug
        if (isTouchingWall)
        {
            Debug.Log($"Mur: {(touchingRightWall ? "Droit" : "Gauche")}, Vitesse Y: {rb.linearVelocity.y}, Au sol: {isGrounded}");
        }

        // Wall Slide avec vérification plus stricte
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

        // Movement
        rb.linearVelocity = new Vector2(mouvementHorizontal * vitesseDeplacement, rb.linearVelocity.y);
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
}

