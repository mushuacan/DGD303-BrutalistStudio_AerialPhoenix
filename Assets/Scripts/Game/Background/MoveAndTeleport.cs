using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAndTeleport : MonoBehaviour
{
    private float movespeed = 2f;
    private float zBorder = 20f;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (-1 * movespeed * Time.deltaTime));

        if (transform.position.z < -zBorder)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zBorder);
        }
    }
}
