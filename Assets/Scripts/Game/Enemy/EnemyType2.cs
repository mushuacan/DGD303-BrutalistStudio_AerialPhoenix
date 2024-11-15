using UnityEngine;
using System.Collections;

public class EnemyType2 : MonoBehaviour
{
    [SerializeField] private Transform target;          // Hedef karakter (örneðin Player)
    [SerializeField] private float waitTime = 1f;       // Bekleme süresi
    [SerializeField] private float upperBoundZ = 10f;   // Yukarý çýkma z seviyesi
    [SerializeField] private float boundaryX = 13f;     // Kenardan çýkýþ X aralýðý
    [SerializeField] private float speed = 5f;          // Karakterin hareket hýzý
    [SerializeField] private float speedChangeRate;
    [SerializeField] private float maxTurnAngle = 3f;   // Maksimum dönüþ açýsý
    [SerializeField] private float angleChangeRate;
    [SerializeField] private float changeFrequency;
    private float changeTimer;

    private bool isDescending = true;       // Baþlangýç iniþ durumu

    private void Start()
    {
        // Karakterin baþlangýç pozisyonunu rastgele ayarla
        float randomX = Random.Range(-boundaryX, boundaryX);
        transform.position = new Vector3(randomX, 0f, upperBoundZ);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;
    }

    private void Update()
    {
        // Baþlangýçta aþaðýya inme hareketi
        if (isDescending)
        {
            transform.Translate(Vector3.forward * speed * 0.7f * Time.deltaTime);

            if (transform.position.z <= 6.5f)
            {
                isDescending = false;
                StartCoroutine(WaitAndRotateToTarget());
            }
        }
        else
        {
            // Hedefe doðru sýnýrlý açýyla dönerek ilerleme
            MoveTowardsTarget();

        }
    }

    private IEnumerator WaitAndRotateToTarget()
    {
        yield return new WaitForSeconds(waitTime);
    }


    private void FaceTarget()
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;

        float limitedAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, maxTurnAngle);
        transform.rotation = Quaternion.Euler(0, limitedAngle, 0);
    }

    private void IncreaseSpeed()
    {
        changeTimer += Time.deltaTime;
        if (changeTimer > changeFrequency)
        {
            speed += speedChangeRate;
            maxTurnAngle += angleChangeRate;
            changeTimer = 0;
        }
    }

    private void MoveTowardsTarget()
    {
        IncreaseSpeed();

        // Hedefe sýnýrlý açýda dön
        FaceTarget();  

        // Hedefe doðru ilerleme
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
