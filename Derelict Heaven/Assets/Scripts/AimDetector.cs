using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimDetector : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Entered");
            GetComponentInParent<Archer>().FoundPlayer(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Exited");
            if (transform.parent.gameObject.activeSelf) GetComponentInParent<Archer>().FoundPlayer(null);

        }
    }
}
