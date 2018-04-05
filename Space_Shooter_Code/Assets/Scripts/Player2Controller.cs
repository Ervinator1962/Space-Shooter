using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player2Controller : MonoBehaviour
{
    public Boundary boundary;

    public float speed;
    public int tilt;
    private float moveRight = 1;

    public GameObject shot;
    public Transform shotSpawn;

    private void FixedUpdate()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb.position.x >= boundary.xMax - 1) {
            moveRight = -1;
        }
        if (rb.position.x <= boundary.xMin + 1){
            moveRight = 1;
        }
        Vector3 movement = new Vector3 (moveRight, 0.0f, 0.0f);
        rb.velocity = movement * speed;
        rb.position = new Vector3
        (
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
        );
        rb.rotation = Quaternion.Euler(0.0f, 180, rb.velocity.x * tilt);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bolt")
        {
            GameObject boltMaker;
            BoltMover bolt;
            AudioSource audio;

            audio = GetComponent<AudioSource>();
            audio.Play();

            boltMaker = Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            bolt = boltMaker.GetComponent<BoltMover>();
            bolt.SetBoltDirection(new Vector3(0.0f, 0.0f, 1.0f));

            Destroy(other.gameObject);
        }
    }

}

