using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;

    private float xVelocity = 0.0f;
    private float yVelocity = 0.0f;

    private bool restricted = true;
    // Start is called before the first frame update
    void Start()
    {
        if (transform.position.y < LevelManager.S.minY)
        {

            transform.position = new Vector3(transform.position.x, LevelManager.S.minY, transform.position.z);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!restricted) return;
        if (player)
        {
            Vector3 playerposition = player.transform.position;
            Vector3 cameraposition = transform.position;

            if (LevelManager.S.isFinalLevel)
            {
                if (playerposition.x >= cameraposition.x)
                {
                    cameraposition.x = Mathf.SmoothDamp(cameraposition.x, playerposition.x, ref xVelocity, 0.1f);
                }
            }
            else
            {
                if (playerposition.x != cameraposition.x)
                {
                    cameraposition.x = Mathf.SmoothDamp(cameraposition.x, playerposition.x, ref xVelocity, 0.1f);
                }
                if (playerposition.y != cameraposition.y)
                {
                    cameraposition.y = Mathf.Clamp(Mathf.SmoothDamp(cameraposition.y, playerposition.y, ref yVelocity, 0.1f), LevelManager.S.minY, 30);
                }
            }
            transform.position = cameraposition;
        }
    }

    public void toggleResticted(bool isRestricted)
    {
        restricted = isRestricted;
    }
}
