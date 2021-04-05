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
    public GameObject instructionPanel;
    public GameObject popUpImage;
    public Texture2D aimingReticle;
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
        DontDestroyOnLoad(this);
        chatPanel.SetActive(false);
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
        instructionPanel.SetActive(false);
        popUpImage.SetActive(false);
    }

    public void ShowPopUpImageForSeconds(Sprite image, float duration)
    {
        StartCoroutine(ShowPopUpImageForSecondsCoroutine(image, duration));
    }

    public void ShowPopUpForSeconds(string message, float duration)
    {
        if (message == "") return;
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

    public void ShowInstructionPanel()
    {
        instructionPanel.SetActive(true);
    }

    public void HideInstructionPanel()
    {
        instructionPanel.SetActive(false);
    }

    public float GetSliderVolume()
    {
        return volumeSlider.value;
    }

    public void ShowAimingCursor()
    {
        Vector2 offset = new Vector2(aimingReticle.width / 2, aimingReticle.height / 2);
        Cursor.SetCursor(aimingReticle, offset, CursorMode.Auto);
    }

    public void HideAimingCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
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
