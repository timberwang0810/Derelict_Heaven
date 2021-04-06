using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager S;
    public Camera cam;
    public GameObject player;
    public GameObject boss;
    private Vector3 origPos;
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

    public void ChainPan(GameObject door, GameObject chain, float animTime)
    {
        StartCoroutine(ChainPanCamera(door, chain, animTime));
    }

    private IEnumerator ChainPanCamera(GameObject door, GameObject chain, float animTime)
    {
        origPos = cam.transform.position;
        yield return new WaitForSeconds(animTime);
        cam.GetComponent<CameraFollow>().toggleResticted(false);
        cam.transform.position = new Vector3(1, -2, -15);
        GameManager.S.gameState = GameManager.GameState.paused;
        yield return new WaitForSeconds(0.75f);
        door.GetComponent<Door>().PlayDeactivateAnim();
        yield return new WaitForSeconds(0.25f);
        foreach (Transform child in chain.transform)
        {
            child.gameObject.GetComponent<Rigidbody2D>().gravityScale = Random.Range(0.2f, 0.7f);
        }
        if (chain.tag == "ChainLong1")
        {
            SoundManager.S.OnChainLong1Fall();
        }else if (chain.tag == "ChainLong2")
        {
            SoundManager.S.OnChainLong2Fall();
        }
        else
        {
            SoundManager.S.OnChainShortFall();
        }
        Destroy(chain, 10);
        boss.GetComponent<Boss>().addChainDown();
        if (boss.GetComponent<Boss>().chainsDown == boss.GetComponent<Boss>().maxChains)
        {
            yield return new WaitForSeconds(2f);
            boss.GetComponent<Boss>().playFall();
            yield return new WaitForSeconds(3.5f);
        }
        yield return new WaitForSeconds(2f);
        GameManager.S.gameState = GameManager.GameState.playing;
        cam.GetComponent<CameraFollow>().toggleResticted(true);
        cam.transform.position = origPos;
    }

    public void SolvePuzzlePan(GameObject door, float animTime, float z)
    {
        StartCoroutine(PanCamera(door, animTime, z));
    }

    private IEnumerator PanCamera(GameObject door, float animTime, float z)
    {
        origPos = cam.transform.position;
        yield return new WaitForSeconds(animTime);
        isMoving = true;
        cam.GetComponent<CameraFollow>().toggleResticted(false);
        PanTo(door.transform.position, z);
        GameManager.S.gameState = GameManager.GameState.paused;
        yield return new WaitForSeconds(0.75f);
        door.GetComponent<Door>().PlayDeactivateAnim();
        yield return new WaitForSeconds(2f);
        GameManager.S.gameState = GameManager.GameState.playing;
        isMoving = false;
        cam.GetComponent<CameraFollow>().toggleResticted(true);
        cam.transform.position = origPos;
    }

    private void PanTo(Vector3 pos, float z)
    {
        moveTo = new Vector3(pos.x, pos.y, z);
    }
}
