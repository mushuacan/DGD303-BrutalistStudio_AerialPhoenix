using UnityEngine;
using System.Collections;

public class EnemyType2 : MonoBehaviour
{
    [SerializeField] private Transform target;          // Hedef karakter (�rne�in Player)
    [SerializeField] private float waitTime = 1f;       // Bekleme s�resi
    [SerializeField] private float upperBoundZ = 10f;   // Yukar� ��kma z seviyesi
    [SerializeField] private float boundaryX = 13f;     // Kenardan ��k�� X aral���
    [SerializeField] private float speed = 5f;          // Karakterin hareket h�z�
    [SerializeField] private float speedChangeRate;
    [SerializeField] private float maxTurnAngle = 3f;   // Maksimum d�n�� a��s�
    [SerializeField] private float angleChangeRate;
    [SerializeField] private float changeFrequency;
    private float changeTimer;

    private bool isDescending = true;       // Ba�lang�� ini� durumu

    private void Start()
    {
        // Karakterin ba�lang�� pozisyonunu rastgele ayarla
        float randomX = Random.Range(-boundaryX, boundaryX);
        transform.position = new Vector3(randomX, 0f, upperBoundZ);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;
    }

    private void Update()
    {
        // Ba�lang��ta a�a��ya inme hareketi
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
            // Hedefe do�ru s�n�rl� a��yla d�nerek ilerleme
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

        // Hedefe s�n�rl� a��da d�n
        FaceTarget();  

        // Hedefe do�ru ilerleme
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
