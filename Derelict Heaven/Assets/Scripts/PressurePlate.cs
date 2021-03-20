using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public Mechanism connectedMechanism;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Enemy") && collision.gameObject.GetComponent<Enemy>().GetForm() == GameManager.Form.pressurizer)
            || (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<Player>().GetForm() == GameManager.Form.pressurizer))
        {
            connectedMechanism.Activate();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Enemy") && collision.gameObject.GetComponent<Enemy>().GetForm() == GameManager.Form.pressurizer)
            || (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<Player>().GetForm() == GameManager.Form.pressurizer))
        {
            connectedMechanism.Deactivate();
        }
    }
}
