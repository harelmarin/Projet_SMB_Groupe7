using UnityEngine;
using System.Collections;

public class PlatformDisappear : MonoBehaviour
{
    [SerializeField] private float disappearDelay = 1f;
    [SerializeField] private float respawnDelay = 1.5f;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private bool isDisappearing = false;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isDisappearing)
        {
            StartCoroutine(DisappearAndRespawn());
        }
    }

    IEnumerator DisappearAndRespawn()
    {
        isDisappearing = true;
        
        // Désactive le trigger et attend
        boxCollider.isTrigger = false;
        yield return new WaitForSeconds(disappearDelay);
        
        // Fait disparaître la plateforme
        spriteRenderer.enabled = false;
        boxCollider.enabled = false;
        
        // Attend puis fait réapparaître
        yield return new WaitForSeconds(respawnDelay);
        
        // Réactive tout
        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
        boxCollider.isTrigger = true;
        isDisappearing = false;
    }
}