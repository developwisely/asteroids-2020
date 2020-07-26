using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Asteroid : MonoBehaviour
{
    public float minSpeed;
    public float maxSpeed;
    public float minTumble;
    public float maxTumble;
    public int health;
    public int pointValue;

    private Rigidbody _rb;

    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Activate();
    }

    private void Activate()
    {
        // Look within 40 degrees of center stage
        transform.LookAt(new Vector3(0, 0, 0));
        transform.Rotate(transform.rotation.x, (transform.rotation.y + Random.Range(-40.0f, 40.0f)), transform.rotation.z);

        // Move the asteroid
        AddForce();

        // Start tumbling
        Tumble();
    }

    private void AddForce()
    {
        _rb.velocity = transform.forward * Random.Range(minSpeed, maxSpeed);
    }

    private void Tumble()
    {
        _rb.angularVelocity = Random.insideUnitSphere * Random.Range(minTumble, maxTumble);
    }

    private void OnCollisionEnter(Collision hit)
    {
        Debug.Log(hit.gameObject);
    }
}
