using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private bool activated = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activated && collision.gameObject.CompareTag("Enemy") && collision.gameObject.GetComponent<Enemy>().GetForm() == GameManager.Form.pressurizer)
        {
            activated = true;
            Debug.Log("activated");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        activated = false;
    }
}
