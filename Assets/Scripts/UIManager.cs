using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public bool pauseGame = false;
    public Button pauseButton, playButton;
    public GameObject pausePanel;
    Animation pausePanelClose;
    Animator pausePanelCloseAnimator;

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

    private void Start()
    {
        Time.timeScale = 1;
        instance.pausePanel.SetActive(false);
        pauseButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false);
        pausePanelClose = instance.pausePanel.GetComponent<Animation>();
        pausePanelCloseAnimator = instance.pausePanel.GetComponent<Animator>();

        instance.highScoreText.text = "" + PlayerPrefs.GetInt("HighScore");
    }

    public static void UpdateLives(int l)
    {
        foreach (Image i in instance.lifeSprites)
        {
            i.color = instance.inactive;
        }
        for (int i = 0; i < l; i++)
        {
            instance.lifeSprites[i].color = instance.active;
        }
    }

    public static void UpdateHealthBar (int h)
    {
        instance.healthBar.sprite = instance.healthBars[h];
    }

    public static void UpdateScore (int s)
    {
        instance.score += s;
        instance.scoreText.text = instance.score.ToString(/*"000,000"*/);
    }

    public static void UpdateHighScore()
    {
        if(instance.score > PlayerPrefs.GetInt("HighScore"))
        {
            instance.highScore = instance.score;
            PlayerPrefs.SetInt("HighScore", instance.highScore);
        }

        instance.highScoreText.text = "" + PlayerPrefs.GetInt("HighScore");
    }

    public static void UpdateWave()
    {
        instance.wave++;
        instance.waveText.text = instance.wave.ToString();
    }

    public static void UpdateCoins()
    {
        instance.coinText.text = Inventory.currentCoins.ToString();
    }

    public static void QuitGame()
    {
        Debug.Log("Quit Game!..");
        Application.Quit();
    }


    public static void PauseButton(bool p)
    {
        if (p)
        {
            instance.pauseButton.gameObject.SetActive(false);
            instance.playButton.gameObject.SetActive(true);
            instance.playButton.interactable = false;
            instance.pausePanel.SetActive(true);
            instance.StartCoroutine(instance.PausePanelOpen());
        }
        else
        {
            instance.pauseButton.gameObject.SetActive(true);
            instance.pauseButton.interactable = false;
            instance.playButton.gameObject.SetActive(false);
            instance.pausePanelCloseAnimator.SetBool("PauseClose", true);
            instance.StartCoroutine(instance.PausePanelClose());

        }
    }

    IEnumerator PausePanelOpen()
    {
        yield return new WaitForSeconds(.5f);
        instance.playButton.interactable = true;
        Time.timeScale = 0;

    }
    IEnumerator PausePanelClose()
    {
        Time.timeScale = 1;
        yield return new WaitForSeconds(.4f);
        instance.pausePanel.SetActive(false);
        instance.pauseButton.interactable = true;

    }

}
