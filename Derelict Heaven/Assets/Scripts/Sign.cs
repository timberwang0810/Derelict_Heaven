using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    public string[] messages;
    private int currIndex = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (currIndex == messages.Length - 1)
            {
                UIManager.S.ShowPopUp(messages[currIndex], false);
            }
            else
            {
                if (currIndex >= messages.Length) currIndex = 0;
                UIManager.S.ShowPopUp(messages[currIndex], messages.Length > 1);
            }
            currIndex++;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        UIManager.S.ShowPopUp(messages[0], messages.Length > 1);
        currIndex += 1;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        UIManager.S.HidePopUp();
        currIndex = 0;
    }
}
