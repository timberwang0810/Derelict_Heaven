using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void btn_StartTheGame()
    {
        SceneManager.LoadScene("LevelSelect");
        Destroy(this.gameObject);
    }

    public void btn_Level1()
    {
        if (GameManager.S) Destroy(GameManager.S.gameObject);
        SceneManager.LoadScene("Level1");
        Destroy(this.gameObject);
    }

    public void btn_Level2()
    {
        if (GameManager.S) Destroy(GameManager.S.gameObject);
        SceneManager.LoadScene("Level2");
        Destroy(this.gameObject);
    }

    public void btn_Level3()
    {
        if (GameManager.S) Destroy(GameManager.S.gameObject);
        SceneManager.LoadScene("Level3");
        Destroy(this.gameObject);
    }

    public void btn_Level4()
    {
        if (GameManager.S) Destroy(GameManager.S.gameObject);
        SceneManager.LoadScene("Level4");
        Destroy(this.gameObject);
    }

    public void btn_Instructions()
    {
        SceneManager.LoadScene("Instructions");
        Destroy(this.gameObject);
    }

    public void btn_Credits()
    {
        SceneManager.LoadScene("Credits");
        Destroy(this.gameObject);
    }

    public void btn_Settings()
    {
        SceneManager.LoadScene("Settings");
        Destroy(this.gameObject);
    }

    public void btn_Back()
    {
        if (GameManager.S) Destroy(GameManager.S.gameObject);
        if (UIManager.S) Destroy(UIManager.S.gameObject);
        if (SoundManager.S) Destroy(SoundManager.S.gameObject);
        SceneManager.LoadScene("Title");
        Destroy(this.gameObject);
    }

    public void btn_Quit()
    {
        Application.Quit();
    }
}
