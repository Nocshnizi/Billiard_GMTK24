using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    [SerializeField] private AudioMixerGroup musicMixer;
    [SerializeField] private AudioMixerGroup soundMixer;
    [SerializeField] private AudioMixerGroup effectMixer;

    public void ChangeMusicVolume(float vol)
    {
        musicMixer.audioMixer.SetFloat("MusicVolume", Mathf.Lerp(-30, 1, vol));
    }

    public void ChangeSoundVolume(float vol) {
        soundMixer.audioMixer.SetFloat("SoundVolume", Mathf.Lerp(-30, 1, vol));
        effectMixer.audioMixer.SetFloat("EffectVolume", Mathf.Lerp(-30, 1, vol));
    }
}
