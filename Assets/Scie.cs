using UnityEngine;
using System.Collections;

public class SawMovement : MonoBehaviour
{
    public float speed = 10f; 
    public float leftLimit = -10f;
    public float rightLimit = 7f; 
    private bool isWaiting = false; 

    void Update()
    {
        if (!isWaiting) 
        {
            transform.position += Vector3.left * speed * Time.deltaTime;

            if (transform.position.x < leftLimit)
            {
                StartCoroutine(RestartSaw());
            }
        }
    }

    IEnumerator RestartSaw()
    {
        isWaiting = true; 
        yield return new WaitForSeconds(1.5f); 
        transform.position = new Vector3(rightLimit, transform.position.y, transform.position.z); // Remet la scie à droite
        isWaiting = false; 
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(collision.gameObject); 
            Debug.Log("Le joueur est mort !");
        }
    }
}
