using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    private Player1Controller player1;
    private Game_Controller gameController;
    public GameObject explosion;
    public int scoreValue;

    private void Start()
    {

        GameObject player1object = GameObject.FindWithTag("Player1");
        if (player1object != null)
        {
            player1 = player1object.GetComponent<Player1Controller>();
        }
        else
        {
            Debug.Log("Cannot find 'Player1Controller' script");
        }
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<Game_Controller>();
        }
        else
        {
            Debug.Log("In DestroyByContact: Cannot find 'Game_Controller' script");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boundary" || other.tag == "Player2" || other.tag == "Bolt2") return;
        //To Do: Make every object handle their own Destroy function
        if (other.tag == "Player1")
        {
            gameController.healthToZero();
        } else
        {
            gameController.AddScore(scoreValue);
        }
        AsteroidDestroy();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Boundary") return;
        Instantiate(explosion, transform.position, transform.rotation);
    }

    public void AsteroidDestroy()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
