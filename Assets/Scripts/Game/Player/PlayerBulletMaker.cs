using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletMaker : MonoBehaviour
{
    [SerializeField] private MultiObjectPool objectPool;
    [SerializeField] private AudioSource bulletSound;

    [SerializeField] private float FireTime;
    [SerializeField] private float deviation;
    [SerializeField] private string bulletType;


    private float timer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > FireTime)
        {
            GameObject bullet = objectPool.GetObject(bulletType, transform.position);
            bullet.transform.position += Vector3.forward;
            Bullet bulletcs = bullet.GetComponent<Bullet>();
            //bulletcs.SetDirection(Vector3.forward);

            // Yönü belirlerken sapma ekleyelim
            Vector3 randomDeviation = new Vector3(Random.Range(-deviation, deviation), 0, 0);

            Vector3 finalDirection = (Vector3.forward + randomDeviation).normalized;

            bulletcs.SetDirection(finalDirection);



            bullet.tag = "Bullet_Player";


            timer = 0;
            if (bulletSound != null && bulletSound.enabled)
            {
                bulletSound.Play();
            }
        }
    }
}
