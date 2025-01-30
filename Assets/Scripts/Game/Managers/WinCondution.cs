using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondution : MonoBehaviour
{
    public float moveSpeed = 2f;  // Hareket hýzý (0,0,0'a doðru)
    public float rotationSpeed = 50f;  // Dönüþ hýzý
    private Vector3 targetPosition = Vector3.zero;  // Hedef pozisyon (0,0,0)

    private Vector3 rotationAxis;  // Dönüþ ekseni

    public GameObject Player;
    [SerializeField] private PlayMusic playMusic;
    public GameObject winMenu;

    private void Start()
    {
        // Baþlangýçta rastgele bir dönüþ ekseni seçiyoruz
        rotationAxis = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    private void Update()
    {
        // Hedef pozisyona doðru yavaþça hareket et
        MoveTowardsTarget();

        // Sürekli dönmesini saðla, rastgele ama sabit bir yöne doðru
        RotateObject();
    }

    private void MoveTowardsTarget()
    {
        // Obje hedef pozisyona doðru hareket ederken yavaþça yaklaþsýn
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private void RotateObject()
    {
        // Obje sürekli döner, rastgele bir eksene göre, fakat bu eksen sabit kalýr
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
