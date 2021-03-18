using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public bool lit;
    public GameObject assignedDoor;

    void Awake()
    {
        assignedDoor.GetComponent<TorchDoor>().AddTorch(gameObject);
    }

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
        Debug.Log("ive been lit");
        lit = true;
        assignedDoor.GetComponent<TorchDoor>().AddLitTorch();
    }
}
