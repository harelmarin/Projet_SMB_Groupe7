using UnityEngine;
using System.Collections;

public class SawMovement : MonoBehaviour
{
    public float speed = 10f;
    public float leftLimit = -10f;
    [SerializeField] private float respawnTime = 2f;
    
    private bool isWaiting = false;
    private Vector3 initialPosition;
    private SpriteRenderer spriteRenderer;
    private Collider2D sawCollider;

    void Awake()
    {
        initialPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        sawCollider = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        if (!isWaiting)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;

            if (transform.position.x < leftLimit)
            {
                StartCoroutine(RespawnSaw());
            }
        }
    }

    IEnumerator RespawnSaw()
    {
        isWaiting = true;
        
        // Désactive les composants
        spriteRenderer.enabled = false;
        sawCollider.enabled = false;
        
        yield return new WaitForSeconds(respawnTime);
        
        // Replace la scie et réactive les composants
        transform.position = initialPosition;
        spriteRenderer.enabled = true;
        sawCollider.enabled = true;
        
        isWaiting = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.Die();
            }
        }
    }
}