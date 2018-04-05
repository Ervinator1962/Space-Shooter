using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class Player1Controller : MonoBehaviour
{
    private Game_Controller gameController;
    public GameObject player1Explosion;

    public float speed;
    public float tilt;
    public int superChargeUp;
    public Boundary boundary;

    public GameObject shot;
    public Transform shotSpawn;

    public float fireRate;
    private float nextFire;

    private float nextSpecial;

    private void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<Game_Controller>();
        }
        else
        {
            Debug.Log("Cannot find 'Game_Controller' script");
        }
        gameController.ResetAsteroidsDestroyed();
        gameController.SetSuper(false);
        //initialisation
        nextFire = 0;
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            GameObject boltMaker;
            BoltMover bolt;
            boltMaker = Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            bolt = boltMaker.GetComponent<BoltMover>();
            bolt.SetBoltDirection(new Vector3(0.0f, 0.0f, 1.0f));
            nextFire = Time.time + fireRate;
        }

        if (Input.GetButton("Fire2") && gameController.GetSuperStatus() == true) 
        {
            StartCoroutine(SuperShot());
            gameController.ResetAsteroidsDestroyed();
            gameController.SetSuper(false);
        }

        if (Input.GetButton("Fire3") && gameController.GetSuperStatus() == true)
        {
            GameObject boltMaker;
            BoltMover bolt1;
            BoltMover bolt2; 
            BoltMover bolt3;

            boltMaker = Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            bolt1 = boltMaker.GetComponent <BoltMover>();
            bolt1.SetBoltDirection(AngleToVector(80.0f));

            boltMaker = Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            bolt2 = boltMaker.GetComponent<BoltMover>();
            bolt2.SetBoltDirection(AngleToVector(90.0f));

            boltMaker = Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            bolt3 = boltMaker.GetComponent<BoltMover>();
            bolt3.SetBoltDirection(AngleToVector(100.0f));

            gameController.ResetAsteroidsDestroyed();
            gameController.SetSuper(false);
        }

    }

    IEnumerator SuperShot()
    {
        float superRate;
        GameObject boltMaker;
        BoltMover bolt;

        superRate = 0.1f;
        for (int i = 0; i < 3; i++)
        {
            boltMaker = Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            bolt = boltMaker.GetComponent<BoltMover>();
            bolt.SetBoltDirection(new Vector3(0.0f, 0.0f, 1.0f));
            yield return new WaitForSeconds(superRate);
        }
    }

    private Vector3 AngleToVector(float angle)
    {
        if (angle < 0 || angle >= 360) angle %= 360;
        angle = AngleToRadians(angle);
        Vector3 vector = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
        return vector;//transform.TransformVector(vector);
    }

    private float AngleToRadians(float angle)
    {
        if (angle < 0 || angle >= 360) angle %= 360;
        return (float)Mathf.PI/180.0f * angle;
    }

    private void FixedUpdate()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.velocity = movement * speed;

        rb.position = new Vector3
        (
            Mathf.Clamp (rb.position.x, boundary.xMin, boundary.xMax),
            0.0f, 
            Mathf.Clamp (rb.position.z, boundary.zMin, boundary.zMax)
        );
        rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
    }

    private void OnTriggerEnter(Collider other)
    {
        //only handles the enemy bolt
        if (other.tag != "Bolt2") return;
        Destroy(other.gameObject);
        gameController.healthToZero();
    }

    public void PlayerDestroy()
    {
        Instantiate(player1Explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public int GetSuperLevel()
    {
        return superChargeUp;
    }
}