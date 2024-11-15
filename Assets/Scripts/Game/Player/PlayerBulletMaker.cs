using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletMaker : MonoBehaviour
{
    [SerializeField] private MultiObjectPool objectPool;

    [SerializeField] private float FireTime;

    private string bulletType;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        bulletType = "bullet_pT1";
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > FireTime)
        {
            GameObject bullet = objectPool.GetObject(bulletType, transform.position);
            bullet.transform.position += Vector3.forward;
            bullet.GetComponent<Bullet>().SetDirection(Vector3.forward);
            bullet.tag = "Bullet_Player";

            timer = 0;
        }
    }
}
