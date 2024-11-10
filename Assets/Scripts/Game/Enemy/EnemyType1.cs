using UnityEngine;

public class EnemyType1 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float verticalSpeed = 2f;           // A�a��ya hareket h�z�
    public float moveHeight = 5f;              // D��man�n duraca�� y�kseklik
    public float horizontalSpeed = 3f;         // Sa�-sol hareket h�z�
    public float horizontalMoveDistance = 3f;  // Sa�-sol hareket mesafesi

    [Header("Shooting Settings")]
    public GameObject bulletPrefab;            // Mermi prefab referans�
    public float shootingInterval = 1f;        // Ate� etme aral���

    private Vector3 leftPosition;              // Sa�-sol s�n�r pozisyonlar�
    private Vector3 rightPosition;
    private bool movingRight = true;           // Sa�-sol hareket y�n�

    private float shootingTimer;

    private void Start()
    {
        // Sa�-sol hareket s�n�rlar�
        leftPosition = transform.position - new Vector3(horizontalMoveDistance, 0, 0);
        rightPosition = transform.position + new Vector3(horizontalMoveDistance, 0, 0);

        shootingTimer = shootingInterval; // �lk at�� i�in zamanlay�c�y� ba�lat
    }

    private void Update()
    {
        // A�a��ya hareket etme ve belirtilen y�ksekli�e ula��nca durma
        if (transform.position.z > moveHeight)
        {
            transform.position -= new Vector3(0, 0, verticalSpeed * Time.deltaTime);
        }
        else
        {
            // Sa�-sol hareket
            HorizontalMovement();

            // Ate� etme
            shootingTimer -= Time.deltaTime;
            if (shootingTimer <= 0)
            {
                Shoot();
                shootingTimer = shootingInterval; // Zamanlay�c�y� s�f�rla
            }
        }
    }

    private void HorizontalMovement()
    {
        // Sa�-sol pozisyonuna g�re hareket et
        if (movingRight)
        {
            transform.position += new Vector3(horizontalSpeed * Time.deltaTime, 0, 0);
            if (transform.position.x >= rightPosition.x)
                movingRight = false; // Sola d�n
        }
        else
        {
            transform.position -= new Vector3(horizontalSpeed * Time.deltaTime, 0, 0);
            if (transform.position.x <= leftPosition.x)
                movingRight = true; // Sa�a d�n
        }
    }

    private void Shoot()
    {
        // Mermiyi instantiate et ve a�a�� y�nl� pozisyonla
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().SetDirection(Vector3.back);
    }
}
