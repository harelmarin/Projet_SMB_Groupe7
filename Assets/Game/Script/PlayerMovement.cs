using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float vitesseDeplacement = 5f;
    private Rigidbody2D rb;
    private float mouvementHorizontal;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        mouvementHorizontal = context.ReadValue<Vector2>().x;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(mouvementHorizontal * vitesseDeplacement, rb.linearVelocity.y);
    }
}

