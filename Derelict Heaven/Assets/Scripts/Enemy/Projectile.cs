using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float destroyTime;
    private bool hitSomething;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        if (collision.gameObject.CompareTag("Player") && !hitSomething)
        {
            Vector2 dir = collision.gameObject.transform.position - gameObject.transform.position;
            dir.Normalize();
            GameManager.S.OnLivesLost(dir);
        }
        hitSomething = true;
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
