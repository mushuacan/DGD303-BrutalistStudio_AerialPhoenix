using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MultiObjectPool objectPool;

    void Start()
    {
        Spawner("enemyT2", 3);
    }


    public void Spawner(string key, int howMany = 1)
    {
        for (int i = 0; i < howMany; i++)
        {
            GameObject obje = objectPool.GetObject(key);
            if (obje != null)
            {
                obje.transform.position = new Vector3(0, 0, 0);
            }
        }
    }
}
