using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public enum GameState { GS_PAUSEMENU, GS_GAME,  GS_LEVELCOMPLETED, GS_GAME_OVER, GS_OPTIONS }

// Start is called before the first frame update

public class GameManager : MonoBehaviour
{
    public GameState currentGameState = GameState.GS_GAME;
    public TMP_Text scoreText;
    public TMP_Text endScoreText;
    public TMP_Text HighscoreText;
    public TMP_Text timeText;
    public TMP_Text enemiesDestroyedText;

    public AudioSource src;
    public AudioClip eagleDeathSound, bonusSound, pick;

    private int score = 10;
    private int highestScore = 10;
    public Image[] keysTab;
    public Image[] hearts;
    private int keysFound = 0;
    private int heartsLeft = 3;
    private int enemiesDestroyed = 0;
    private float time = 0f;

    public static GameManager instance;

    public Canvas pauseMenuCanvas;
    public Canvas CompletedLevelCanvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        currentGameState = GameState.GS_GAME;
        instance = this;
        keysTab[0].color = Color.gray;
        keysTab[1].color = Color.gray;
        keysTab[2].color = Color.gray;
        scoreText.SetText(score.ToString());
    }
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        scoreText.SetText(score.ToString());
        timeText.SetText(string.Format("{0:00}:{1:00}", (int)((time + 1) / 60), (time + 1) % 60));


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("TEST1");
            if(currentGameState == GameState.GS_GAME) 
            {
                PauseMenu();
            }
            else if (currentGameState == GameState.GS_PAUSEMENU) 
            { 
                InGame(); 
            }

        }
    }
    void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;
        pauseMenuCanvas.enabled = (currentGameState == GameState.GS_PAUSEMENU);
    }

    void PauseMenu()
    {
        SetGameState(GameState.GS_PAUSEMENU);
    }

    void Options()
    {
        SetGameState(GameState.GS_OPTIONS);
    }
    void InGame()
    {
        SetGameState(GameState.GS_GAME);
    }
    void LevelCompleted()
    {
        SetGameState(GameState.GS_LEVELCOMPLETED);
    }
    void GameOver()
    {
        SetGameState(GameState.GS_GAME_OVER);
    }
    public void AddPoints(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }

    public void AddKeys()
    {
        src.clip = bonusSound;
        src.Play();
        keysTab[keysFound].color = Color.white;
        keysFound++;
    }

    public void AddGem()
    {
        src.clip = pick;
        src.Play();
    }

    public void UpdateHearth()
    {
        heartsLeft = heartsLeft > 0 ? heartsLeft - 1 : 0;
        hearts[heartsLeft].enabled = false;
    }

    public void AddEnemyDestruction()
    {
        src.clip = eagleDeathSound;
        src.Play();
        enemiesDestroyed++;
        enemiesDestroyedText.SetText(enemiesDestroyed.ToString());
    }

    public void OnRestartButtonPressed()
    {
        SceneManager.LoadScene("Level1");
    }

    public void OnResumeButtonPressed()
    {
        InGame();
    }

    public void OnExitButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnNextLevelButtonPressed()
    {
        //SceneManager.LoadScene("Level2");
    }

    public void LevelCompletion()
    {
        LevelCompleted();
        CompletedLevelCanvas.enabled = true;
        endScoreText.text = ("Your Score: " + score.ToString());
        if(score>highestScore)
        {
            highestScore = score;
        }
        HighscoreText.text = ("Highscore: " + highestScore.ToString());
    }
}
