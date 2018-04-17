using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltController : MonoBehaviour {
    private Game_Controller gameController;
    private Player1Controller player1;

    private void Start()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();

        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<Game_Controller>();
        }
        else
        {
            Debug.Log("Cannot find 'Game_Controller' script");
        }

        GameObject player1object = GameObject.FindWithTag("Player1");
        if (player1object != null)
        {
            player1 = player1object.GetComponent<Player1Controller>();
        }
        else
        {
            Debug.Log("Cannot find 'Player1Controller' script");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boundary" || other.tag == "Bolt" || other.tag == "Bolt2") return;
        if (other.tag == "Asteroid")
        {
            gameController.UpdateAsteroidsDestroyed();
        }
        else 
        {
            gameController.ResetAsteroidsDestroyed();
        }
        Destroy(gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Boundary") gameController.ResetAsteroidsDestroyed();
    }

    public void BoltDestroy()
    {
        Destroy(gameObject);
    }
}
