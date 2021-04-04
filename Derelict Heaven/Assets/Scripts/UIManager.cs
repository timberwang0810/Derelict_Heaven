using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager S;

    public GameObject chatPanel;
    public GameObject pausePanel;
    public GameObject settingsPanel;
    public GameObject popUpImage;
    public Slider volumeSlider;

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
        settingsPanel.SetActive(false);
        popUpImage.SetActive(false);
    }

    public void ShowPopUpImageForSeconds(Sprite image, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(ShowPopUpImageForSecondsCoroutine(image, duration));
    }

    public void ShowPopUpForSeconds(string message, float duration)
    {
        if (message == "") return;
        StopAllCoroutines();
        StartCoroutine(ShowPopUpForSecondsCoroutine(message, duration));
    }

    public void ShowPopUp(string message, bool hasNext)
    {
        chatPanel.GetComponentsInChildren<TextMeshProUGUI>()[0].text = message;
        chatPanel.GetComponentsInChildren<TextMeshProUGUI>()[1].enabled = hasNext;
        chatPanel.SetActive(true);
    }

    public void HidePopUp()
    {
        chatPanel.SetActive(false);
    }

    public void ShowPausePanel()
    {
        pausePanel.SetActive(true);
    }

    public void HidePausePanel()
    {
        pausePanel.SetActive(false);
    }

    public void ShowSettingsPanel()
    {
        settingsPanel.SetActive(true);
    }

    public void HideSettingsPanel()
    {
        settingsPanel.SetActive(false);
    }

    public float GetSliderVolume()
    {
        return volumeSlider.value;
    }

    private IEnumerator ShowPopUpForSecondsCoroutine(string message, float duration)
    {
        ShowPopUp(message, false);
        yield return new WaitForSeconds(duration);
        HidePopUp();
    }

    private IEnumerator ShowPopUpImageForSecondsCoroutine(Sprite image, float duration)
    {
        popUpImage.GetComponent<Image>().sprite = image;
        popUpImage.SetActive(true);
        yield return new WaitForSeconds(duration);
        popUpImage.SetActive(false);
    }
}
