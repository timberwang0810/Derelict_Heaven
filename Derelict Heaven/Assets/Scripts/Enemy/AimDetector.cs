using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Entered");
            GetComponentInParent<Archer>().FoundPlayer(collision.gameObject);
            GetComponentInParent<Archer>().ChangeAnimSight(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Exited");
            if (transform.parent.gameObject.activeSelf)
            {
                GetComponentInParent<Archer>().FoundPlayer(null);
                GetComponentInParent<Archer>().firstShot = true;
                GetComponentInParent<Archer>().ChangeAnimSight(false);
            }
        }
    }
}
