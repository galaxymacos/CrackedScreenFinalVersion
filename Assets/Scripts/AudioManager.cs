using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

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

[Serializable]
public class SoundTrack
{
    public AudioMixerGroup AudioMixerGroup;
    public Sound[] sounds;
}
public class AudioManager : MonoBehaviour
{
    // Whenever a sound of each sound array is playing, the other sounds in the same array will be forced to stop
    private Dictionary<AudioGroup, SoundTrack> soundDictionary;

    public SoundTrack CharacterSounds;
    public SoundTrack FirstBossSounds;
    public SoundTrack SecondBossSounds;
    public SoundTrack UiSounds;
    public SoundTrack BackgroundMusics;
    
    
    
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
        
        soundDictionary = new Dictionary<AudioGroup, SoundTrack>();
        soundDictionary.Add(AudioGroup.Character,CharacterSounds);
        soundDictionary.Add(AudioGroup.Ui,UiSounds);
        soundDictionary.Add(AudioGroup.Bgm, BackgroundMusics);
        soundDictionary.Add(AudioGroup.FirstBoss, FirstBossSounds);
        soundDictionary.Add(AudioGroup.SecondBoss, SecondBossSounds);


        foreach (SoundTrack soundTrack in soundDictionary.Values) // Create one audiosource for each audioGroup
        {
            foreach (Sound sound in soundTrack.sounds)
            {
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.outputAudioMixerGroup = soundTrack.AudioMixerGroup;
                sound.source = audioSource;
            }
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            PlaySound(AudioGroup.Bgm,"ChapterOne");
        }
        if (SceneManager.GetActiveScene().name == "Level2")
        {
            PlaySound(AudioGroup.Bgm,"ChapterTwo");
        }
//        PlaySound(AudioGroup.Bgm,"Chapter One Bgm");
    }

    public void PlaySound(AudioGroup audioGroup, string soundName)
    {
        soundDictionary.TryGetValue(audioGroup,out SoundTrack soundTrack);
        if (soundTrack != null)
            foreach (var sound in soundTrack.sounds)
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
        soundDictionary.TryGetValue(audioGroup,out SoundTrack soundTrack);
        if (soundTrack?.sounds.Length > 0)
        {
            foreach (Sound sound in soundTrack.sounds)
            {
                sound.source.Stop();
            }
            
        }
    }
    
    
}
