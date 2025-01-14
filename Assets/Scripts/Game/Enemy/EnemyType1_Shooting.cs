using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType1_Shooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private string bulletType;
    [SerializeField] private Transform spawnPoint;
    public float shootingInterval = 1f;        // Shooting interval

    private float shootingTimer;

    private void Start()
    {
        shootingTimer = shootingInterval;
    }

    private void Update()
    {
        HandleShooting();
    }

    private void HandleShooting()
    {
        shootingTimer -= Time.deltaTime;
        if (shootingTimer <= 0)
        {
            Shoot();
            shootingTimer = shootingInterval;
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

