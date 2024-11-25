using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType3 : MonoBehaviour
{
    [SerializeField] private float enterSpeed;
    [SerializeField] private float spawnHeight;
    [SerializeField] private float neededHeight;
    [SerializeField] private float attackRate;
    private float attackTimer;
    [SerializeField] private float angryAttackRate;
    private int angerTimer;
    [SerializeField] private float lowHealthAttackRate;
    [SerializeField] private int angerThreshold;
    public GameObject player;
    [SerializeField] private string bulletNameKey;
    [SerializeField] private float angleBetweenBullets;
    [SerializeField] private float angerWarpSize;
    public CharacterState currentState;


    // Start is called before the first frame update
    void Start()
    {
        ResetSettings();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == CharacterState.Coming)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, (transform.position.z + (-1 * enterSpeed * Time.deltaTime)));
            if (transform.position.z <= neededHeight)
            {
                currentState = CharacterState.Idle;
            }
        }
        else if (currentState == CharacterState.Idle)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackRate)
            {
                Attack();
                attackTimer = 0;
                angerTimer++;
                if (angerTimer >= angerThreshold)
                {
                    currentState = CharacterState.Angry;
                    angerTimer = 0;
                }
            }
        }
        else if (currentState == CharacterState.Angry)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= angryAttackRate)
            {
                Attack(true); // 3 times maybe ?
                angerTimer++;
                attackTimer = 0;
                if (angerTimer >= angerThreshold)
                {
                    currentState = CharacterState.Idle;
                    angerTimer = 0;
                }
            }
        }
        else if (currentState == CharacterState.LowHealth)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= lowHealthAttackRate)
            {
                Attack(true); // 3 times maybe ?
                attackTimer = 0;
            }
        }
        else if (currentState == CharacterState.Dead)
        {
            Debug.Log("Bomber is dead");
        }
        else
        {
            Debug.LogWarning("EnemyType3'ün Enum'unda hata var.");
        }
    }

    public void StateChanger(CharacterState charState)
    {
        currentState = charState;
    }

    private void OnEnable()
    {
        ResetSettings();
    }
    private void ResetSettings()
    {
        transform.position = new Vector3(0, 0, spawnHeight);
        currentState = CharacterState.Coming;
        attackTimer = 0;
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Attack(bool angerWarp = false)
    {
        GameObject bullet1 = TakeBullet();
        GameObject bullet2 = TakeBullet();
        GameObject bullet3 = TakeBullet();

        Vector3 playerPosition = player.transform.position;

        if (angerWarp)
        {
            float angerWarpX = Random.Range(-angerWarpSize, angerWarpSize);
            float angerWarpZ = Random.Range(-angerWarpSize, angerWarpSize);
            playerPosition = playerPosition + new Vector3(angerWarpX, 0, angerWarpZ);
        }

        // Bullet2'yi yerleþtir
        bullet2.transform.position = transform.position + Vector3.forward;
        bullet2.transform.LookAt(playerPosition);
        bullet2.GetComponent<Bullet>().SetDirection(bullet2.transform.forward);

        // Bullet1'i Bullet2'nin soluna yerleþtir ve döndür
        Vector3 leftOffset = -bullet2.transform.right;
        bullet1.transform.position = bullet2.transform.position + leftOffset;
        bullet1.transform.rotation = bullet2.transform.rotation;
        bullet1.transform.Rotate(0, -angleBetweenBullets, 0);
        bullet1.GetComponent<Bullet>().SetDirection(bullet1.transform.forward);

        // Bullet3'ü Bullet2'nin saðýna yerleþtir ve döndür
        Vector3 rightOffset = bullet2.transform.right;
        bullet3.transform.position = bullet2.transform.position + rightOffset;
        bullet3.transform.rotation = bullet2.transform.rotation;
        bullet3.transform.Rotate(0, angleBetweenBullets, 0);
        bullet3.GetComponent<Bullet>().SetDirection(bullet3.transform.forward);

        // A little bit hard coded.
    }


    private GameObject TakeBullet()
    {
        GameObject bullet = ObjectPoolSingleton.Instance.GetObject(bulletNameKey);
        bullet.GetComponent<Bullet>().SetDirection(Vector3.back);
        bullet.transform.position = this.gameObject.transform.position;
        return bullet;
    }
}

public enum CharacterState
{
    Coming,
    Idle,
    Angry,
    LowHealth,
    Dead
}