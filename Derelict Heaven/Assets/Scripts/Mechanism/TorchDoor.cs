using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchDoor : MonoBehaviour
{
    private List<GameObject> torches = new List<GameObject>();
    private int numTorches;
    private int litTorches;
    // Start is called before the first frame update
    void Start()
    {
        numTorches = torches.Count;
        Debug.Log("got " + numTorches + " torches");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddTorch(GameObject torch)
    {
        torches.Add(torch);
    }
    
    public void AddLitTorch()
    {
        litTorches++;
        Debug.Log("adding " + litTorches + " " + numTorches);
        if (litTorches == numTorches)
        {
            Debug.Log("open torch door");
            //later itll prob have animation, destroy for now tho
            Destroy(this.gameObject);
        }
    }
}
