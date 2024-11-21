using System.Collections.Generic;
using UnityEngine;

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
        [Tooltip("Havuz elemanlar� hakk�nda bilgi notu.")]
        [SerializeField] private string comment;
    }

    [Header("Parent Object for Pool Items")]
    [SerializeField] private Transform parentTransform; // Objelerin eklenece�i parent objesi

    [SerializeField] private List<PoolItem> poolItems;

    // Havuz i�in dictionary (key: prefab tipi, value: prefab havuzu)
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        // E�er parentTransform atanmad�ysa otomatik olu�tur
        if (parentTransform == null)
        {
            GameObject poolParent = new GameObject("ObjectPoolParent");
            parentTransform = poolParent.transform;
        }

        // Havuz dictionary'sini ba�lat
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var item in poolItems)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            // Havuz elemanlar�n� olu�tur ve s�raya ekle
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
            Debug.LogWarning($"Havuzda {key} anahtar�yla e�le�en bir nesne yok.");
            return null;
        }

        GameObject obj;

        if (poolDictionary[key].Count > 0)
        {
            obj = poolDictionary[key].Dequeue();
        }
        else
        {
            // E�er havuzda nesne kalmam��sa yeni bir tane olu�tur
            PoolItem poolItem = poolItems.Find(item => item.key == key);
            if (poolItem != null)
            {
                obj = Instantiate(poolItem.prefab, parentTransform);
            }
            else
            {
                Debug.LogError($"Havuzda {key} anahtar�yla e�le�en prefab bulunamad�.");
                return null;
            }
        }

        obj.SetActive(true);

        // Varsay�lan pozisyon ve rotasyon
        obj.transform.position = position == default ? Vector3.zero : position;
        obj.transform.rotation = rotation == default ? Quaternion.identity : rotation;

        return obj;
    }

    public void ReturnObject(string key, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(key))
        {
            Debug.LogError($"Havuzda {key} anahtar�yla e�le�en bir nesne yok.");
            Destroy(obj); // E�er ge�ersiz bir nesne geldiyse do�rudan yok et
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

        Debug.LogWarning($"Havuzda {key} anahtar�yla e�le�en bir nesne yok.");
        return 0;
    }
}
