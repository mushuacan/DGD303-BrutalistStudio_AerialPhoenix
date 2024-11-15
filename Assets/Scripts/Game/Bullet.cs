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
        Debug.LogWarning("Kur�un �ununla kar��la�t�: " + other.name + ", " + other.tag);
        if (other.gameObject.CompareTag("Player") && gameObject.tag != "Bullet_Player")
        {
            Debug.Log("Oyuncu hasar almal�");
        }
        else if (other.gameObject.CompareTag("Enemy") && gameObject.tag == "Bullet_Player")
        {
            if (other.gameObject.name == "EnemyType2(Clone)")
            {
                other.GetComponent<EnemyType2_Collision>().ExplodeByExternalFactors();
            }
        }
    }
}
