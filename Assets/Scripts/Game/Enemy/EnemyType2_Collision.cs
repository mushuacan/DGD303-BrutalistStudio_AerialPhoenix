
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyType2_Collision : MonoBehaviour
{
    private GameObject player;
    private MultiObjectPool poolManager;
    [SerializeField] private float triggerDistance;
    private float distanceToPlayer;
    private Tween delayedExplosionTween;


    private void Start()
    {
        poolManager = FindObjectOfType<MultiObjectPool>();  // Havuz yöneticisini bul
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        // Mesafeyi hesapla
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Eðer oyuncu 100 birimden daha yakýnsa
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

        }
    }

    private void ExplodeYourself()
    {
        delayedExplosionTween.Kill();

        poolManager.ReturnObject("enemyT2", gameObject);

        Debug.Log("Patladý");
    }

    public void ExplodeByExternalFactors()
    {
        Debug.Log("Dýþ etmenler tarafýndan patlatýldý");

        delayedExplosionTween.Kill();

        poolManager = FindObjectOfType<MultiObjectPool>();  // Havuz yöneticisini bul
        poolManager.ReturnObject("enemyT2", gameObject);
    }

    private void DelayedExplosion(float delay)
    {
        Debug.Log("Geri sayým");
        delayedExplosionTween = DOVirtual.DelayedCall(delay, () =>
        {
            ExplodeYourself();
        });
    }
}
