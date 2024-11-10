using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Mermi h�z�
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
        // Mermi bir objeye �arpt���nda yok olsun
        Destroy(gameObject);
    }
}
