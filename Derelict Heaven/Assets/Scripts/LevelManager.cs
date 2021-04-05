﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager S;
    public int currLevel;
    public string currLevelName;
    public AudioSource levelBGM;

    public float minY;

    private void Awake()
    {
        S = this;
        if (SoundManager.S)
        {
            SoundManager.S.OnNewLevel();
        }
    }

    private void Start()
    {
        if (GameManager.S)
        {
            Debug.Log("starting");
            GameManager.S.StartNewGame();
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToNextLevel()
    {
        SceneManager.LoadScene("Level" + (currLevel + 1));
        currLevel += 1;
    }
}
