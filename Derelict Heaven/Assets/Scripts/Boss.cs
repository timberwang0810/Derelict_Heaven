using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int chainsDown = 0;
    public int maxChains;
    public GameObject winTrigger;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addChainDown()
    {
        chainsDown++;
        if (chainsDown == maxChains)
        {
            winTrigger.SetActive(true);
        }
    }

    public void playOpen()
    {
        animator.SetTrigger("open");
        SoundManager.S.onBossDoorOpenSound();
    }

    public void playFall()
    {
        SoundManager.S.OnBossDeathSound();
        animator.SetTrigger("fall");
    }
}
