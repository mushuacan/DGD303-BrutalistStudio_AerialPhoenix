using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UIElements;

public class EnemyType2 : MonoBehaviour
{
    [SerializeField] private Transform target;          // Hedef karakter (örneðin Player)
    [SerializeField] private float waitTime = 1f;       // Bekleme süresi
    [SerializeField] private float upperBoundZ = 10f;   // Yukarý çýkma z seviyesi
    [SerializeField] private float boundaryX = 13f;     // Kenardan çýkýþ X aralýðý
    [SerializeField] private float baseSpeed = 5f;      // Karakterin baþlangýç hareket hýzý
    [SerializeField] private float maxTurnAngle = 3f;   // Maksimum dönüþ açýsý
    [SerializeField] private float explosionDistance = 2f; // Patlama mesafesi
    [SerializeField] private float speedMultiplier = 1.5f; // Hýzlanma çarpaný
    [SerializeField] private float turnStrengthIncreaseInterval = 3f; // Kaç saniyede dönüþ kabiliyeti artar
    [SerializeField] private float turnStrengthMultiplier = 1.2f;     // Dönüþ gücü artýþ oraný
    private Tween delayedTween;

    private float currentSpeed;
    private float turnStrengthTimer;
    private bool isDescending = true;                  // Baþlangýç iniþ durumu

    private void Start()
    {
        // Karakterin baþlangýç pozisyonunu rastgele ayarla
        float randomX = Random.Range(-boundaryX, boundaryX);
        transform.position = new Vector3(randomX, 0f, upperBoundZ);

        // Hedefi bul
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }

        // Baþlangýç hýzý ve dönüþ güçlendirme zamanlayýcýsý
        currentSpeed = baseSpeed;
        turnStrengthTimer = 0f;
    }

    private void Update()
    {
        if (target == null) return;

        // Zamanlayýcý ile dönüþ kabiliyeti artýr
        turnStrengthTimer += Time.deltaTime;
        if (turnStrengthTimer >= turnStrengthIncreaseInterval)
        {
            maxTurnAngle *= turnStrengthMultiplier; // Dönüþ kabiliyeti artýr
            turnStrengthTimer = 0f;                 // Zamanlayýcý sýfýrla
        }

        // Baþlangýçta aþaðýya inme hareketi
        if (isDescending)
        {
            if (transform.position.z <= 6.5f)
            {
                if (delayedTween == null)
                {
                    delayedTween = DOVirtual.DelayedCall(waitTime, StartCharging);
                }
            }
            else
            {
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
        // Yönü -Z pozisyonuna döndür
        transform.rotation = Quaternion.Euler(0, 180, 0);

    }

    private void MoveTowardsTarget()
    {
        // Hedefe doðru dön
        FaceTarget();

        // Oyuncuya yeterince yaklaþtýysa patlama
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget <= explosionDistance)
        {
            Explode();
        }
        else
        {
            // Hedef uzaksa hýzlanarak ilerle
            currentSpeed = baseSpeed * speedMultiplier;
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
        }
    }

    private void FaceTarget()
    {
        if (target == null) return;

        Vector3 directionToTarget = (target.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;

        // Sýnýrlý dönüþ açýsý
        float limitedAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, maxTurnAngle * Time.deltaTime * currentSpeed);
        transform.rotation = Quaternion.Euler(0, limitedAngle, 0);
    }

    private void Explode()
    {
        EnemyType2_Collision enemyType2_Collision = GetComponent<EnemyType2_Collision>();
        if (enemyType2_Collision == null)
        {
            Debug.LogError("EnemyType2_Collision bulunamadý. Yeni bir bileþen ekleniyor.");
            enemyType2_Collision = gameObject.AddComponent<EnemyType2_Collision>();
        }

        if (enemyType2_Collision != null)
        {
            enemyType2_Collision.ExplodeYourself();
        }
        else
        {
            Debug.LogError("EnemyType2_Collision bileþeni hala eklenemedi.");
        }
    }

}
