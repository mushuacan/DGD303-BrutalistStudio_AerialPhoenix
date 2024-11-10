using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float maxMoveSpeed;
    public float speedHorizontal;
    public float speedVertical;

    [Header("References")]
    public Rigidbody rb;

    private Vector3 targetPosition;
    private bool isMouseControlActive;

    private void Start()
    {
        //rb = GetComponent<Rigidbody>();
        isMouseControlActive = false;
    }

    void Update()
    {
        MouseMovementTargetPosition();
        SwitchController();
    }

    private void FixedUpdate()
    {
        if (isMouseControlActive)
        {
            //Mouse Controls
            MouseMovement();
        }
        else
        {
            //Keyboard Controls
            MoveSpeedHorizontal();
            MoveSpeedVertical();


            rb.MovePosition(new Vector3(rb.position.x + speedHorizontal * Time.fixedDeltaTime, rb.position.y, rb.position.z + speedVertical * Time.fixedDeltaTime));
        }
    }


    public void SwitchController()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isMouseControlActive = !isMouseControlActive;

            speedHorizontal = 0;
            speedVertical = 0;
        }
    }

    #region Mouse Controller Functions
    //This function works on Update
    private void MouseMovementTargetPosition()
    {
        // Fare pozisyonunu al ve dünya koordinatlarýna çevir
        Vector3 mousePos = Input.mousePosition;
        targetPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.transform.position.y));
    }

    //This function works on fixedUpdate
    private void MouseMovement()
    {
        // Hareket yönünü hesapla (x ve z düzlemi üzerinde)
        Vector3 direction = new Vector3(targetPosition.x, transform.position.y, targetPosition.z) - transform.position;

        // Karakteri fare yönüne doðru döndür ve hareket ettir
        if (direction.magnitude > 0.1f) // Eðer hedefe yakýn deðilse
        {
            Quaternion toRotation = Quaternion.LookRotation(direction); // Hedef rotasyon
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, toRotation, Time.deltaTime * maxMoveSpeed)); // Dönüþü yumuþat

            // Hareketi Rigidbody ile uygula
            Vector3 moveDirection = direction.normalized * maxMoveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + moveDirection);
        }
    }
    #endregion

    #region Keyboard Control Functions
    private void MoveSpeedHorizontal()
    {
        if (Input.GetKey(KeyCode.D))
        {
            speedHorizontal = CalculateMovementSpeed(speedHorizontal, maxMoveSpeed, "D");
        }
        else if (Input.GetKey(KeyCode.A))
        {
            speedHorizontal = CalculateMovementSpeed(speedHorizontal, -maxMoveSpeed, "A");
        }
        else if (speedHorizontal != 0)
        {
            speedHorizontal = CalculateStoppingSpeed(speedHorizontal);
        }
    }

    private void MoveSpeedVertical()
    {
        if (Input.GetKey(KeyCode.W))
        {
            speedVertical = CalculateMovementSpeed(speedVertical, maxMoveSpeed, "W");
        }
        else if (Input.GetKey(KeyCode.S))
        {
            speedVertical = CalculateMovementSpeed(speedVertical, -maxMoveSpeed, "S");
        }
        else if (speedVertical != 0)
        {
            speedVertical = CalculateStoppingSpeed(speedVertical);
        }
    }

    private float CalculateMovementSpeed(float speed, float maxSpeed, string pushedKey)
    {

        if (speed == maxSpeed)
        {
            return speed;
        }

        speed += (maxSpeed - speed) * 0.2f + maxSpeed * 0.2f;
        if (Mathf.Abs(speed) > Mathf.Abs(maxSpeed))
        {
            speed = maxSpeed;
        }

        Debug.Log(pushedKey + " tuþu basýlý ve hýz -> " + speed);

        return speed;
    }

    private float CalculateStoppingSpeed(float speed)
    {
        if (Mathf.Abs(speed) <= 0.5f)
        {
            speed = 0;
            return speed;
        }

        speed = speed * 0.8f;

        Debug.Log("Bir tuþa basýlý deðil ve hýz -> " + speedHorizontal);

        return speed;
    }
    #endregion
}
