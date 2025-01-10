using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float maxMoveSpeed;
    public float edgeForX;
    public float edgeForZ;

    [Header("References")]
    public Rigidbody rb;

    private Vector3 targetPosition;
    [SerializeField] private bool isMouseControlActive;

    private Vector3 movementInput;

    private void Start()
    {
        //isMouseControlActive = false;
    }

    void Update()
    {
        MouseMovementTargetPosition();
        SwitchController();
        CollectKeyboardInput(); // W, A, S, D hareket giri�lerini topla
    }

    private void FixedUpdate()
    {
        if (isMouseControlActive)
        {
            // Fare kontrol�
            MouseMovement();
        }
        else
        {
            // Klavye kontrol�
            MoveWithKeyboard();
        }
    }

    private void MouseMovementTargetPosition()
    {
        // Fare pozisyonunu al ve d�nya koordinatlar�na �evir
        Vector3 mousePos = Input.mousePosition;
        targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.transform.position.y));

        // S�n�rlar� uygula
        targetPosition.x = Mathf.Clamp(targetPosition.x, -edgeForX, edgeForX);
        targetPosition.z = Mathf.Clamp(targetPosition.z, -edgeForZ, edgeForZ);
    }

    private void MouseMovement()
    {
        // Hareket y�n�n� hesapla
        Vector3 direction = new Vector3(targetPosition.x, transform.position.y, targetPosition.z) - transform.position;

        if (direction.magnitude > 0.1f) // E�er hedefe yak�n de�ilse
        {
            // Hareketi Rigidbody ile uygula
            Vector3 moveDirection = direction.normalized * maxMoveSpeed * Time.fixedDeltaTime;
            Vector3 nextPosition = rb.position + moveDirection;

            // S�n�rlar� uygula
            nextPosition = ClampPosition(nextPosition);
            rb.MovePosition(nextPosition);
        }
    }


    private void CollectKeyboardInput()
    {
        // Hareket giri�lerini topla
        movementInput = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) movementInput.z += 1;
        if (Input.GetKey(KeyCode.S)) movementInput.z -= 1;
        if (Input.GetKey(KeyCode.D)) movementInput.x += 1;
        if (Input.GetKey(KeyCode.A)) movementInput.x -= 1;

        // Hareket y�n�n� normalize et (daha h�zl� gitmeyi engellemek i�in)
        if (movementInput.magnitude > 1)
        {
            movementInput.Normalize();
        }
    }

    private void MoveWithKeyboard()
    {
        // Hareket h�z�n� uygula
        Vector3 movement = movementInput * maxMoveSpeed * Time.fixedDeltaTime;

        // Yeni pozisyonu hesapla
        Vector3 nextPosition = rb.position + movement;

        // S�n�rlar� uygula
        nextPosition = ClampPosition(nextPosition);
        rb.MovePosition(nextPosition);
    }

    private Vector3 ClampPosition(Vector3 position)
    {
        // Pozisyonu s�n�rlar i�inde tut
        position.x = Mathf.Clamp(position.x, -edgeForX, edgeForX);
        position.z = Mathf.Clamp(position.z, -edgeForZ, edgeForZ);

        return position;
    }

    public void SwitchController()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isMouseControlActive = !isMouseControlActive;

            // Ge�i� s�ras�nda hareket giri�lerini s�f�rla
            movementInput = Vector3.zero;
        }
    }
}
