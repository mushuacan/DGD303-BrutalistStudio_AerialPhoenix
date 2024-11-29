// Optimized by ChatGPT Code Copilot under mushu's supervision

using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] private MultiObjectPool objectPool;
    [SerializeField] private ActiveObjectCounter objectCounter;
    [SerializeField] private int maxEnemyCount;
    [SerializeField] private bool spawnEnemies;

    [Header("Phase Settings")]
    [Tooltip("Lütfen ilk elemanýný 0 býrakýn.")]
    [SerializeField] private List<int> phasesThreshold = new List<int> { 0 };
    [SerializeField] private List<int> phasesMaxEnemyCount = new List<int>();
    [SerializeField] private List<string> phaseNames = new List<string>();
    [SerializeField] private TextMeshProUGUI phaseText;

    [Header("Internal Variables")]
    [SerializeField] private int currentPhase = 1;
    private int totalEnemySpawned;
    private int totalEnemyDied;
    private int enemyCountNOW;

    private Tween spawnerTween;

    private const float SpawnPositionZ_T1 = 10f;
    private const float SpawnPositionZ_T2 = 10f;
    private const float SpawnPositionZ_T3 = 6f;
    private const float SpawnDelay = 1f;
    private const float PhaseCheckDelay = 2f;

    void Start()
    {
        if (spawnEnemies)
        {
            StartSpawning();
        }
        else
        {
            TestSpawn();
        }
    }

    private void StartSpawning()
    {
        spawnerTween = DOVirtual.DelayedCall(SpawnDelay, SpawnEnemiesLoop);
    }

    private void TestSpawn()
    {
        SpawnEnemy("enemyT3");
    }

    private void SpawnEnemiesLoop()
    {
        if (EnemyCount() >= maxEnemyCount)
        {
            spawnerTween = DOVirtual.DelayedCall(PhaseCheckDelay, SpawnEnemiesLoop);
            return;
        }

        PhaseArranger();
        CreateEnemies();

        spawnerTween = DOVirtual.DelayedCall(SpawnDelay, SpawnEnemiesLoop);
    }

    private void CreateEnemies()
    {
        if (totalEnemySpawned >= phasesThreshold[currentPhase])
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
                HandlePhaseThreeEnemySpawning();
                break;

            case 4:
                SpawnEnemy("enemyT3");
                break;
        }
    }

    private void HandlePhaseThreeEnemySpawning()
    {
        int randNum = Random.Range(1, 10);

        if (randNum <= 2 && (phasesThreshold[currentPhase] - 4) >= totalEnemySpawned)
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
    }

    private void PhaseArranger()
    {
        totalEnemySpawned = ObjectPoolSingleton.Instance.GetEnemyCountInfo(3);
        totalEnemyDied = ObjectPoolSingleton.Instance.GetEnemyCountInfo(2);
        enemyCountNOW = EnemyCount();

        if (totalEnemyDied >= phasesThreshold[currentPhase])
        {
            currentPhase++;
            if (currentPhase >= phasesThreshold.Count)
            {
                spawnerTween.Kill();
                Debug.LogError("Tüm fazlar tamamlandý.");
                return;
            }
        }

        maxEnemyCount = phasesMaxEnemyCount[currentPhase];
        UpdatePhaseText();
    }

    private void UpdatePhaseText()
    {
        if (phaseText != null && currentPhase < phaseNames.Count)
        {
            phaseText.text = $"Faz -> {phaseNames[currentPhase]}";
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
            GameObject enemy = ObjectPoolSingleton.Instance.GetObject(key);
            if (enemy != null)
            {
                SetEnemyPosition(key, enemy);
            }
        }
    }

    private void SetEnemyPosition(string key, GameObject enemy)
    {
        float randomZ = Random.Range(-10f, 10f);
        Vector3 spawnPosition = key switch
        {
            "enemyT1" => new Vector3(randomZ, 0, SpawnPositionZ_T1),
            "enemyT2" => new Vector3(randomZ, 0, SpawnPositionZ_T2),
            "enemyT3" => new Vector3(randomZ, 0, SpawnPositionZ_T3),
            _ => enemy.transform.position
        };

        enemy.transform.position = spawnPosition;

        if (key == "enemyT1")
        {
            var enemyScript = enemy.GetComponent<EnemyType1>();
            enemyScript?.SetPosition(spawnPosition);
        }
    }
}
