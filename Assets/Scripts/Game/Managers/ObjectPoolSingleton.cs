using UnityEngine;

public class ObjectPoolSingleton : MonoBehaviour
{
    [SerializeField] private MultiObjectPool objectPool;
    public static ObjectPoolSingleton Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Ýlk örneði atar
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Ýkincil örneði yok eder
        }
    }

    public GameObject GetObject(string objectType)
    {
        return objectPool.GetObject(objectType); // Obje döndür
    }

    public void ReturnObject(string objectType, GameObject obj)
    {
        objectPool.ReturnObject(objectType, obj);
    }
}
