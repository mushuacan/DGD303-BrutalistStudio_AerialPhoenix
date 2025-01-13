using UnityEngine;

public class EnemyType1_Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float verticalSpeed = 2f;           // Downward movement speed
    public float neededHeight = 5f;            // Height where enemy stops descending
    public float horizontalSpeed = 3f;         // Horizontal movement speed
    public float horizontalMoveDistance = 3f;  // Horizontal move range
    [SerializeField] private float edgeX = 13f;

    private Vector3 leftPosition;
    private Vector3 rightPosition;
    private bool movingRight = true;
    private bool movingDown = true;
    public GameObject belirtgi;
    private bool collisioned = false;

    private void Start()
    {
        InitializeSettings();
    }

    private void OnEnable()
    {
        InitializeSettings();
    }

    private void InitializeSettings()
    {
        collisioned = false;
        movingRight = true;
        movingDown = true;
        SetPosition(gameObject.transform.position);
    }

    private void Update()
    {
        if (movingDown)
        {
            MoveVertically();
        }
        else
        {
            HandleHorizontalMovement();
        }
    }

    private void MoveVertically()
    {
        transform.position -= Vector3.forward * verticalSpeed * Time.deltaTime;
        if (transform.position.z <= neededHeight)
        {
            movingDown = false;
        }
    }
    public void CreateBelirtgi()
    {
        //Instantiate(belirtgi, transform.position, Quaternion.identity);
    }

    private void HandleHorizontalMovement()
    {
        float movementDelta = horizontalSpeed * Time.deltaTime;

        if (movingRight)
        {
            transform.position += Vector3.right * movementDelta;
        }
        else
        {
            transform.position -= Vector3.right * movementDelta;
        }

        if (movingRight && transform.position.x >= rightPosition.x)
        {
            movingRight = false; CreateBelirtgi();
        }
        else if (!movingRight && transform.position.x <= leftPosition.x)
        {
            movingRight = true; CreateBelirtgi();
        }

        if (transform.position.x >= edgeX)
        {
            movingRight = false; CreateBelirtgi();
        }
        else if (transform.position.x <= -edgeX)
        {
            movingRight = true; CreateBelirtgi();
        }
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
        leftPosition = transform.position - new Vector3(horizontalMoveDistance, 0, 0);
        rightPosition = transform.position + new Vector3(horizontalMoveDistance, 0, 0);
    }

    #region Collider Interaction (Unchanged)
    private void OnTriggerEnter(Collider other)
    {
        string otherName = other.gameObject.name;
        if (collisioned == false && (otherName == "EnemyType1(Clone)" || otherName == "EnemyType3(Clone)"))
        {
            movingRight = !movingRight;
            collisioned = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        string objName = other.gameObject.name;
        if (objName == "EnemyType1(Clone)")
        {
            collisioned = true;
            if (other.transform.position.x < transform.position.x && transform.position.x > 0)
            {
                transform.position += new Vector3(0.1f, 0, 0);
            }
            else if (other.transform.position.x > transform.position.x && transform.position.x < 0)
            {
                transform.position -= new Vector3(0.1f, 0, 0);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (collisioned == true)
        {
            collisioned = false;
            SetPosition(transform.position);
        }
    }
    #endregion
}