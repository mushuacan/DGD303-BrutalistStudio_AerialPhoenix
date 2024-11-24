using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.EditorTools;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float initialHealth;
    private float health;
    [SerializeField] private string thisObjectKey;
    public TextMeshProUGUI textMeshProUGUI;

    private void Start()
    {
        ResetSettings();
    }
    private void OnEnable()
    {
        ResetSettings();
    }

    private void ResetSettings()
    {
        health = initialHealth;
        textMeshProUGUI.text = "Can: " + health;
    }


    public void GiveDamage(float damage)
    {
        health -= damage;
        textMeshProUGUI.text = "Can: " + health;
        CheckHealth();
    }

    public void CheckHealth()
    {
        if (health <= 4.44)
        {
            ObjectPoolSingleton.Instance.ReturnObject(thisObjectKey, this.gameObject);
        }
    }
}
