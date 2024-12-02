using DG.Tweening;
using UnityEngine;

public class EnemyType2_Collision : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private float triggerDistance = 100f;
    [SerializeField] private float damageToPlayer;
    private float distanceToPlayer;
    private Tween delayedExplosionTween;

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

        // Mesafeyi hesapla
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Eðer oyuncu tetikleme mesafesindeyse
        if (distanceToPlayer < triggerDistance && delayedExplosionTween == null)
        {
            Debug.Log("Mesafe = " + distanceToPlayer);
            DelayedExplosion(1.5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = (PlayerHealth) other.gameObject.GetComponent<PlayerHealth>();
            playerHealth.DamagePlayer(damageToPlayer);
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

    private void DelayedExplosion(float delay)
    {
        Debug.Log("Geri sayým baþladý.");
        delayedExplosionTween = DOVirtual.DelayedCall(delay, ExplodeYourself);
    }
}
