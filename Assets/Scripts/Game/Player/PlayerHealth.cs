using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float playerHealth = 100f;
    public TextMeshProUGUI textMeshProUGUI;

    // Start is called before the first frame update
    void Start()
    {
        textMeshProUGUI.text = "Can: " + playerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamagePlayer(float damage)
    {
        playerHealth -= damage;

        textMeshProUGUI.text = "Can: " + playerHealth;

        if (playerHealth <= 0)
        {
            // End Game
        }
    }
}
