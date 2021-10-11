using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoHorizontal : MonoBehaviour
{
    public float speed = 0.1f;
    public Transform target;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == target.position)
        {
            targetPosition = initialPosition;
        }
        if (transform.position == initialPosition)
        {
            targetPosition = target.position;
        }
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

    }
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("colisión xd ");
        targetPosition = initialPosition;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("colisión xd ");
        targetPosition = initialPosition;
    }
}
