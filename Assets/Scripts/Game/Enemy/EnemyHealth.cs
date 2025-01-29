using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float initialHealth;
    [SerializeField] private float healthUnderDeath = 4.44f;
    [SerializeField] private float bomberLowHealth;
    private EnemyType3 enemyType3;
    private float health;
    [SerializeField] private string thisObjectKey;
    public TextMeshProUGUI textMeshProUGUI;
    public GameObject explosionPrefab;

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
        if (textMeshProUGUI != null) 
        textMeshProUGUI.text = "Can: " + health;
        if (thisObjectKey == "enemyT3")
        {
            enemyType3 = gameObject.GetComponent<EnemyType3>();
        }
    }


    public void GiveDamage(float damage)
    {
        health -= damage;
        if (textMeshProUGUI != null)
        textMeshProUGUI.text = "Can: " + health;
        CheckHealth();
    }

    public void CheckHealth()
    {
        if (health <= healthUnderDeath)
        {
            Die();
        }
        if (thisObjectKey == "enemyT3")
        {
            if (health <= bomberLowHealth)
            {
                if (enemyType3 != null)
                {
                    enemyType3.StateChanger(CharacterState.LowHealth);
                }
                else { Debug.LogError("EnemyType3 script null"); }
            }
        }
    }

    public void Die()
    {
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        GameObject obje = ObjectPoolSingleton.Instance.GetObject("hurda");
        obje.transform.position = this.transform.position;
        ObjectPoolSingleton.Instance.ReturnObject(thisObjectKey, this.gameObject);
    }
}
