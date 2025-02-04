using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Mermi hýzý
    public float damage = 10f;
    [SerializeField] private float maxRange = 30f;
    [SerializeField] private string bulletObjectKey;

    private Vector3 direction;

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        EdgeControl();
    }

    private void EdgeControl()
    {
        if (Mathf.Abs(transform.position.x) > maxRange)
        {
            ReturnBullet();
        }
        if (Mathf.Abs(transform.position.y) > maxRange)
        {
            ReturnBullet();
        }
        if (Mathf.Abs(transform.position.z) > maxRange)
        {
            ReturnBullet();
        }
    }

    private void ReturnBullet()
    {
        ObjectPoolSingleton.Instance.ReturnObject(bulletObjectKey, this.gameObject);
    }

    public void ArrangedDamage(float setDamage)
    {
        damage = setDamage;
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Kurþun þununla karþýlaþtý: " + other.name + ", " + other.tag);
        if (other.gameObject.CompareTag("Player") && gameObject.tag != "Bullet_Player")
        {
            Debug.Log("Oyuncu hasar almalý");
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            playerHealth.DamagePlayer(damage);
            ReturnBullet();
        }
        else if (other.gameObject.CompareTag("Enemy") && gameObject.tag == "Bullet_Player")
        {
            if (other.gameObject.name == "EnemyType1(Clone)")
            {
                EnemyHealth enemyHealth = (EnemyHealth)other.gameObject.GetComponent<EnemyHealth>();
                enemyHealth.GiveDamage(damage);
                Debug.Log("Kurþun tarafýndan " + other.name + ", hasar gördü. Þimdi kurþun kendisini yok edecek.");
                ReturnBullet();
            }
            else if (other.gameObject.name == "EnemyType2(Clone)")
            {
                EnemyHealth enemyHealth = (EnemyHealth)other.gameObject.GetComponent<EnemyHealth>();
                enemyHealth.GiveDamage(damage);
                Debug.Log("Kurþun tarafýndan " + other.name + ", hasar gördü. Þimdi kurþun kendisini yok edecek.");
                ReturnBullet();
            }
            else if (other.gameObject.name == "EnemyType3(Clone)")
            {
                EnemyHealth enemyHealth = (EnemyHealth)other.gameObject.GetComponent<EnemyHealth>();
                enemyHealth.GiveDamage(damage);
                Debug.Log("Kurþun tarafýndan " + other.name + ", hasar gördü. Þimdi kurþun kendisini yok edecek.");
                ReturnBullet();
            }
            else if (other.gameObject.name == "EnemyType4(Clone)")
            {
                EnemyHealth enemyHealth = (EnemyHealth)other.gameObject.GetComponent<EnemyHealth>();
                enemyHealth.GiveDamage(damage);
                Debug.Log("Kurþun tarafýndan " + other.name + ", hasar gördü. Þimdi kurþun kendisini yok edecek.");
                ReturnBullet();
            }
        }
    }
}
