using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager S;

    public GameObject chatPanel;

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
    }


    public IEnumerator ShowPopUpForSeconds(string message, float duration)
    {
        chatPanel.GetComponentInChildren<TextMeshProUGUI>().text = message;
        chatPanel.SetActive(true);
        yield return new WaitForSeconds(duration);
        chatPanel.SetActive(false);
    }
}
