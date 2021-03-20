using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    private bool lit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void lightTorch()
    {
        if (lit) return;
        Debug.Log("ive been lit");
        lit = true;
        GetComponentInParent<TorchSet>().OnTorchLit();
    }

    public void putOutTorch()
    {
        if (!lit) return;
        lit = false;
        GetComponentInParent<TorchSet>().OnTorchUnlit();
    }
}
