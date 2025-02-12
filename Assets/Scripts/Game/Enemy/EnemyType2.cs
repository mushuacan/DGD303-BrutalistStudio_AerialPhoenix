using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class EnemyType2 : MonoBehaviour
{
    [Header("De�i�kenler")]
    [SerializeField] private Transform subComponent;
    [SerializeField] private Transform target;          // Hedef karakter (�rne�in Player)
    [SerializeField] private float waitTime = 1f;       // Bekleme s�resi
    [SerializeField] private float upperBoundZ = 10f;   // Yukar� ��kma z seviyesi
    [SerializeField] private float boundaryX = 13f;     // Kenardan ��k�� X aral���

    [SerializeField] private float tiltSensitivity = 0.5f; // Yat�� hassasiyeti
    [SerializeField] private float maxTiltAngle = 15f; // Maksimum yat�� a��s�
    [SerializeField] private float tiltSmoothTime = 0.1f; // Yat�� yumu�atma s�resi

    private float currentTiltAngle = 0f; // Mevcut tilt a��s�
    private float tiltVelocity = 0f; // Yumu�atma i�in h�z de�eri


    [Header("initial De�i�kenler")]
    [SerializeField] private float baseSpeed = 5f;      // Karakterin ba�lang�� hareket h�z�
    [SerializeField] private float initialTurnAngle = 3f;   // Maksimum d�n�� a��s�
    [SerializeField] private float explosionDistance = 2f; // Patlama mesafesi
    [SerializeField] private float speedMultiplier = 1.5f; // H�zlanma �arpan�
    [SerializeField] private float turnStrengthIncreaseInterval = 3f; // Ka� saniyede d�n�� kabiliyeti artar
    [SerializeField] private float turnStrengthMultiplier = 1.2f;     // D�n�� g�c� art�� oran�
    private Tween delayedTween;

    private float currentSpeed;
    private float maxTurnAngle;
    private float turnStrengthTimer;
    private bool isDescending = true;                  // Ba�lang�� ini� durumu

    private void Start()
    {

        // Hedefi bul
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        ResetState();
        
    }

    private void Update()
    {
        if (target == null)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (-0.7f * currentSpeed * Time.deltaTime));
            Debug.LogWarning("Target yok"); //return;
        }
        // Zamanlay�c� ile d�n�� kabiliyeti art�r
        turnStrengthTimer += Time.deltaTime;
        if (turnStrengthTimer >= turnStrengthIncreaseInterval)
        {
            maxTurnAngle *= turnStrengthMultiplier; // D�n�� kabiliyeti art�r
            turnStrengthTimer = 0f;                 // Zamanlay�c� s�f�rla
        }

        // Ba�lang��ta a�a��ya inme hareketi
        if (isDescending)
        {
            if (transform.position.z <= 6.5f)
            {
                if (delayedTween == null)
                {
                    delayedTween = DOVirtual.DelayedCall(waitTime, StartCharging);
                }
                else
                {
                    // Debug.Log("TweenError #1");
                }
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (-0.7f * currentSpeed * Time.deltaTime));
            }
        }
        else
        {
            MoveTowardsTarget();
        }
    }

    private void StartCharging()
    {
        isDescending = false;
        // Y�n� -Z pozisyonuna d�nd�r
        transform.rotation = Quaternion.Euler(0, 180, 0);

    }

    private void MoveTowardsTarget()
    {
        if (target == null) return;

        // Hedefe do�ru d�n
        FaceTarget();

        // Oyuncuya yeterince yakla�t�ysa patlama
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget <= explosionDistance)
        {
            Explode();
        }
        else
        {
            // Hedef uzaksa h�zlanarak ilerle
            currentSpeed = baseSpeed * speedMultiplier;
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
        }
    }

    private void FaceTarget()
    {
        if (target == null) return;

        // Hedefe y�nelmek i�in a�� hesaplama
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;

        // S�n�rl� d�n�� a��s�
        float limitedAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, maxTurnAngle * Time.deltaTime * currentSpeed);
        transform.rotation = Quaternion.Euler(0, limitedAngle, 0);

        // Alt bile�eni sa�a sola yat�rma
        if (subComponent != null)
        {
            // Hedefin karakterin sa��nda m� solunda m� oldu�unu bul
            float angleDifference = Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle);

            // Yat�� y�n�n� hesapla
            float targetTiltAngle = Mathf.Clamp(-angleDifference * tiltSensitivity, -maxTiltAngle, maxTiltAngle);

            // Yumu�at�lm�� tilt a��s�n� hesapla
            currentTiltAngle = Mathf.SmoothDamp(currentTiltAngle, targetTiltAngle, ref tiltVelocity, tiltSmoothTime);

            // Alt bile�ene rotasyonu uygula
            subComponent.localRotation = Quaternion.Euler(0, 0, currentTiltAngle);
        }
    }

    private void Explode()
    {
        EnemyType2_Collision enemyType2_Collision = GetComponent<EnemyType2_Collision>();
        if (enemyType2_Collision == null)
        {
            Debug.LogError("EnemyType2_Collision bulunamad�. Yeni bir bile�en ekleniyor.");
            enemyType2_Collision = gameObject.AddComponent<EnemyType2_Collision>();
        }

        if (enemyType2_Collision != null)
        {
            enemyType2_Collision.ExplodeYourself();
        }
        else
        {
            Debug.LogError("EnemyType2_Collision bile�eni hala eklenemedi.");
        }
    }

    private void OnEnable()
    {
        ResetState();
    }

    private void ResetState()
    {
        isDescending = true;
        delayedTween.Kill();
        delayedTween = null;
        currentSpeed = baseSpeed;
        maxTurnAngle = initialTurnAngle;

        currentTiltAngle = 0f;
        tiltVelocity = 0f;

        // Karakterin ba�lang�� pozisyonunu rastgele ayarla
        float randomX = Random.Range(-boundaryX, boundaryX);
        transform.position = new Vector3(randomX, 0f, upperBoundZ); 
        transform.rotation = Quaternion.Euler(0, 180, 0);
        subComponent.localRotation = Quaternion.Euler(0, 0, 0);
    }

}
