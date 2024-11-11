using UnityEngine;
using System.Collections;

public class CharacterScript : MonoBehaviour
{
    [SerializeField] private float speed = 5f;          // Karakterin hareket hýzý
    [SerializeField] private Transform target;          // Hedef karakter (örneðin Player)
    [SerializeField] private float waitTime = 1f;       // Bekleme süresi
    [SerializeField] private float maxTurnAngle = 3f;   // Maksimum dönüþ açýsý
    [SerializeField] private float upperBoundZ = 10f;   // Yukarý çýkma z seviyesi
    [SerializeField] private float boundaryX = 13f;     // Kenardan çýkýþ X aralýðý

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
        Debug.Log("Coroutine baþlatýldý");
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Bekleme tamamlandý");
    }


    private void FaceTarget()
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;

        float limitedAngle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, maxTurnAngle);
        transform.rotation = Quaternion.Euler(0, limitedAngle, 0);
        maxTurnAngle += 0.0001f;
    }

    private void MoveTowardsTarget()
    {
        FaceTarget();  // Hedefe sýnýrlý açýda dön

        // Hedefe doðru ilerleme
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
