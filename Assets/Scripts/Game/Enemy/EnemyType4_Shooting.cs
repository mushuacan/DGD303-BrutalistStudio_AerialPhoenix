using System.Collections;
using UnityEngine;

public class EnemyType4_Shooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private string bulletType;
    [SerializeField] private Transform spawnPoint1;
    [SerializeField] private Transform spawnPoint2;
    public float shootingInterval = 1f;  // Seriler arasýndaki bekleme süresi
    public int burstCount = 3;           // Bir seride kaç mermi ateþlenecek
    public float burstInterval = 0.15f;  // Seri içindeki mermiler arasý süre
    public float pauseAfterBurst = 0.5f; // Seri sonrasý ekstra bekleme

    private bool shootingTurn;
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
                if (shootingTurn) { Shoot(spawnPoint1); }
                else { Shoot(spawnPoint2); }

                shootingTurn = !shootingTurn;

                yield return new WaitForSeconds(burstInterval);
            }

            yield return new WaitForSeconds(pauseAfterBurst);
        }
    }

    private void Shoot(Transform spawnpoint)
    {
        GameObject bullet = ObjectPoolSingleton.Instance.GetObject(bulletType);
        if (bullet != null)
        {
            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            if (bulletComponent != null)
            {
                bulletComponent.SetDirection(spawnpoint.forward);
                bullet.transform.position = spawnpoint.position;
                bullet.transform.rotation = spawnpoint.rotation;
            }
        }
    }
}
