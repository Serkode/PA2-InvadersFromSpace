using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    public int scoreValue;
    public GameObject explosion;
    public GameObject coinPrefab, lifePrefab, healthPrefab;

    private const int LIFE_CHANCE = 10;
    private const int HEALTH_CHANCE = 20;
    private const int COIN_CHANCE = 100;

    public void Kill()
    {
        UIManager.UpdateScore(scoreValue);
        UIManager.UpdateHighScore();
        AlienMaster.allAliens.Remove(gameObject);
        Instantiate(explosion, transform.position, Quaternion.identity);

        int ran = Random.Range(0, 1000);

        if (ran <= LIFE_CHANCE)
        {
            Instantiate(lifePrefab, transform.position, Quaternion.identity);
        }
        else if(ran <= HEALTH_CHANCE)
        { 
            Instantiate(healthPrefab, transform.position, Quaternion.identity);
        }
        else if (ran <= COIN_CHANCE)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }

        if (AlienMaster.allAliens.Count == 0)
        {
            GameManager.SpawnNewWave();
        }

        gameObject.SetActive(false);

    }
}
