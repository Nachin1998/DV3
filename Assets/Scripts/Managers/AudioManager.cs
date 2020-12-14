using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Slider sfxSlider;
    public Slider musicSlider;

    public void ChangeSFXVolume()
    {
        AkSoundEngine.SetRTPCValue("SFX_VOLUME", sfxSlider.value);
    }

    public void ChangeMusicVolume()
    {
        AkSoundEngine.SetRTPCValue("MUSIC_VOLUME", musicSlider.value);
    }
}
