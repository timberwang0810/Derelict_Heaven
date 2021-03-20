using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState { menu, getReady, playing, paused, oops, gameOver };
    public enum Form { original, charger, archer, pressurizer };
    public GameState gameState;
    public static GameManager S;

    // Enemy Prefabs
    public GameObject Charger;
    public GameObject Archer;

    // Game Variables
    public int maxLives;
    private int lives;

    private void Awake()
    {
        // Singleton Definition
        if (GameManager.S)
        {
            // singleton exists, delete this object
            Destroy(this.gameObject);
        }
        else
        {
            S = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void StartNewGame()
    {
        gameState = GameState.getReady;
        // TODO: put any setup + coroutines here
        ResetLevel();
    }

    public void ResetLevel()
    {
        // Reset any variables necessary in the level
        gameState = GameState.playing;
    }

    public void OnLivesLost()
    {
        lives--;
        if (lives <= 0) OnLevelLost();
    }

    public void OnLevelLost()
    {
        gameState = GameState.gameOver;
        UIManager.S.ShowPopUpForSeconds("You Lost!", 3);
    }

    public void OnLevelWon()
    {
        gameState = GameState.gameOver;
        UIManager.S.ShowPopUpForSeconds("You Won!", 3);
    }
}
