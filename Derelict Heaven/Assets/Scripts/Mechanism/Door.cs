using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Mechanism
{
    public GameObject chain;

    public override void Activate(float animTime)
    {
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
        if (this.CompareTag( "TorchDoor"))
        {
            SoundManager.S.TorchDoorSFX();
        }else if(this.CompareTag("PressureDoor"))
        {
            SoundManager.S.PressureDoorSFX();
        }
    }

    public override void Deactivate()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}
