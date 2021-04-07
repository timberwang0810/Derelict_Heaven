using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchSet : MonoBehaviour
{
    public Mechanism connectedMechanism;
    private Torch[] torches;
    private int numLit;

    public AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        torches = GetComponentsInChildren<Torch>(); 
    }


    public void OnTorchLit()
    {
        numLit++;
        if (numLit >= torches.Length)
        {
            connectedMechanism.Activate(0.3f);
            audio.Play();
            SoundManager.S.OnHittingTorch();
        }
    }

    public void OnTorchUnlit()
    {
        audio.Stop();
        numLit--;
        connectedMechanism.Deactivate();
    }
}
