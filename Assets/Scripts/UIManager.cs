using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public TextMeshProUGUI scoreText;
    int score;
    public TextMeshProUGUI highScoreText;
    int highScore;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI waveText;
    int wave;
    public Image[] lifeSprites;
    public Image healthBar;
    public Sprite[] healthBars;
    Color32 active = new Color(1, 1, 1, 1);
    Color32 inactive = new Color(1, 1, 1, 0.25f);

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void UpdateLives(int l)
    {

    }

    public static void UpdateScore (int l)
    {

    }

    public static void UpdateHighScore()
    {

    }

    public static void UpdateWave()
    {

    }

    public static void UpdateCoins()
    {

    }

}
