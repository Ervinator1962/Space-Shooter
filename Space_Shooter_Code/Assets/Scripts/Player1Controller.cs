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

    Rigidbody rb;

    public float speed;
    public float tilt;

    public int superChargeUp;

    public Boundary boundary;

    private float moveHorizontal;
    private float moveVertical;
    private Vector3 movement;

    private bool startBarrel;
    public float barrelDistance;
    public float barrelDuration;
    private float barrelAngle;
    private float barrelOffset;
    private float startBarrelTime;
  

    //1 = right, -1 = left, 0 = not barrelling
    private int barrelDirection;

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
        rb = GetComponent<Rigidbody>();
        gameController.ResetAsteroidsDestroyed();
        gameController.SetSuper(false);
        //initialisation
        nextFire = 0;
        barrelDirection = 0;
        barrelOffset = 0.001f;
        startBarrel = false;
    }

    private void FixedUpdate()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");


        if (barrelDirection != 0)
            {
            if (startBarrel == true)
            {
                //offset to avoid division by zero in first barrel frame
                startBarrelTime = Time.time - barrelOffset;
                startBarrel = false;
            }
            if (Time.time < startBarrelTime + barrelDuration)
            {
                movement = new Vector3(-barrelDirection, 0.0f, moveVertical);
                rb.velocity = movement * barrelDistance/barrelDuration;
                barrelAngle = 360.0f * (Time.time - startBarrelTime) / barrelDuration;
                rb.rotation = Quaternion.Euler(0.0f, 0.0f, barrelDirection * barrelAngle);
            }
            else
            {
                startBarrel = false;
                barrelAngle = 0;
                barrelDirection = 0;
            }
        }
        else
        {
            movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            rb.velocity = movement * speed;
            rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
        }

        rb.position = new Vector3
        (
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
        );
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
            BoltMover bolt1, bolt2, bolt3;
            float angleInitial, angleOffset;
            angleInitial = 90.0f;
            angleOffset = 15.0f;
            boltMaker = Instantiate(shot, shotSpawn.position, Quaternion.Euler(0.0f, angleOffset, 0.0f));
            bolt1 = boltMaker.GetComponent <BoltMover>();
            bolt1.SetBoltDirection(AngleToVector(angleInitial - angleOffset));
            
            boltMaker = Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            bolt2 = boltMaker.GetComponent<BoltMover>();
            bolt2.SetBoltDirection(AngleToVector(angleInitial));

            boltMaker = Instantiate(shot, shotSpawn.position, Quaternion.Euler(0.0f, -angleOffset, 0.0f));
            bolt3 = boltMaker.GetComponent<BoltMover>();
            bolt3.SetBoltDirection(AngleToVector(angleInitial + angleOffset));

            gameController.ResetAsteroidsDestroyed();
            gameController.SetSuper(false);
        } 

        if (Input.GetButton("Jump") && barrelDirection == 0)
        {
            startBarrel = true;
            if (Input.GetAxis("Horizontal") > 0)
            {
                barrelDirection = -1;
            }
            else
            {
                barrelDirection = 1;
            }
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