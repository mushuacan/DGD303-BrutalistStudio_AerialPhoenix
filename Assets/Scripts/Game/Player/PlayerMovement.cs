using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float maxMoveSpeed;
    public float speedHorizontal;
    public float speedVertical;
    private Vector3 targetPosition;

    private bool isMouseOn;

    public Rigidbody rb;

    private void Start()
    {
        //rb = GetComponent<Rigidbody>();
        isMouseOn = false;
    }

    void Update()
    {
        MouseMovementTargetPosition();
        SwitchController();
    }

    private void FixedUpdate()
    {
        if (isMouseOn)
        {
            //Mouse Controls
            MouseMovement();
        }
        else
        {
            //Keyboard Controls
            MoveSpeedHorizontal();
            MoveSpeedVertical();
        }
    }


    public void SwitchController()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isMouseOn = !isMouseOn;

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
            speedHorizontal = MovementFormula(speedHorizontal, maxMoveSpeed, "D");
        }
        else if (Input.GetKey(KeyCode.A))
        {
            speedHorizontal = MovementFormula(speedHorizontal, -maxMoveSpeed, "A");
        }
        else if (speedHorizontal != 0)
        {
            speedHorizontal = StoppingFormula(speedHorizontal);
        }

        rb.MovePosition(new Vector3(rb.position.x + speedHorizontal * Time.fixedDeltaTime, rb.position.y, rb.position.z));
    }

    private void MoveSpeedVertical()
    {
        if (Input.GetKey(KeyCode.W))
        {
            speedVertical = MovementFormula(speedVertical, maxMoveSpeed, "W");
        }
        else if (Input.GetKey(KeyCode.S))
        {
            speedVertical = MovementFormula(speedVertical, -maxMoveSpeed, "S");
        }
        else if (speedVertical != 0)
        {
            speedVertical = StoppingFormula(speedVertical);
        }
        rb.MovePosition(new Vector3(rb.position.x, rb.position.y, rb.position.z + speedVertical * Time.fixedDeltaTime));
    }

    private float MovementFormula(float speed, float maxSpeed, string pushedKey)
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

    private float StoppingFormula(float speed)
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
