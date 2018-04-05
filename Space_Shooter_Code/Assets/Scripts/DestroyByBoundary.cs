using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByBoundary : MonoBehaviour
{
    private Game_Controller gameController;
    public int asteroidDamage;
    private void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<Game_Controller>();
        }
        else
        {
            Debug.Log("In DestroyByBoundary: Cannot find 'Game_Controller' script");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Asteroid" && gameController.gameOver == false)
        {
            DestroyByContact asteroid = other.gameObject.GetComponent<DestroyByContact>();
            asteroid.InstantiateExplosion();
            gameController.DecreaseHealth(asteroidDamage);
        }
        Destroy(other.gameObject);
    }
}

