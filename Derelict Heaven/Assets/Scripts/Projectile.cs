using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float destroyTime;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        if (collision.gameObject.CompareTag("Player")) GameManager.S.OnLivesLost();
        // TODO: Change collider when asset is put in
        GetComponent<CircleCollider2D>().enabled = false;
        Destroy(this.gameObject, destroyTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Torch")
        {
            collision.gameObject.GetComponent<Torch>().lightTorch();
        }
    }
}
