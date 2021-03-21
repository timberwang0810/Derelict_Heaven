using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager S;

    public GameObject chatPanel;
    public GameObject pausePanel;

    private void Awake()
    {
        // Singleton Definition
        if (UIManager.S)
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
        chatPanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    public void ShowPopUpForSeconds(string message, float duration)
    {
        if (message == "") return;
        StartCoroutine(ShowPopUpForSecondsCoroutine(message, duration));
    }

    public void ShowPausePanel()
    {
        pausePanel.SetActive(true);
    }

    public void HidePausePanel()
    {
        pausePanel.SetActive(false);
    }

    private IEnumerator ShowPopUpForSecondsCoroutine(string message, float duration)
    {
        chatPanel.GetComponentInChildren<TextMeshProUGUI>().text = message;
        chatPanel.SetActive(true);
        yield return new WaitForSeconds(duration);
        chatPanel.SetActive(false);
    }
}
