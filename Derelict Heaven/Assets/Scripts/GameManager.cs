using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager S;

    // Enemy Prefabs
    public GameObject Charger;

    // UI Variables
    [Header("UI Components")]
    public GameObject chatPanel;

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
        chatPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ShowPopUpForSeconds(string message, float duration)
    {
        chatPanel.GetComponentInChildren<TextMeshProUGUI>().text = message;
        chatPanel.SetActive(true);
        yield return new WaitForSeconds(duration);
        chatPanel.SetActive(false);
    }
}
