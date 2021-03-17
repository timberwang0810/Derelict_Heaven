using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float destroyTime;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("hit " + collision.gameObject.name);
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        Destroy(this.gameObject, destroyTime);
    }
}
