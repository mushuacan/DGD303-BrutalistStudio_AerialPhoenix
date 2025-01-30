using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondution : MonoBehaviour
{
    public float moveSpeed = 2f;  // Hareket h�z� (0,0,0'a do�ru)
    public float rotationSpeed = 50f;  // D�n�� h�z�
    private Vector3 targetPosition = Vector3.zero;  // Hedef pozisyon (0,0,0)

    private Vector3 rotationAxis;  // D�n�� ekseni

    public GameObject Player;
    [SerializeField] private PlayMusic playMusic;
    public GameObject winMenu;

    private void Start()
    {
        // Ba�lang��ta rastgele bir d�n�� ekseni se�iyoruz
        rotationAxis = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    private void Update()
    {
        // Hedef pozisyona do�ru yava��a hareket et
        MoveTowardsTarget();

        // S�rekli d�nmesini sa�la, rastgele ama sabit bir y�ne do�ru
        RotateObject();
    }

    private void MoveTowardsTarget()
    {
        // Obje hedef pozisyona do�ru hareket ederken yava��a yakla�s�n
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private void RotateObject()
    {
        // Obje s�rekli d�ner, rastgele bir eksene g�re, fakat bu eksen sabit kal�r
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0.0f;
            playMusic.PlayVictoryMusic();
            winMenu.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
