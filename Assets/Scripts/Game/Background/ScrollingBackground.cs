using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private Transform part1;
    [SerializeField] private Transform part2;
    [SerializeField] private float size;
    [SerializeField] private float speed;
    private Vector3 positionMarker;

    // Start is called before the first frame update
    void Start()
    {
        part1.transform.position = new Vector3(part1.transform.position.x, part1.transform.position.y, 0);
        part2.transform.position = new Vector3(part2.transform.position.x, part2.transform.position.y, size);
        positionMarker = new Vector3(0, 0, (-1) * speed);
    }

    // Update is called once per frame
    void Update()
    {
        part1.transform.position += positionMarker * Time.deltaTime;
        part2.transform.position += positionMarker * Time.deltaTime;

        if (part1.transform.position.z < -size )
        {
            part1.transform.position = new Vector3(part2.transform.position.x, part2.transform.position.y, part2.transform.position.z + size);
        }
        if (part2.transform.position.z < -size)
        {
            part2.transform.position = new Vector3(part1.transform.position.x, part1.transform.position.y, part1.transform.position.z + size);
        }
    }
}
