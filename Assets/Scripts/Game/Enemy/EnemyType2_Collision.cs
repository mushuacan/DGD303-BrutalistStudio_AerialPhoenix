using DG.Tweening;
using UnityEngine;

public class EnemyType2_Collision : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private float damageToPlayer;
    private Tween delayedExplosionTween;
    public GameObject explosionPrefab1;
    public GameObject explosionPrefab2;

    private void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player objesi bulunamad�! Patlama tetiklenemeyebilir.");
        }
    }

    private void Update()
    {
        if (player == null) return; // Player eksikse i�lem yapma

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = (PlayerHealth) other.gameObject.GetComponent<PlayerHealth>();
            playerHealth.DamagePlayer(damageToPlayer);
            Instantiate(explosionPrefab1, transform.position, Quaternion.identity);
            Instantiate(explosionPrefab2, transform.position, Quaternion.identity);
            ExplodeYourself();
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            // Di�er d��manlarla �arp��ma i�in �zel i�lem
        }
    }

    public void ExplodeYourself()
    {
        // Tweeni durdurmadan �nce kontrol
        if (delayedExplosionTween != null)
        {
            delayedExplosionTween.Kill();
        }
        ObjectPoolSingleton.Instance.ReturnObject("enemyT2", this.gameObject);
        
        Debug.Log("Patlad�");
    }

    public void ExplodeByExternalFactors()
    {
        Debug.Log("D�� etmenler taraf�ndan patlat�ld�");

        if (delayedExplosionTween != null)
        {
            delayedExplosionTween.Kill();
        }

        ObjectPoolSingleton.Instance.ReturnObject("enemyT2", this.gameObject);
    }
}
