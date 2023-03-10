using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    //Win/Game Over state
    public GameOverScreen GameOverScreen;
    public WinScreen WinScreen;
    
    public static ScoreManager instance;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoreText;

    int score = 0;
    int highscore = 0;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        highscore = PlayerPrefs.GetInt("highscore", 0);
        scoreText.text = score.ToString() + " POINTS";
        highscoreText.text = "HIGHSCORE: " + highscore.ToString();
    }

    public void AddPoint()
    {
        score += 1;
        scoreText.text = score.ToString() + "POINTS";

        if (highscore < score)
            PlayerPrefs.SetInt("highscore", score);
    }

    //Game over screen score
    public void GameOver()
    {
        GameOverScreen.Setup(score);
    }

    //Win screen score
    public void Win()
    {
        WinScreen.Setup(score);
    }
}