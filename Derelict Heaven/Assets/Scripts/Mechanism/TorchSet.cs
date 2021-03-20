using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchSet : MonoBehaviour
{
    public Mechanism connectedMechanism;
    private Torch[] torches;
    private int numLit;

    // Start is called before the first frame update
    void Start()
    {
        torches = GetComponentsInChildren<Torch>(); 
    }


    public void OnTorchLit()
    {
        numLit++;
        if (numLit >= torches.Length)
        {
            Debug.Log("open torch mechanism");
            connectedMechanism.Activate();
        }
    }

    public void OnTorchUnlit()
    {
        numLit--;
        connectedMechanism.Deactivate();
    }
}
