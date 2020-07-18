using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float impulseSpeed = 500f;
    public float rotateAngle = 200f;

    public Transform bulletSpawn;

    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Vertical") == 1)
        {
            Impulse();
        }

        Rotate(Input.GetAxisRaw("Horizontal"));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // fire weapon
        }
    }

    private void Impulse()
    {
        _rb.velocity += transform.forward * impulseSpeed * Time.deltaTime;
    }

    private void Rotate(float rotateDirection)
    {
        var rotation = Quaternion.AngleAxis(rotateDirection * rotateAngle * Time.deltaTime, Vector3.up);
        transform.forward = rotation * transform.forward;
    }
}
