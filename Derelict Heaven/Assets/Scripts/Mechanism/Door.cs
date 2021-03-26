using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Mechanism
{
    public override void Activate()
    {
        Debug.Log("Door Activated");
        //gameObject.GetComponent<BoxCollider2D>().enabled = false;
        // disable entire thing for now
        CameraManager.S.SolvePuzzlePan(gameObject);
    }

    public void PlayDeactivateAnim()
    {
        gameObject.SetActive(false);
    }

    public override void Deactivate()
    {
        Debug.Log("Door Deactivated");
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}
