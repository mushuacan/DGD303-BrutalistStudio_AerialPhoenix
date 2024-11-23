using DG.Tweening;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private MultiObjectPool objectPool;
    [SerializeField] private ActiveObjectCounter objectCounter;


    void Start()
    {
        Debug.LogError("ResetState ayarlamayý unutma ~Mushu");
        DOVirtual.DelayedCall(1, WaitAndSpawnNewEnemies);
    }

    private void WaitAndSpawnNewEnemies()
    {
        if (EnemyCount() < 3)
        {
            CreateEnemies();
        }

        DOVirtual.DelayedCall(1, WaitAndSpawnNewEnemies);
    }

    private void CreateEnemies()
    {
        int randNum = Random.Range(1, 100);

        if (randNum <= 40)
        {
            SpawnEnemy("enemyT2", 3);
        }
        else if (randNum <= 60)
        {
            SpawnEnemy("enemyT1", 1);
        }
        else
        {
            SpawnEnemy("enemyT2", 1);
        }
    }

    private int EnemyCount()
    {
        return objectCounter.GetActiveObjectCount();
    }

    public void SpawnEnemy(string key, int howMany = 1)
    {
        for (int i = 0; i < howMany; i++)
        {
            GameObject obje = ObjectPoolSingleton.Instance.GetObject(key);
            if (obje != null)
            {
                // Z pozisyonu -13 ile 13 arasýnda rastgele bir deðer
                float randomZ = Random.Range(-10f, 10f);

                // Objeyi yeni Z pozisyonuna yerleþtir
                obje.transform.position = new Vector3(randomZ, 0, 6);

            }
        }
    }
}
