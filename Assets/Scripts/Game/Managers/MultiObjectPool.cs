using System.Collections.Generic;
using UnityEngine;

public class MultiObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class PoolItem
    {
        [Tooltip("Nesne türünü tanýmlayan anahtar.")]
        public string key;          
        [Tooltip("Prefab referansý")]
        public GameObject prefab;   
        [Tooltip("Baþlangýç havuz büyüklüðü")]
        public int initialSize = 10; 
    }

    [SerializeField] private List<PoolItem> poolItems;

    // Her bir tür için ayrý bir havuz dictionary'si
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var item in poolItems)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            // Her prefab için belirli sayýda nesne oluþturuluyor
            for (int i = 0; i < item.initialSize; i++)
            {
                GameObject obj = Instantiate(item.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(item.key, objectPool);
        }
    }

    // Havuzdan nesne al (anahtarla nesne tipini belirliyoruz)
    public GameObject GetObject(string key)
    {
        if (!poolDictionary.ContainsKey(key))
        {
            Debug.LogWarning($"Pool with key {key} does not exist.");
            return null;
        }

        GameObject obj;
        if (poolDictionary[key].Count > 0)
        {
            obj = poolDictionary[key].Dequeue();
        }
        else
        {
            // Havuzda nesne kalmamýþsa yeni bir tane oluþtur
            PoolItem poolItem = poolItems.Find(item => item.key == key);
            if (poolItem != null)
            {
                obj = Instantiate(poolItem.prefab);
            }
            else
            {
                Debug.LogError($"Prefab for key {key} not found in pool items.");
                return null;
            }
        }

        obj.SetActive(true);
        return obj;
    }

    // Nesneyi havuza geri býrak
    public void ReturnObject(string key, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(key))
        {
            Debug.LogWarning($"Pool with key {key} does not exist.");
            return;
        }

        obj.SetActive(false);
        poolDictionary[key].Enqueue(obj);
    }
}
