using UnityEngine;

public class ObjectPoolSingleton : MonoBehaviour
{
    [SerializeField] private MultiObjectPool objectPool;
    public static ObjectPoolSingleton Instance { get; private set; }

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

    public GameObject GetObject(string objectType)
    {
        return objectPool.GetObject(objectType); // Obje d�nd�r
    }

    public void ReturnObject(string objectType, GameObject obj)
    {
        objectPool.ReturnObject(objectType, obj);
    }
}
