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
            Debug.LogError("Player objesi bulunamadý! Patlama tetiklenemeyebilir.");
        }
    }

    private void Update()
    {
        if (player == null) return; // Player eksikse iþlem yapma

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
            // Diðer düþmanlarla çarpýþma için özel iþlem
        }
    }

    public void ExplodeYourself()
    {
        // Tweeni durdurmadan önce kontrol
        if (delayedExplosionTween != null)
        {
            delayedExplosionTween.Kill();
        }
        ObjectPoolSingleton.Instance.ReturnObject("enemyT2", this.gameObject);
        
        Debug.Log("Patladý");
    }

    public void ExplodeByExternalFactors()
    {
        Debug.Log("Dýþ etmenler tarafýndan patlatýldý");

        if (delayedExplosionTween != null)
        {
            delayedExplosionTween.Kill();
        }

        ObjectPoolSingleton.Instance.ReturnObject("enemyT2", this.gameObject);
    }
}
