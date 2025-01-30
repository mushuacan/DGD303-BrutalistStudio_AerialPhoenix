using System.Collections;
using UnityEngine;

public class EnemyType1_Shooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private string bulletType;
    [SerializeField] private Transform spawnPoint;
    public float shootingInterval = 1f;  // Seriler arasýndaki bekleme süresi
    public int burstCount = 3;           // Bir seride kaç mermi ateþlenecek
    public float burstInterval = 0.15f;  // Seri içindeki mermiler arasý süre
    public float pauseAfterBurst = 0.5f; // Seri sonrasý ekstra bekleme

    private bool isShooting;

    private void Start()
    {
        //StartCoroutine(ShootingRoutine());
    }

    private void OnEnable()
    {
        StartCoroutine(ShootingRoutine());
    }

    private IEnumerator ShootingRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootingInterval);

            for (int i = 0; i < burstCount; i++)
            {
                Shoot();
                yield return new WaitForSeconds(burstInterval);
            }

            yield return new WaitForSeconds(pauseAfterBurst);
        }
    }

    private void Shoot()
    {
        GameObject bullet = ObjectPoolSingleton.Instance.GetObject(bulletType);
        if (bullet != null)
        {
            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            if (bulletComponent != null)
            {
                bulletComponent.SetDirection(spawnPoint.forward);
                bullet.transform.position = spawnPoint.position;
                bullet.transform.rotation = spawnPoint.rotation;
            }
        }
    }
}
