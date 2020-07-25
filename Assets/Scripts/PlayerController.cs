using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player speeds
    public float thrustSpeed = 100f;
    public float rotateSpeed = 200f;
    public float maxSpeed = 150f;

    // Jets
    public GameObject jetImpulse;
    public GameObject jetRotateClockwise;
    public GameObject jetRotateCounterClockwise;

    private Rigidbody _rb;
    private float rotation = 0;
    private float acceleration = 0;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        handleRotation();
        handleThrust();
    }

    private void handleRotation()
    {
        rotation = Input.GetAxisRaw("Horizontal") * rotateSpeed * Time.deltaTime;
        transform.Rotate(0, rotation, 0);

        if (rotation > 0)
        {
            jetRotateClockwise.SetActive(true);
            jetRotateCounterClockwise.SetActive(false);
        }
        else if (rotation < 0)
        {
            jetRotateCounterClockwise.SetActive(true);
            jetRotateClockwise.SetActive(false);
        }
        else
        {
            jetRotateClockwise.SetActive(false);
            jetRotateCounterClockwise.SetActive(false);
        }
    }

    private void handleThrust()
    {
        acceleration = thrustSpeed * Input.GetAxisRaw("Vertical");
        if (acceleration > 0)
        {
            jetImpulse.SetActive(true);
            _rb.AddForce(transform.forward * acceleration);
            _rb.velocity = new UnityEngine.Vector3(Mathf.Clamp(_rb.velocity.x, -maxSpeed, maxSpeed), 0, Mathf.Clamp(_rb.velocity.z, -maxSpeed, maxSpeed));
        }
        else
        {
            jetImpulse.SetActive(false);
        }
    }
}
