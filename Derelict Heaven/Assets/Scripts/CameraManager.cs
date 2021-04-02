using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager S;
    public Camera cam;
    public GameObject player;
    private Vector3 moveTo;
    private bool isMoving;

    private void Awake()
    {
        // Singleton Definition
        if (CameraManager.S)
        {
            // singleton exists, delete this object
            Destroy(this.gameObject);
        }
        else
        {
            S = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        moveTo = cam.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving && (Vector3.Distance(moveTo, cam.transform.position) > Vector3.kEpsilon))
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, moveTo, Time.deltaTime * 3);
        }
    }

    public void SolvePuzzlePan(GameObject door, float animTime)
    {
        StartCoroutine(PanCamera(door, animTime));
    }

    private IEnumerator PanCamera(GameObject door, float animTime)
    {
        yield return new WaitForSeconds(animTime);
        isMoving = true;
        PanTo(door.transform.position);
        GameManager.S.gameState = GameManager.GameState.paused;
        yield return new WaitForSeconds(0.75f);
        door.GetComponent<Door>().PlayDeactivateAnim();
        yield return new WaitForSeconds(2f);
        GameManager.S.gameState = GameManager.GameState.playing;
        isMoving = false;
        SetToPlayer();
    }

    private void PanTo(Vector3 pos)
    {
        cam.transform.SetParent(null);
        moveTo = new Vector3(pos.x, pos.y, -5);
    }

    private void SetToPlayer()
    {
        cam.transform.SetParent(player.transform);
        cam.transform.localPosition = new Vector3(0, 0, -10);
    }
}
