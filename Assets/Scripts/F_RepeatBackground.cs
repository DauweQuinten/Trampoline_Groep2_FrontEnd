using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F_RepeatBackground : MonoBehaviour
{
    Vector3 startPosition;
    BoxCollider boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z > startPosition.z + boxCollider.size.z/2)
        {
            transform.position = startPosition;
        }
    }
}
