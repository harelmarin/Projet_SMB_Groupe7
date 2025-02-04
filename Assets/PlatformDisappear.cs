using UnityEngine;
using System.Collections;

public class PlatformTriggerToggle : MonoBehaviour
{
    private BoxCollider2D boxCollider; 

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Le joueur est sur la plateforme !");
            StartCoroutine(DisableTriggerTemporarily());
        }
    }

    IEnumerator DisableTriggerTemporarily()
    {
        boxCollider.isTrigger = false; 
        Debug.Log("Is Trigger désactivé !");
        
        yield return new WaitForSeconds(1.5f); 
        
        boxCollider.isTrigger = true;
        Debug.Log("Is Trigger réactivé !");
    }
}
