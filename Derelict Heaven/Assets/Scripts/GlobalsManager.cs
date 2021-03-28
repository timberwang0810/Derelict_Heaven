using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalsManager : MonoBehaviour
{
    public Slider volumeSlider;
    private static float currentVolume = 1;

    // Initial settings
    private void Start()
    {
        if (SoundManager.S) SoundManager.S.AdjustVolume(currentVolume);
        volumeSlider.value = currentVolume;
    }

    public void OnVolumeAdjusted()
    {
        currentVolume = volumeSlider.value;
        if (SoundManager.S) SoundManager.S.AdjustVolume(currentVolume);
    }
}
