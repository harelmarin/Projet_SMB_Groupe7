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
        transform.position = new Vector3(rightLimit, transform.position.y, transform.position.z);
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
