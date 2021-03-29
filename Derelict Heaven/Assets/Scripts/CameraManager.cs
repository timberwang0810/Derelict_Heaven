using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager S;
    public Camera cam;
    public GameObject player;
    private Vector3 moveTo;

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
        if ((Vector3.Distance(moveTo, cam.transform.position) > Vector3.kEpsilon) && cam.transform.parent == null)
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
        PanTo(door.transform.position);
        GameManager.S.gameState = GameManager.GameState.paused;
        yield return new WaitForSeconds(2);
        door.GetComponent<Door>().PlayDeactivateAnim();
        yield return new WaitForSeconds(1);
        GameManager.S.gameState = GameManager.GameState.playing;
        SetToPlayer();
    }

    private void PanTo(Vector3 pos)
    {
        cam.transform.SetParent(null);
        moveTo = new Vector3(pos.x, pos.y, -5);
    }

    private void SetToPlayer()
    {
        cam.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        cam.transform.SetParent(player.transform);
    }
}
