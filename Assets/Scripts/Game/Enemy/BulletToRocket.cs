using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletToRocket : MonoBehaviour
{
    public GameObject bullet;
    public GameObject rocket;
    private void OnEnable()
    {
        GameObject character = GameObject.Find("EnemyType3(Clone)");
        if (character == null)
            return;
        else
        {
            bullet.SetActive(false);
            rocket.SetActive(true);
        }
    }
}
