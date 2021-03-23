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
        if (GameManager.S) Destroy(GameManager.S.gameObject);
        SceneManager.LoadScene("Level1");
        Destroy(this.gameObject, 1.0f);
    }

    public void btn_Instructions()
    {
        SceneManager.LoadScene("Instructions");
        Destroy(this.gameObject, 1.0f);
    }

    public void btn_Credits()
    {
        SceneManager.LoadScene("Credits");
        Destroy(this.gameObject, 1.0f);
    }

    public void btn_Back()
    {
        SceneManager.LoadScene("Title");
        Destroy(this.gameObject, 1.0f);
    }

    public void btn_Quit()
    {
        Application.Quit();
    }
}
