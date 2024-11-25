using UnityEngine;

public class EnemyType1 : MonoBehaviour
{
    [Header("Reset")]

    [Header("Movement Settings")]
    public float verticalSpeed = 2f;           // Aþaðýya hareket hýzý
    public float neededHeight = 5f;            // Düþmanýn duracaðý yükseklik
    public float horizontalSpeed = 3f;         // Sað-sol hareket hýzý
    public float horizontalMoveDistance = 3f;  // Sað-sol hareket mesafesi
    [SerializeField] private float edgeX = 13f;

    [Header("Shooting Settings")]
    public GameObject bulletPrefab;            // Mermi prefab referansý
    public float shootingInterval = 1f;        // Ateþ etme aralýðý

    private Vector3 leftPosition;              // Sað-sol sýnýr pozisyonlarý
    private Vector3 rightPosition;
    private bool movingRight = true;           // Sað-sol hareket yönü
    private bool collisioned = false;

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
        collisioned = false;
        movingRight = true;
    }

    private void Update()
    {
        // Aþaðýya hareket etme ve belirtilen yüksekliðe ulaþýnca durma
        if (transform.position.z > neededHeight)
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
            if (transform.position.x >= rightPosition.x && !collisioned)
                movingRight = false; // Sola dön
        }
        else
        {
            transform.position -= new Vector3(horizontalSpeed * Time.deltaTime, 0, 0);
            if (transform.position.x <= leftPosition.x && !collisioned)
                movingRight = true; // Saða dön
        }
        if (transform.position.x >= edgeX)
        {
            movingRight = false;
        }
        else if (transform.position.x <= -edgeX)
        {
            movingRight = true;
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

    #region EnemyType1 Ýç içe giriþi engelleme merkezi
    private void OnTriggerEnter(Collider other)
    {
        string otherName = other.gameObject.name;
        if (collisioned == false && otherName == "EnemyType1(Clone)" || otherName == "EnemyType3")
        {
            movingRight = !movingRight;
            collisioned = true;
        }
        //Debug.Log("Enemy 1 þununla karþýlaþtý ->" + other.gameObject.name);
    }

    private void OnTriggerStay(Collider other)
    {
        string objName = other.gameObject.name;
        if (objName == "EnemyType1(Clone)")
        {
            collisioned = true;
            if (other.transform.position.x < transform.position.x && 0 < transform.position.x)
            {
                transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z);
            }
            if (other.transform.position.x > transform.position.x && 0 > transform.position.x)
            {
                transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        collisioned = false;
        SetPosition(transform.position);
    }
    #endregion
}
