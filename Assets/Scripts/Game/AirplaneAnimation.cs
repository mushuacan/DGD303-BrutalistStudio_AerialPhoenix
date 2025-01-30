using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneAnimation : MonoBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private float katsayi;
    [SerializeField] private float yumuþatmaHýzý;

    public void MoveDirection(Vector3 direction)
    {
        Quaternion hedefRotasyon = Quaternion.Euler(direction.z * katsayi, 0, direction.x * -katsayi);
        model.transform.rotation = Quaternion.Slerp(model.transform.rotation, hedefRotasyon, Time.deltaTime * yumuþatmaHýzý);

    }
}
