using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MultiObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class PoolItem
    {
        [Tooltip("Nesne t�r�n� tan�mlayan anahtar.")]
        public string key;          
        [Tooltip("Prefab referans�")]
        public GameObject prefab;   
        [Tooltip("Ba�lang�� havuz b�y�kl���")]
        public int initialSize = 10;
        [SerializeField] private string comment;
    }

    [Header("Parent Object for Pool Items")]
    [SerializeField] private Transform parentTransform;  // Objelerin eklenece�i parent objesi

    [SerializeField] private List<PoolItem> poolItems;



    // Her bir t�r i�in ayr� bir havuz dictionary'si
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var item in poolItems)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            // Her prefab i�in belirli say�da nesne olu�turuluyor
            for (int i = 0; i < item.initialSize; i++)
            {
                GameObject obj = Instantiate(item.prefab, parentTransform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(item.key, objectPool);
        }
    }

    // Havuzdan nesne al (anahtarla nesne tipini belirliyoruz)
    public GameObject GetObject(string key, Vector3 pozition = default)
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
            // Havuzda nesne kalmam��sa yeni bir tane olu�tur
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

        if (pozition == default) { pozition = Vector3.zero; }
        obj.transform.position = pozition;
        
        return obj;
    }

    // Nesneyi havuza geri b�rak
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
