using UnityEngine;

public class EnemyType1 : MonoBehaviour
{
    [Header("Reset")]

    [Header("Movement Settings")]
    public float verticalSpeed = 2f;           // Aþaðýya hareket hýzý
    public float moveHeight = 5f;              // Düþmanýn duracaðý yükseklik
    public float horizontalSpeed = 3f;         // Sað-sol hareket hýzý
    public float horizontalMoveDistance = 3f;  // Sað-sol hareket mesafesi

    [Header("Shooting Settings")]
    public GameObject bulletPrefab;            // Mermi prefab referansý
    public float shootingInterval = 1f;        // Ateþ etme aralýðý

    private Vector3 leftPosition;              // Sað-sol sýnýr pozisyonlarý
    private Vector3 rightPosition;
    private bool movingRight = true;           // Sað-sol hareket yönü

    private float shootingTimer;

    private void Start()
    {
        ResetSettings();
    }

    private void OnEnable()
    {
        ResetSettings();
    }

    private void ResetSettings()
    {
        movingRight = true;
    }

    private void Update()
    {
        // Aþaðýya hareket etme ve belirtilen yüksekliðe ulaþýnca durma
        if (transform.position.z > moveHeight)
        {
            transform.position -= new Vector3(0, 0, verticalSpeed * Time.deltaTime);
        }
        else
        {
            // Sað-sol hareket
            HorizontalMovement();

            // Ateþ etme
            shootingTimer -= Time.deltaTime;
            if (shootingTimer <= 0)
            {
                Shoot();
                shootingTimer = shootingInterval; // Zamanlayýcýyý sýfýrla
            }
        }
    }

    private void HorizontalMovement()
    {
        // Sað-sol pozisyonuna göre hareket et
        if (movingRight)
        {
            transform.position += new Vector3(horizontalSpeed * Time.deltaTime, 0, 0);
            if (transform.position.x >= rightPosition.x)
                movingRight = false; // Sola dön
        }
        else
        {
            transform.position -= new Vector3(horizontalSpeed * Time.deltaTime, 0, 0);
            if (transform.position.x <= leftPosition.x)
                movingRight = true; // Saða dön
        }
    }

    public void SetPosition(Vector3 pozition)
    {
        transform.position = pozition;

        // Sað-sol hareket sýnýrlarý
        leftPosition = transform.position - new Vector3(horizontalMoveDistance, 0, 0);
        rightPosition = transform.position + new Vector3(horizontalMoveDistance, 0, 0);

        shootingTimer = shootingInterval; // Ýlk atýþ için zamanlayýcýyý baþlat
    }

    private void Shoot()
    {
        GameObject bullet = ObjectPoolSingleton.Instance.GetObject("bullet_eT1");
        bullet.GetComponent<Bullet>().SetDirection(Vector3.back);
        bullet.transform.position = this.gameObject.transform.position;
    }
}
