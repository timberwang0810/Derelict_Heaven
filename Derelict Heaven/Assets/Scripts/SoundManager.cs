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
    public void OnLandSound(Player p)
    {
        Debug.Log("fired");
        if (!LandSFX) return;// TODO: DELETE
        switch (p.myForm)
        {
            case Form.original:
                audio.PlayOneShot(LandSFX);
                break;
            case Form.charger:
                OnChargerLandSound();
                break;
            // TODO: Follow this format for other characters
            default:
                audio.PlayOneShot(LandSFX);
                break;
        }
    }
    public void OnJumpSound(Form f)
    {
        switch (f)
        {
            case Form.original:
                audio.PlayOneShot(JumpSFX, 0.5f);
                break;
            case Form.charger:
                OnChargerJumpSound();
                break;
            // TODO: Follow this format for other characters
            default:
                audio.PlayOneShot(JumpSFX, 0.5f);
                break;
        }
    }
    public void OnDeathSound(Form f)
    {
        if (!DeathSFX) return;// TODO: DELETE
        switch (f)
        {
            case Form.original:
                audio.PlayOneShot(DeathSFX);
                break;
            case Form.charger:
                OnChargerDeathSound();
                break;
            // TODO: Follow this format for other characters
            default:
                audio.PlayOneShot(DeathSFX);
                break;
        }
    }

    public void OnRunSound(Form f)
    {
        switch (f)
        {
            case Form.original:
                break;
            case Form.charger:
                break;
            // TODO: Follow this format for other characters
            default:
                break;
        }
    }

    public void OnWalkSound(Form f)
    {
        switch (f)
        {
            case Form.original:
                break;
            case Form.charger:
                OnChargerWalkSound();
                break;
            // TODO: Follow this format for other characters
            default:
                break;
        }
    }

    public void OnStopWalkSound(Form f)
    {
        switch (f)
        {
            case Form.original:
                break;
            case Form.charger:
                OnStopChargerWalkSound();
                break;
            // TODO: Follow this format for other characters
            default:
                break;
        }
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
        audio.clip = ChargerWalkSFX;
        audio.Play();
    }

    public void OnStopChargerWalkSound()
    {
        audio.Stop();
        audio.clip = null;
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
