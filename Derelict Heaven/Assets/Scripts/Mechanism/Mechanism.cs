using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mechanism : MonoBehaviour
{
    public abstract void Activate(float animTime);
    public abstract void Deactivate();
}
