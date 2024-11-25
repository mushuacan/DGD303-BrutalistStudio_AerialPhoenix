using DG.Tweening;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private MultiObjectPool objectPool;
    [SerializeField] private ActiveObjectCounter objectCounter;
    [SerializeField] private int minEnemyCount;
    [SerializeField] private bool spawnEnemies;


    void Start()
    {
        if (spawnEnemies)
        DOVirtual.DelayedCall(1, WaitAndSpawnNewEnemies);
    }

    private void WaitAndSpawnNewEnemies()
    {
        if (EnemyCount() < minEnemyCount)
        {
            CreateEnemies();
        }

        DOVirtual.DelayedCall(1, WaitAndSpawnNewEnemies);
    }

    private void CreateEnemies()
    {
        int randNum = Random.Range(1, 100);

        if (randNum <= 20)
        {
            SpawnEnemy("enemyT2", 3);
        }
        else if (randNum <= 70)
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
                SetPositionOfEnemies(key, obje);

            }
        }
    }

    private void SetPositionOfEnemies(string key, GameObject obje)
    {
        float randomZ = Random.Range(-10f, 10f);


        if (key == "enemyT1")
        {
            EnemyType1 enemyScript = obje.GetComponent<EnemyType1>();
            enemyScript.SetPosition(new Vector3(randomZ, 0, 10));
        }
        else if (key == "enemyT2")
        {
            obje.transform.position = new Vector3(randomZ, 0, 10);
        }
        else
        {
            obje.transform.position = new Vector3(randomZ, 0, 6);
        }
    }
}
