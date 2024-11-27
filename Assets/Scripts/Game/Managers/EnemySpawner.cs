using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private MultiObjectPool objectPool;
    [SerializeField] private ActiveObjectCounter objectCounter;
    [SerializeField] private int maxEnemyCount;
    [SerializeField] private bool spawnEnemies;

    private Tween spawnerTween;

    [Header("Lütfen ilk elemanýný 0 býrakýn")]
    [Tooltip("Kaç düþman ölünce diðer faza geçilecek?")]
    public List<int> phasesThreshold = new List<int>();
    [SerializeField] int currentPhase;

    [SerializeField] private int totalEnemySpawned;
    [SerializeField] private int totalEnemyDied;
    [SerializeField] private int enemyCountNOW;

    void Start()
    {
        currentPhase = 1;
        if (spawnEnemies)
            spawnerTween = DOVirtual.DelayedCall(1, WaitAndSpawnNewEnemies);
    }

    private void WaitAndSpawnNewEnemies()
    {
        if (EnemyCount() >= maxEnemyCount)
        {
            spawnerTween = DOVirtual.DelayedCall(2, WaitAndSpawnNewEnemies);
            return;
        }

        PhaseArranger();

        CreateEnemies();

        spawnerTween = DOVirtual.DelayedCall(1, WaitAndSpawnNewEnemies);
    }

    private void CreateEnemies()
    {
        if (ObjectPoolSingleton.Instance.GetEnemyCountInfo(3) >= phasesThreshold[currentPhase])
            return;

        switch (currentPhase)
        {
            case 1:
                SpawnEnemy("enemyT1");
                break;

            case 2:
                SpawnEnemy("enemyT2");
                break;

            case 3:
                int randNum = Random.Range(1, 10);
                if (randNum <= 2)
                {
                    SpawnEnemy("enemyT2", 3);
                }
                else if (randNum <= 7)
                {
                    SpawnEnemy("enemyT1");
                }
                else
                {
                    SpawnEnemy("enemyT2");
                }
                break;

            case 4:
                SpawnEnemy("enemyT3");
                break;
        }
    }

    private void PhaseArranger()
    {
        totalEnemySpawned = ObjectPoolSingleton.Instance.GetEnemyCountInfo(3);
        totalEnemyDied = ObjectPoolSingleton.Instance.GetEnemyCountInfo(2);
        enemyCountNOW = EnemyCount();

        if(totalEnemyDied >= phasesThreshold[currentPhase]) 
        {
            currentPhase++;
        }
        if (phasesThreshold.Count < currentPhase)
        {
            spawnerTween.Kill();
            Debug.LogError("Fazlar bitti");
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
