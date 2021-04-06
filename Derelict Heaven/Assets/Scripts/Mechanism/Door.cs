using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Mechanism
{
    public GameObject chain;

    public override void Activate(float animTime)
    {
        Debug.Log("Door Activated");
        //gameObject.GetComponent<BoxCollider2D>().enabled = false;
        // disable entire thing for now
        if (chain == null) CameraManager.S.SolvePuzzlePan(gameObject, animTime, -5);
        else {
            CameraManager.S.ChainPan(gameObject, chain, animTime);
        }
    }

    public void PlayDeactivateAnim()
    {
        //gameObject.SetActive(false);
        GetComponent<Animator>().SetTrigger("open");
        if (this.tag == "TorchDoor")
        {
            SoundManager.S.TorchDoorSFX();
        }else if(this.tag == "PressureDoor")
        {
            SoundManager.S.PressureDoorSFX();
        }
    }

    public override void Deactivate()
    {
        Debug.Log("Door Deactivated");
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}
