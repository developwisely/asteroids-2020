using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeSeconds = 1f;
    private Rigidbody _rb;

    public void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = transform.forward * 2500;
    }

    public void setAutoDestroyTimer()
    {
        // Set auto destroy timer
        Destroy(gameObject, lifeSeconds);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
