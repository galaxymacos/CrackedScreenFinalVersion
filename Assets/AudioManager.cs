using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// This enum contains music group that have only one instance
/// </summary>
public enum AudioGroup
{
    Character,
    Ui,
    Bgm,
    FirstBoss,
    SecondBoss
}
public class AudioManager : MonoBehaviour
{
    // Whenever a sound of each sound array is playing, the other sounds in the same array will be forced to stop
    private Dictionary<AudioGroup, Sound[]> soundDictionary;

    public Sound[] CharacterSounds;
    public Sound[] FirstBossSounds;
    public Sound[] SecondBossSounds;
    public Sound[] UiSounds;
    public Sound[] BackgroundMusics;
    
    
    
    // Start is called before the first frame update

    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        soundDictionary = new Dictionary<AudioGroup, Sound[]>();
        soundDictionary.Add(AudioGroup.Character,CharacterSounds);
        soundDictionary.Add(AudioGroup.Ui,UiSounds);
        soundDictionary.Add(AudioGroup.Bgm, BackgroundMusics);
        soundDictionary.Add(AudioGroup.FirstBoss, FirstBossSounds);
        soundDictionary.Add(AudioGroup.SecondBoss, SecondBossSounds);


        foreach (Sound[] sounds in soundDictionary.Values) // Create one audiosource for each audioGroup
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            foreach (Sound sound in sounds)
            {
                sound.source = audioSource;
            }
        }
    }

    private void Start()
    {
        PlaySound(AudioGroup.Bgm,"Chapter One Bgm");
    }

    public void PlaySound(AudioGroup audioGroup, string soundName)
    {
        soundDictionary.TryGetValue(audioGroup,out var soundArr);
        if (soundArr != null)
            foreach (var sound in soundArr)
            {
     
                    if (sound.name == soundName)
                    {
                        if (!sound.source.isPlaying)
                        {
                            sound.source.clip = sound.clip;
                            sound.source.volume = sound.volume;
                            sound.source.pitch = sound.pitch;
                            sound.source.loop = sound.loop;
                            sound.source.Play();
                        }
                    }
                

            }
    }

    public void StopSound(AudioGroup audioGroup)
    {
        soundDictionary.TryGetValue(audioGroup,out var soundArr);
        soundArr?[0].source.Stop();
    }
    
    
}
