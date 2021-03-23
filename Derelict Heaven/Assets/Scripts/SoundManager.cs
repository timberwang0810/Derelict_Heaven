using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager S;

    public AudioSource audio;

    public AudioClip ConsumeSFX;
    public AudioClip UnConsumeSFX;

    public AudioClip ChargerRunSFX;
    public AudioClip ChargerWalkSFX;
    public AudioClip ChargerJumpSFX;
    public AudioClip ChargerLandSFX;
    public AudioClip ChargerDeathSFX;

    public AudioClip ArrowChargeSFX;
    public AudioClip ArrowFireSFX;

    public AudioClip WallBreakSFX;

    public AudioClip JumpSFX;
    public AudioClip DeathSFX;
    public AudioClip LandSFX;




    private void Awake()
    {

        // Singleton Definition
        if (SoundManager.S)
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
        audio = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Angel Sounds
    public void OnLandSound()
    {
        audio.PlayOneShot(LandSFX);
    }
    public void OnJumpSound()
    {
        audio.PlayOneShot(JumpSFX, 0.5f);
    }
    public void OnDeathSound()
    {
        audio.PlayOneShot(DeathSFX);
    }

    public void OnConsumeSound()
    {
        audio.PlayOneShot(ConsumeSFX, 0.5f);
    }
    public void OnUnConsumeSound()
    {
        audio.PlayOneShot(UnConsumeSFX, 0.5f);
    }


    // Charger Sounds
    public void OnChargerLandSound()
    {
        audio.PlayOneShot(ChargerLandSFX);
    }
    public void OnChargerJumpSound()
    {
        audio.PlayOneShot(ChargerJumpSFX);
    }
    public void OnChargerDeathSound()
    {
        audio.PlayOneShot(ChargerDeathSFX);
    }
    public void OnChargerWalkSound()
    {
        audio.PlayOneShot(ChargerWalkSFX);
    }

    // Arrow Sounds
    public void OnArrowCharge()
    {
        audio.PlayOneShot(ArrowChargeSFX);
    }
    public void OnArrowFire()
    {
        audio.PlayOneShot(ArrowFireSFX);
    }
}
