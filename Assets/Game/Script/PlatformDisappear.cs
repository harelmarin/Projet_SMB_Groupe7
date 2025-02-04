using UnityEngine;
using System.Collections;

public class PlatformDisappear : MonoBehaviour
{
    [SerializeField] private float disappearDelay = 1f;
    private Collider2D platformCollider;
    private SpriteRenderer spriteRenderer;
    private bool isDisappearing = false;

    void Start()
    {
        platformCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (platformCollider != null)
        {
            platformCollider.isTrigger = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isDisappearing)
        {
            StartCoroutine(Disappear());
        }
    }

    IEnumerator Disappear()
    {
        isDisappearing = true;
        
        // Désactive le trigger et attend
        if (platformCollider != null)
        {
            platformCollider.isTrigger = false;
        }
        yield return new WaitForSeconds(disappearDelay);
        
        // Fait disparaître la plateforme définitivement
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
        if (platformCollider != null)
        {
            platformCollider.enabled = false;
        }
        
        // Optionnel : détruit complètement l'objet
        Destroy(gameObject);
    }
}