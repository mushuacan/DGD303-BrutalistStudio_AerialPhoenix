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
        [Tooltip("Havuz elemanlarý hakkýnda bilgi notu.")]
        [SerializeField] private string comment;
    }

    [Header("Parent Object for Pool Items")]
    [SerializeField] private Transform parentTransform; // Objelerin ekleneceði parent objesi

    [SerializeField] private List<PoolItem> poolItems;

    // Havuz için dictionary (key: prefab tipi, value: prefab havuzu)
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        // Eðer parentTransform atanmadýysa otomatik oluþtur
        if (parentTransform == null)
        {
            GameObject poolParent = new GameObject("ObjectPoolParent");
            parentTransform = poolParent.transform;
        }

        // Havuz dictionary'sini baþlat
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var item in poolItems)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            // Havuz elemanlarýný oluþtur ve sýraya ekle
            for (int i = 0; i < item.initialSize; i++)
            {
                GameObject obj = Instantiate(item.prefab, parentTransform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(item.key, objectPool);
        }
    }

    public GameObject GetObject(string key, Vector3 position = default, Quaternion rotation = default)
    {
        if (!poolDictionary.ContainsKey(key))
        {
            Debug.LogWarning($"Havuzda {key} anahtarýyla eþleþen bir nesne yok.");
            return null;
        }

        GameObject obj;

        if (poolDictionary[key].Count > 0)
        {
            obj = poolDictionary[key].Dequeue();
        }
        else
        {
            // Eðer havuzda nesne kalmamýþsa yeni bir tane oluþtur
            PoolItem poolItem = poolItems.Find(item => item.key == key);
            if (poolItem != null)
            {
                obj = Instantiate(poolItem.prefab, parentTransform);
            }
            else
            {
                Debug.LogError($"Havuzda {key} anahtarýyla eþleþen prefab bulunamadý.");
                return null;
            }
        }

        obj.SetActive(true);

        // Varsayýlan pozisyon ve rotasyon
        obj.transform.position = position == default ? Vector3.zero : position;
        obj.transform.rotation = rotation == default ? Quaternion.identity : rotation;

        return obj;
    }

    public void ReturnObject(string key, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(key))
        {
            Debug.LogError($"Havuzda {key} anahtarýyla eþleþen bir nesne yok.");
            Destroy(obj); // Eðer geçersiz bir nesne geldiyse doðrudan yok et
            return;
        }

        obj.SetActive(false);
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        poolDictionary[key].Enqueue(obj);
    }

    public int GetPoolSize(string key)
    {
        if (poolDictionary.ContainsKey(key))
        {
            return poolDictionary[key].Count;
        }

        Debug.LogWarning($"Havuzda {key} anahtarýyla eþleþen bir nesne yok.");
        return 0;
    }
}
