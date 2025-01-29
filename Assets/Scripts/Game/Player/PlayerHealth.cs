using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float playerHealth = 100f;
    public float maxPlayerHealth;
    public TextMeshProUGUI textMeshProUGUI;
    public Slider healthSlider;
    public GameObject deathMenu;
    [SerializeField] private float collsionDamage;
    [SerializeField] private bool isDeadable;
    [SerializeField] private ESC_Menu escapeMenu;

    // Start is called before the first frame update
    void Start()
    {
        textMeshProUGUI.text = "Can: " + playerHealth;
        healthSlider.value = playerHealth;
        deathMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamagePlayer(float damage)
    {
        playerHealth -= damage;

        textMeshProUGUI.text = "Can: " + playerHealth;
        healthSlider.value = playerHealth;

        if (playerHealth <= 0 && isDeadable)
        {
            Time.timeScale = 0.0f;
            escapeMenu.ChangeMenuAbleity(false);
            deathMenu.SetActive(true);
        }
    }

    public void HealPlayer(float heal)
    {
        playerHealth += heal;
        if (playerHealth >= maxPlayerHealth)
        {
            playerHealth = maxPlayerHealth;
        }
        textMeshProUGUI.text = "Can: " + playerHealth;
        healthSlider.value = playerHealth;
    }
    private void OnTriggerStay(Collider other)
    {

        EnemyHealth enemyHeath = other.gameObject.GetComponent<EnemyHealth>();
        if (enemyHeath != null)
        {
            enemyHeath.GiveDamage(collsionDamage);
            DamagePlayer(collsionDamage);
        }
        Debug.Log("Karþýlaþýldý. Ad " + other.gameObject.name + ", ayrýca " + enemyHeath != null);
    }

}
