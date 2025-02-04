using UnityEngine;

public class CollisionMort : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private Vector3 respawnPoint; // Point de respawn

    private void Start()
    {
        // Sauvegarde la position initiale du joueur comme point de respawn
        // si vous n'en définissez pas un dans l'Inspector
        if (respawnPoint == Vector3.zero)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                respawnPoint = player.transform.position;
            }
        }
    }

    void FixedUpdate()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    // Utilisez soit OnCollisionEnter2D soit OnTriggerEnter2D selon votre configuration
    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandlePlayerCollision(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandlePlayerCollision(other.gameObject);
    }

    private void HandlePlayerCollision(GameObject collisionObject)
    {
        if (collisionObject.CompareTag("Player"))
        {
           
            collisionObject.transform.position = respawnPoint;
            
            
            Rigidbody2D rb = collisionObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
}