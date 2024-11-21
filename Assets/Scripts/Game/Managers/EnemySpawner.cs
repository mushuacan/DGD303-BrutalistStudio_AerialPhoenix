using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MultiObjectPool objectPool;
    private float timer;

    void Start()
    {
        DOVirtual.DelayedCall(1, SpawnThreeDrone);
    }

    private void SpawnThreeDrone()
    {
        Spawner("enemyT2", 3);
        DOVirtual.DelayedCall(4, SpawnTwoEnemyT1);
    }
    private void SpawnTwoEnemyT1()
    {
        Spawner("enemyT1", 3);
        DOVirtual.DelayedCall(3, SpawnOneDrone);
    }
    private void SpawnOneDrone()
    {
        Spawner("enemyT2", 1);
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

    //private void Update()
    //{
    //    timer += Time.deltaTime;
    //    if(timer > 1)
    //    {
    //        Spawner("enemyT2", 1);
    //        timer = 0;
    //    }
    //}
}
