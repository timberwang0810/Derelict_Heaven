using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager S;

    public AudioSource sfxAudio;
    public AudioSource movementAudio;

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

    public AudioClip TorchHitSFX;

    private AudioSource currLevelBGM;

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

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdjustVolume(float volume)
    {
        sfxAudio.volume = volume;
        movementAudio.volume = volume;
        currLevelBGM.volume = volume;
    }

    public void OnNewLevel()
    {
        currLevelBGM = LevelManager.S.levelBGM;
        currLevelBGM.volume = sfxAudio.volume;
    }

    // Angel Sounds
    public void OnLandSound(Player p)
    {
        Debug.Log("fired");
        if (!LandSFX) return;// TODO: DELETE
        switch (p.myForm)
        {
            case Form.original:
                sfxAudio.PlayOneShot(LandSFX);
                break;
            case Form.charger:
                OnChargerLandSound();
                break;
            // TODO: Follow this format for other characters
            default:
                sfxAudio.PlayOneShot(LandSFX);
                break;
        }
    }
    public void OnJumpSound(Form f)
    {
        switch (f)
        {
            case Form.original:
                sfxAudio.PlayOneShot(JumpSFX, 0.5f);
                break;
            case Form.charger:
                OnChargerJumpSound();
                break;
            // TODO: Follow this format for other characters
            default:
                sfxAudio.PlayOneShot(JumpSFX, 0.5f);
                break;
        }
    }
    public void OnDeathSound(Form f)
    {
        if (!DeathSFX) return;// TODO: DELETE
        switch (f)
        {
            case Form.original:
                sfxAudio.PlayOneShot(DeathSFX);
                break;
            case Form.charger:
                OnChargerDeathSound();
                break;
            // TODO: Follow this format for other characters
            default:
                sfxAudio.PlayOneShot(DeathSFX);
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
                OnChargerRunSound();
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

    public void OnStopMovementSound()
    {
        movementAudio.Stop();
        movementAudio.clip = null;
    }

    public void OnConsumeSound()
    {
        sfxAudio.PlayOneShot(ConsumeSFX, 0.5f);
    }
    public void OnUnConsumeSound()
    {
        sfxAudio.PlayOneShot(UnConsumeSFX, 0.5f);
    }

    // Charger Sounds
    public void OnChargerLandSound()
    {
        sfxAudio.PlayOneShot(ChargerLandSFX);
    }
    public void OnChargerJumpSound()
    {
        sfxAudio.PlayOneShot(ChargerJumpSFX);
    }
    public void OnChargerDeathSound()
    {
        sfxAudio.PlayOneShot(ChargerDeathSFX);
    }
    public void OnChargerWalkSound()
    {
        if (movementAudio.clip && movementAudio.clip.Equals(ChargerRunSFX)) return;
        movementAudio.clip = ChargerWalkSFX;
        movementAudio.Play();
    }

    public void OnChargerRunSound()
    {
        movementAudio.clip = ChargerRunSFX;
        movementAudio.Play();
    }

    // Arrow Sounds
    public void OnArrowCharge()
    {
        sfxAudio.PlayOneShot(ArrowChargeSFX);
    }
    public void OnArrowFire()
    {
        sfxAudio.PlayOneShot(ArrowFireSFX);
    }

    public void OnWallBreak()
    {
        sfxAudio.PlayOneShot(WallBreakSFX);
    }

    public void OnHittingTorch()
    {
        sfxAudio.PlayOneShot(TorchHitSFX);
    }
}
