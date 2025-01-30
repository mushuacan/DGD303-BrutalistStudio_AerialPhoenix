using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsJokeByMushu : MonoBehaviour
{
    public bool state;
    public GameObject obje;

    // Start is called before the first frame update
    void Start()
    {
        state = false;
    }

    public void ChangeState()
    {
        state = !state;
        obje.SetActive(state);
    }
}
