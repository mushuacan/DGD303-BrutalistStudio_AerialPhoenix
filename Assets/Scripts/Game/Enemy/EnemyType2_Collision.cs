using DG.Tweening;
using UnityEngine;

public class EnemyType2_Collision : MonoBehaviour
{
    private GameObject player;
    private MultiObjectPool poolManager;
    [SerializeField] private float triggerDistance = 100f;
    private float distanceToPlayer;
    private Tween delayedExplosionTween;

    private void Start()
    {
        poolManager = FindObjectOfType<MultiObjectPool>(); // Havuz yöneticisini bul
        if (poolManager == null)
        {
            Debug.LogError("MultiObjectPool bulunamadý! Havuz sistemi düzgün çalýþmayabilir.");
        }

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

        if (poolManager != null)
        {
            poolManager.ReturnObject("enemyT2", this.gameObject);
        }
        else
        {
            Debug.LogError("PoolManager bulunamadý. Obje yok ediliyor.");
            Destroy(gameObject); // Havuz sistemi yoksa objeyi yok et
        }

        Debug.Log("Patladý");
    }

    public void ExplodeByExternalFactors()
    {
        Debug.Log("Dýþ etmenler tarafýndan patlatýldý");

        if (delayedExplosionTween != null)
        {
            delayedExplosionTween.Kill();
        }

        if (poolManager != null)
        {
            poolManager.ReturnObject("enemyT2", gameObject);
        }
        else
        {
            Debug.LogError("PoolManager bulunamadý. Obje yok ediliyor.");
            Destroy(gameObject);
        }
    }

    private void DelayedExplosion(float delay)
    {
        Debug.Log("Geri sayým baþladý.");
        delayedExplosionTween = DOVirtual.DelayedCall(delay, ExplodeYourself);
    }
}
