using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateWinCond : MonoBehaviour
{
    public GameObject winCond;
    public void Activate(Vector3 pozisyon)
    {
        winCond.SetActive(true);
        winCond.transform.position = pozisyon;
    }
}
