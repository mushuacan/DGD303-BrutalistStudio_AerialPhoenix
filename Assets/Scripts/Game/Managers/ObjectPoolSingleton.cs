using UnityEngine;

public class ObjectPoolSingleton : MonoBehaviour
{
    [SerializeField] private MultiObjectPool objectPool;
    public static ObjectPoolSingleton Instance { get; private set; }

    public static int enemyDiedCount;
    public static int totalEnemySpawned;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // �lk �rne�i atar
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // �kincil �rne�i yok eder
        }
    }

    private void Start()
    {
        ResetCounts();
    }

    public GameObject GetObject(string objectType)
    {
        if (objectType == "enemyT1" || objectType == "enemyT2" || objectType == "enemyT3")
            IncreaseEnemyCount(1);
        return objectPool.GetObject(objectType); // Obje d�nd�r
    }

    public void ReturnObject(string objectType, GameObject obj)
    {
        if (objectType == "enemyT1" || objectType == "enemyT2" || objectType == "enemyT3")
            DecreaseEnemyCount(1);
        objectPool.ReturnObject(objectType, obj);
    }

    public void IncreaseEnemyCount(int count)
    {
        totalEnemySpawned += count;
    }
    public void DecreaseEnemyCount(int count)
    {
        enemyDiedCount += count;
    }

    /// <summary>
    /// Giri�ler: 2-enemyDiedCount 3-totalEnemySpawned
    /// </summary>
    /// <param name="info">Eklemek istedi�iniz say� de�eri.</param>
    public int GetEnemyCountInfo(int info)
    {
        if (info == 2)
        return enemyDiedCount;
        else if (info == 3)
        return totalEnemySpawned;
        else return 0;
    }

    public void ResetCounts()
    {
        enemyDiedCount = 0;
        totalEnemySpawned = 0;
    }
}
