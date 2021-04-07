using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalsManager : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;
    private static float currentBGMVolume = 0.25f;
    private static float currentSFXVolume = 1.0f;

    // Initial settings
    private void Start()
    {
        if (SoundManager.S) SoundManager.S.AdjustBGMVolume(currentBGMVolume);
        bgmSlider.value = currentBGMVolume;
        sfxSlider.value = currentSFXVolume;
    }

    public void OnBGMVolumeAdjusted()
    {
        currentBGMVolume = bgmSlider.value;
        if (SoundManager.S) SoundManager.S.AdjustBGMVolume(currentBGMVolume);
    }

    public void OnSFXVolumeAdjusted()
    {
        currentSFXVolume = sfxSlider.value;
        if (SoundManager.S) SoundManager.S.AdjustSFXVolume(currentSFXVolume);
    }
}
