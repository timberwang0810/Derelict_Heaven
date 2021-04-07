using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public Mechanism connectedMechanism;

    public void Activate()
    {
        connectedMechanism.Activate(1.3f);
    }
}
