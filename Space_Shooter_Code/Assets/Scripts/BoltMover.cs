using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltMover : MonoBehaviour {
    Player1Controller player1;
    Rigidbody rb;
    Vector3 boltDirection;
    public float speed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = boltDirection * speed;
    }

    public void SetBoltDirection(Vector3 direction)
    {
        boltDirection = direction;
    }

    public void SetBoltRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }
}
