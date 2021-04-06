using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager S;
    public int currLevel;
    public string currLevelName;
    public AudioSource levelBGM;
    public bool isFinalLevel = false;

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
        if (SoundManager.S)
        {
            SoundManager.S.OnNewLevel();
        }
        if (GameManager.S)
        {
            Debug.Log("starting");
            GameManager.S.StartNewGame();
        }
    }

    public void RestartLevel()
    {
        Destroy(GameObject.Find("ButtonManager"));
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToNextLevel()
    {
        Destroy(GameObject.Find("ButtonManager"));
        SceneManager.LoadScene("Level" + (currLevel + 1));
        currLevel += 1;
    }
}
