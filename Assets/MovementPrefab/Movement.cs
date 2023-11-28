using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turnSpeed = 100f;

    void Start()
    {

    }

    void Update()
    {
        // Move forward
        if (Input.GetKey("w"))
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }

        // Move backward
        if (Input.GetKey("s"))
        {
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }

        // Move left
        if (Input.GetKey("a"))
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }

        // Move right
        if (Input.GetKey("d"))
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }

        // Turn left
        if (Input.GetKey("q"))
        {
            transform.Rotate(Vector3.down, turnSpeed * Time.deltaTime);
        }

        // Turn right
        if (Input.GetKey("e"))
        {
            transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
        }
    }
}