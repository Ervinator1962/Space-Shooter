using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Controller : MonoBehaviour
{
    private int round;
    private Player1Controller player1;

    public GameObject hazard;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public GUIText scoreText;
    public GUIText healthText;
    public GUIText gameOverText;
    public GUIText restartText;
    public GUIText superText;
    public GUIText roundText;

    public int asteroidDamage;

    public bool gameOver;
    private bool restart;

    private int score;
    private int health;

    private bool playerAlive;
    public bool superActivated;

    private int consecutiveDestroyed;
    private void Start()
    {
        score = 0;
        round = 0;
        playerAlive = true;
        health = 100;
        gameOver = false;
        restart = false;
        restartText.text = "";
        gameOverText.text = "";
        superText.text = "";
        scoreText.text = "";
        UpdateScore();
        UpdateHealth();
        StartCoroutine(SpawnWaves());
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

    private void Update()
    {
        if (restart && Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
        if (health == 0 && playerAlive == true) {
            playerAlive = false;
            player1.PlayerDestroy();
            GameOver();
        }
        if (consecutiveDestroyed == player1.GetSuperLevel())
        {
            superActivated = true;
            superText.text = "Super Ready";
        } 
        if (superActivated == false)
        {
            superText.text = "";
        }
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while(true)
        {
            round += 1;
            roundText.text = "Wave " + round;
            for (int i = 0; i < hazardCount; i++)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                if (i == 3) roundText.text = "";
                if (gameOver == false)
                {
                    Instantiate(hazard, spawnPosition, spawnRotation);
                }
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);

            if (gameOver)
            {
                restartText.text = "Press 'R' for restart";
                restart = true;
                break;
            }
        }
    }

    public int GetConsecutiveDestroyed()
    {
        return consecutiveDestroyed;
    }

    public bool GetSuperStatus()
    {
        return superActivated;
    }

    public void SetSuper(bool status)
    {
        superActivated = status;
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    public void DecreaseHealth()
    {
        //hacky way to prevent over-decreasing health
        health -= asteroidDamage;
        UpdateHealth();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    void UpdateHealth()
    {
        healthText.text = "Health: " + health;
    }

    public void healthToZero()
    {
        health = 0;
        UpdateHealth();
    }

    public void UpdateAsteroidsDestroyed()
    {
        consecutiveDestroyed += 1;
    }

    public void ResetAsteroidsDestroyed()
    {
        consecutiveDestroyed = 0;
    }

    public void GameOver()
    {
        gameOver = true;
        gameOverText.text = "Game Over";
    }
}
