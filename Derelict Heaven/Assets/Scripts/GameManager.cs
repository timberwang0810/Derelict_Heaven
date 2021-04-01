﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState { menu, getReady, playing, paused, oops, gameOver };
    public GameState gameState;
    public static GameManager S;

    // Enemy Prefabs
    public GameObject Charger;
    public GameObject Archer;
    public GameObject Pressurizer;

    // Player
    public GameObject player;

    // Game Variables
    public int maxLives;
    public int maxLevels;
    private int lives;
    private bool invincible;
    private bool keyGotten = false;

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
        lives = maxLives;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameState == GameState.playing)
            {
                UIManager.S.ShowPausePanel();
                gameState = GameState.paused;
                Time.timeScale = 0;
            }

            else if (gameState == GameState.paused)
            {
                UIManager.S.HidePausePanel();
                gameState = GameState.playing;
                Time.timeScale = 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && gameState == GameState.gameOver)
        {
            LevelManager.S.RestartLevel();
        }
    }

    public void StartNewGame()
    {
        gameState = GameState.getReady;
        // TODO: put any setup + coroutines here
        lives = maxLives;
        player = GameObject.Find("Player");
        ResetLevel();
    }

    public void ResetLevel()
    {
        // Reset any variables necessary in the level
        // Don't know if we need if player don't get destroyed upon losing life
        gameState = GameState.playing;
    }

    public void OnLivesLost(Vector2 damageDir)
    {
        Debug.Log("life lost");
        if (invincible) return;
        player.gameObject.GetComponent<Rigidbody2D>().AddForce(damageDir, ForceMode2D.Impulse);
        player.gameObject.GetComponent<Animator>().SetBool("embody", false);
        lives--;
        if (lives <= 0) StartCoroutine(TakeDamageCoroutine(true));
        else StartCoroutine(TakeDamageCoroutine(false));
    }

    public void OnLevelLost()
    {
        gameState = GameState.gameOver;
        UIManager.S.ShowPopUp("You Lost! Press R to restart", false);
    }

    public void OnLevelComplete()
    {
        if (keyGotten)
        {
            StartCoroutine(LevelCompleteCoroutine());
            // TODO: Go to next level or end
        }
    }

    private void OnLevelWon()
    {
        gameState = GameState.gameOver;
        if (LevelManager.S.currLevel >= maxLevels)
        {
            UIManager.S.ShowPopUpForSeconds("You Won!", 3);
        }
        else
        {
            LevelManager.S.GoToNextLevel();
        }
    }

    public void PlayerGotKey()
    {
        keyGotten = true;
    }

    public bool IsInvincible()
    {
        return invincible;
    }

    private IEnumerator LevelCompleteCoroutine()
    {
        gameState = GameState.oops;
        Camera.main.transform.SetParent(null);

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector2.up * 4;
        rb.gravityScale = 0;
        yield return new WaitForSeconds(2.0f);
        OnLevelWon();
    }

    private IEnumerator TakeDamageCoroutine(bool isDead)
    {
        if (isDead) gameState = GameState.oops;
        invincible = true;
        SpriteRenderer r = player.GetComponent<SpriteRenderer>();
        Color opaque = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Color transparent = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        for (int i = 0; i < 3; i++)
        {
            float timer = 0;
            while (timer < 0.1)
            {
                timer += Time.deltaTime;
                r.color = Color.Lerp(opaque, transparent, timer / 1);
                yield return null;

            }
            timer = 0;
            while (timer < 0.1)
            {
                timer += Time.deltaTime;
                r.color = Color.Lerp(transparent, opaque, timer / 1);
                yield return null;
            }
        }
        r.color = opaque;
        invincible = false;

        if (isDead)
        {
            float timer = 0;
            Vector3 playerScale = player.gameObject.transform.localScale;
            float oldX = playerScale.x;
            float oldY = playerScale.y;
            while (timer < 1.5f)
            {
                timer += Time.deltaTime;
                playerScale.x = Mathf.Lerp(oldX, 0, timer / 1.5f);
                playerScale.y = Mathf.Lerp(oldY, 0, timer / 1.5f);
                player.gameObject.transform.localScale = playerScale;
                yield return null;
            }
            OnLevelLost();
        }
    }
}
