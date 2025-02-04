// Optimized by ChatGPT Code Copilot under mushu's supervision

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType3 : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float enterSpeed;
    [SerializeField] private float spawnHeight;
    [SerializeField] private float neededHeight;

    [Header("Attack Settings")]
    [SerializeField] private float attackRate;
    [SerializeField] private float angryAttackRate;
    [SerializeField] private float lowHealthAttackRate;
    [SerializeField] private int angerThreshold;
    [SerializeField] private float angleBetweenBullets;
    [SerializeField] private float angerWarpSize;

    [Header("Bullet Settings")]
    [SerializeField] private string bulletNameKey;

    [Header("References")]
    public GameObject player;
    public AudioSource audioOfBoss;

    public CharacterState currentState;

    private float attackTimer;
    private int angerTimer;

    private void Start()
    {
        ResetSettings();
    }

    private void Update()
    {
        HandleStateLogic();
    }

    private void HandleStateLogic()
    {
        switch (currentState)
        {
            case CharacterState.Coming:
                HandleComingState();
                break;

            case CharacterState.Idle:
                HandleIdleState();
                break;

            case CharacterState.Angry:
                HandleAngryState();
                break;

            case CharacterState.LowHealth:
                HandleLowHealthState();
                break;

            case CharacterState.Dead:
                Debug.Log("Bomber is dead");
                break;

            default:
                Debug.LogWarning("Invalid state in EnemyType3");
                break;
        }
    }

    private void HandleComingState()
    {
        transform.position += Vector3.back * enterSpeed * Time.deltaTime;

        if (transform.position.z <= neededHeight)
        {
            currentState = CharacterState.Idle;
        }
    }

    private void HandleIdleState()
    {
        if (HandleAttack(attackRate))
        {
            angerTimer++;

            if (angerTimer >= angerThreshold)
            {
                currentState = CharacterState.Angry;
                angerTimer = 0;
            }
        }
    }

    private void HandleAngryState()
    {
        if (HandleAttack(angryAttackRate, true))
        {
            angerTimer++;

            if (angerTimer >= angerThreshold)
            {
                currentState = CharacterState.Idle;
                angerTimer = 0;
            }
        }
    }

    private void HandleLowHealthState()
    {
        HandleAttack(lowHealthAttackRate, true);
    }

    private bool HandleAttack(float rate, bool angerWarp = false)
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= rate)
        {
            attackTimer = 0;
            Attack(angerWarp);
            return true;
        }

        return false;
    }

    private void Attack(bool angerWarp = false)
    {
        var playerPosition = player.transform.position;

        if (angerWarp)
        {
            playerPosition += new Vector3(
                Random.Range(-angerWarpSize, angerWarpSize),
                0,
                Random.Range(-angerWarpSize, angerWarpSize)
            );
        }
        audioOfBoss.Play();
        SpawnBullet(Vector3.zero, 0, playerPosition); // Center bullet
        SpawnBullet(-Vector3.right, -angleBetweenBullets, playerPosition); // Left bullet
        SpawnBullet(Vector3.right, angleBetweenBullets, playerPosition); // Right bullet
    }

    private void SpawnBullet(Vector3 offset, float angleOffset, Vector3 targetPosition)
    {
        GameObject bullet = TakeBullet();
        bullet.transform.position = transform.position + offset;

        var direction = (targetPosition - bullet.transform.position).normalized;
        bullet.transform.rotation = Quaternion.LookRotation(direction);
        bullet.transform.Rotate(0, angleOffset, 0);

        bullet.GetComponent<Bullet>().SetDirection(bullet.transform.forward);
    }

    private GameObject TakeBullet()
    {
        var bullet = ObjectPoolSingleton.Instance.GetObject(bulletNameKey);
        bullet.transform.position = transform.position;
        bullet.GetComponent<Bullet>().SetDirection(Vector3.back);
        return bullet;
    }

    private void ResetSettings()
    {
        transform.position = new Vector3(0, 0, spawnHeight);
        currentState = CharacterState.Coming;
        attackTimer = 0;
        angerTimer = 0;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void StateChanger(CharacterState charState)
    {
        currentState = charState;
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
