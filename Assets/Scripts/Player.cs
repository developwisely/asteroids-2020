using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float impulseSpeed = 500f;
    public float rotateAngle = 200f;

    // Jets
    public GameObject jetImpulse;
    public GameObject jetRotateClockwise;
    public GameObject jetRotateCounterClockwise;

    // Weapon Spawn points
    public GameObject primaryBulletSpawnLeft;
    public GameObject primaryBulletSpawnRight;
    public GameObject secondaryBulletSpawn;

    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Impulse(Input.GetAxisRaw("Vertical"));
        Rotate(Input.GetAxisRaw("Horizontal"));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // fire weapon
        }
    }

    private void Impulse(float activeImpulse)
    {
        if (activeImpulse == 1)
        {
            jetImpulse.SetActive(true);
            _rb.velocity += transform.forward * impulseSpeed * Time.deltaTime;
        }
        else
        {
            jetImpulse.SetActive(false);
        }   
    }

    private void Rotate(float rotateDirection)
    {
        if (rotateDirection > 0)
        {
            jetRotateClockwise.SetActive(true);
            jetRotateCounterClockwise.SetActive(false);
        }
        else if (rotateDirection < 0)
        {
            jetRotateCounterClockwise.SetActive(true);
            jetRotateClockwise.SetActive(false);
        }
        else
        {
            jetRotateClockwise.SetActive(false);
            jetRotateCounterClockwise.SetActive(false);
        }

        var rotation = Quaternion.AngleAxis(rotateDirection * rotateAngle * Time.deltaTime, Vector3.up);
        transform.forward = rotation * transform.forward;
    }
}
