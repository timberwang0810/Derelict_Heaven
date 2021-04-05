using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public GameObject winTrigger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.S.PlayerGotKey();
            SoundManager.S.OnObtainKeySound();
            winTrigger.GetComponent<Animator>().SetTrigger("open");
            Destroy(this.gameObject);
        }
    }
}
