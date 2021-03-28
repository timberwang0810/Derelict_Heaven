using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ReturnQueueManager : MonoBehaviour
{
    struct ReturnEnemy
    {
        public Form form;
        public Vector3 spawn;

        public ReturnEnemy(Form f, Vector3 s)
        {
            this.form = f;
            this.spawn = s;
        }
    }
    public int delayTime;
    private Queue returnQueue = new Queue();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddToQueue(Form form, Vector3 spawn)
    {
        returnQueue.Enqueue(new ReturnEnemy(form, spawn));
    }

    public void deleteEnemy()
    {
        returnQueue.Dequeue();
    }

    public void returnEnemy()
    {
        StartCoroutine(returnEnemyDelay());
    }

    private IEnumerator returnEnemyDelay()
    {
        yield return new WaitForSeconds(delayTime);
        spawnEnemy();
    }

    private void spawnEnemy()
    {
        ReturnEnemy ret = (ReturnEnemy)returnQueue.Dequeue();
        GameObject newEnemy;
        switch (ret.form)
        {
            case Form.charger:
                newEnemy = Instantiate(GameManager.S.Charger);
                break;
            case Form.archer:
                newEnemy = Instantiate(GameManager.S.Archer);
                break;
            case Form.pressurizer:
                newEnemy = Instantiate(GameManager.S.Pressurizer);
                break;
            default:
                throw new Exception("no enemy of this type");
        }
        newEnemy.transform.position = ret.spawn;
    }
}
