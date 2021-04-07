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

    [Header("Charger Sounds")]
    public AudioClip ChargerRunSFX;
    public AudioClip ChargerWalkSFX;
    public AudioClip ChargerJumpSFX;
    public AudioClip ChargerLandSFX;
    public AudioClip ChargerDeathSFX;

    [Header("Pressurizer Sounds")]
    public AudioClip PressurizerWalkSFX;
    public AudioClip PressurizerUseSFX;

    [Header("Archer Sounds")]
    public AudioClip ArrowChargeSFX;
    public AudioClip ArrowFireSFX;

    public AudioClip WallBreakSFX;

    public AudioClip JumpSFX;
    public AudioClip DeathSFX;
    public AudioClip LandSFX;
    public AudioClip AscendSFX;

    public AudioClip TorchDoorOpen;
    public AudioClip PressureDoorOpen;
    public AudioClip GetKeySFX;

    [Header("Boss Level Sounds")]
    public AudioClip ChainLong1SFX;
    public AudioClip ChainLong2SFX;
    public AudioClip ChainShortSFX;
    public AudioClip BossDeathSFX;
    public AudioClip BossDoorOpenSFX;

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

    public void StopBGM()
    {
        if (currLevelBGM) currLevelBGM.Stop();
    }

    public void PlayBGM()
    {
        if (currLevelBGM) currLevelBGM.Play();
    }

    public void AdjustBGMVolume(float bgmVolume)
    {
        if (currLevelBGM) currLevelBGM.volume = bgmVolume;
    }

    public void AdjustSFXVolume(float sfxVolume)
    {
        sfxAudio.volume = sfxVolume;
        movementAudio.volume = sfxVolume;
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
            case Form.pressurizer:
                OnPressurizerWalkSound();
                break;
            // TODO: Follow this format for other characters
            default:
                break;
        }
    }

    public void OnObtainKeySound()
    {
        sfxAudio.PlayOneShot(GetKeySFX, 0.5f);
    }

    public void OnStopMovementSound()
    {
        movementAudio.Stop();
        movementAudio.clip = null;
    }

    public void OnConsumeSound()
    {
        sfxAudio.PlayOneShot(ConsumeSFX, 0.8f);
    }
    public void OnUnConsumeSound()
    {
        sfxAudio.PlayOneShot(UnConsumeSFX, 0.8f);
    }

    public void OnAscendSound()
    {
        sfxAudio.PlayOneShot(AscendSFX, 0.5f);
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

    public void OnPressurizerWalkSound()
    {
        movementAudio.clip = PressurizerWalkSFX;
        movementAudio.Play();
    }

    public void OnPressurizerUseSound()
    {
        sfxAudio.PlayOneShot(PressurizerUseSFX);
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

    public void TorchDoorSFX()
    {
        sfxAudio.PlayOneShot(TorchDoorOpen, 0.8f);
    }

    public void PressureDoorSFX()
    {
        sfxAudio.PlayOneShot(PressureDoorOpen);
    }

    public void OnChainLong1Fall()
    {
        sfxAudio.PlayOneShot(ChainLong1SFX);
    }
    public void OnChainLong2Fall()
    {
        sfxAudio.PlayOneShot(ChainLong2SFX);
    }
    public void OnChainShortFall()
    {
        sfxAudio.PlayOneShot(ChainShortSFX);
    }
    public void OnBossDeathSound()
    {
        sfxAudio.PlayOneShot(BossDeathSFX);
    }
    public void onBossDoorOpenSound()
    {
        sfxAudio.PlayOneShot(BossDoorOpenSFX);
    }

}
