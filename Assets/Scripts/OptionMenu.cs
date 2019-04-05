using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;


    public void OnMusicVolumeChange(float _newVol)
    {
        print("Music volume changed");
        audioMixer.SetFloat("MusicVol", _newVol);
    }

    public void OnSfxVolumeChange(float _newVol)
    {
        audioMixer.SetFloat("SfxVol", _newVol);
    }
}