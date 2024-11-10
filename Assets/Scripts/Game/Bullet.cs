using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Mermi hýzý
    private Vector3 direction;

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Mermi bir objeye çarptýðýnda yok olsun
        Destroy(gameObject);
    }
}
