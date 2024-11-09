using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float maxMoveSpeed;  // Hareket hýzý
    public float speedHorizontal;
    public float speedVertical;

    private Rigidbody rb;         // Rigidbody referansý

    private void Start()
    {
        rb = GetComponent<Rigidbody>();  // Rigidbody bileþenini al
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.D))
        {
            speedHorizontal = (maxMoveSpeed - speedHorizontal) * 0.8f;
            Debug.Log("D tuþu basýlý ve hýz -> " + speedHorizontal);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            speedHorizontal = ((-1 * maxMoveSpeed) - speedHorizontal) * 0.8f;
            Debug.Log("A tuþu basýlý ve hýz -> " + speedHorizontal);
        }
        else if (speedHorizontal != 0)
        {
            speedHorizontal = speedHorizontal * 0.8f;
            if (Mathf.Abs(speedHorizontal) <= 0.5f)
            {
                speedHorizontal = 0;
            }
        }

        rb.MovePosition(new Vector3(rb.position.x + speedHorizontal * Time.fixedDeltaTime, rb.position.y, rb.position.z));

        if (Input.GetKey(KeyCode.W))
        {
            speedVertical = (maxMoveSpeed - speedVertical) * 0.8f;
            Debug.Log("D tuþu basýlý ve hýz -> " + speedVertical);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            speedVertical = ((-1 * maxMoveSpeed) - speedVertical) * 0.8f;
            Debug.Log("A tuþu basýlý ve hýz -> " + speedVertical);
        }
        else if (speedVertical != 0)
        {
            speedVertical = speedVertical * 0.8f;
            if (Mathf.Abs(speedVertical) <= 0.5f)
            {
                speedVertical = 0;
            }
        }

        rb.MovePosition(new Vector3(rb.position.x, rb.position.y, rb.position.z + speedVertical * Time.fixedDeltaTime));

    }
}
