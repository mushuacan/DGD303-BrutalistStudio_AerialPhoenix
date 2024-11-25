using UnityEngine;

public class EnemyType1 : MonoBehaviour
{
    [Header("Reset")]

    [Header("Movement Settings")]
    public float verticalSpeed = 2f;           // A�a��ya hareket h�z�
    public float neededHeight = 5f;            // D��man�n duraca�� y�kseklik
    public float horizontalSpeed = 3f;         // Sa�-sol hareket h�z�
    public float horizontalMoveDistance = 3f;  // Sa�-sol hareket mesafesi
    [SerializeField] private float edgeX = 13f;

    [Header("Shooting Settings")]
    public GameObject bulletPrefab;            // Mermi prefab referans�
    public float shootingInterval = 1f;        // Ate� etme aral���

    private Vector3 leftPosition;              // Sa�-sol s�n�r pozisyonlar�
    private Vector3 rightPosition;
    private bool movingRight = true;           // Sa�-sol hareket y�n�
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
        // A�a��ya hareket etme ve belirtilen y�ksekli�e ula��nca durma
        if (transform.position.z > neededHeight)
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
            if (transform.position.x >= rightPosition.x && !collisioned)
                movingRight = false; // Sola d�n
        }
        else
        {
            transform.position -= new Vector3(horizontalSpeed * Time.deltaTime, 0, 0);
            if (transform.position.x <= leftPosition.x && !collisioned)
                movingRight = true; // Sa�a d�n
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

        // Sa�-sol hareket s�n�rlar�
        leftPosition = transform.position - new Vector3(horizontalMoveDistance, 0, 0);
        rightPosition = transform.position + new Vector3(horizontalMoveDistance, 0, 0);

        shootingTimer = shootingInterval; // �lk at�� i�in zamanlay�c�y� ba�lat
    }

    private void Shoot()
    {
        GameObject bullet = ObjectPoolSingleton.Instance.GetObject("bullet_eT1");
        bullet.GetComponent<Bullet>().SetDirection(Vector3.back);
        bullet.transform.position = this.gameObject.transform.position;
    }

    #region EnemyType1 �� i�e giri�i engelleme merkezi
    private void OnTriggerEnter(Collider other)
    {
        string otherName = other.gameObject.name;
        if (collisioned == false && otherName == "EnemyType1(Clone)" || otherName == "EnemyType3")
        {
            movingRight = !movingRight;
            collisioned = true;
        }
        //Debug.Log("Enemy 1 �ununla kar��la�t� ->" + other.gameObject.name);
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
