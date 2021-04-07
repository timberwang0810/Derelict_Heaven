using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinningTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && GameManager.S.gameState == GameManager.GameState.playing) GameManager.S.OnLevelComplete();
    }
}
