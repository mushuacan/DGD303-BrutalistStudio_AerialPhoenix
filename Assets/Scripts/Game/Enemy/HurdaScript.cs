using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurdaScript : MonoBehaviour
{
    [SerializeField] private float bounds;
    [SerializeField] private float speed;

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + new Vector3(0, 0, -1 * speed * Time.deltaTime);
        //transform.rotation = Quaternion.Euler(10, -35, -35);
        StayInStage();
    }

    private void StayInStage()
    {
        if (Mathf.Abs(transform.position.z) >= bounds
            || Mathf.Abs(transform.position.y) >= bounds
            || Mathf.Abs(transform.position.x) >= bounds)
        {
            ObjectPoolSingleton.Instance.ReturnObject("hurda", this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            playerHealth.HealPlayer(7);
            ObjectPoolSingleton.Instance.ReturnObject("hurda", this.gameObject);
        }
    }
}
