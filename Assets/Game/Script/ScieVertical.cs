using UnityEngine;
using System.Collections;

public class SawVerticalMovement : MonoBehaviour
{
    public float speed = 10f;
    public float bottomLimit = -10f;
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
            // Change Vector3.left en Vector3.down pour descendre
            transform.position += Vector3.down * speed * Time.deltaTime;

            // Vérifie la position Y au lieu de X
            if (transform.position.y < bottomLimit)
            {
                StartCoroutine(RespawnSaw());
            }
        }
    }

    IEnumerator RespawnSaw()
    {
        isWaiting = true;
        
        spriteRenderer.enabled = false;
        sawCollider.enabled = false;
        
        yield return new WaitForSeconds(respawnTime);
        
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