using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Experimental.PlayerLoop;
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

    internal string currentBgm = "";
    internal string prevBgm = "";

    public void ChangeBgm(string newBgm) {
//        StopSound(AudioGroup.Bgm);
        StartAllBackgroundSounds();
        prevBgm = currentBgm;
        currentBgm = newBgm;
    }

    private void Start()
    {
        StartAllBackgroundSounds();

        if (SceneManager.GetActiveScene().name == "LevelTutorial")
        {
            ChangeBgm("Tutorial");
        }
        else if (SceneManager.GetActiveScene().name == "Level1")
        {
            ChangeBgm("ChapterOneBegin");
            print("switch to chapter one bgm");

        }
        else if (SceneManager.GetActiveScene().name == "Level2")
        {
            ChangeBgm("ChapterTwoBegin");
        }
    }

    private void StartAllBackgroundSounds()
    {
        for (int i = 0; i < soundDictionary[AudioGroup.Bgm].sounds.Length; i++)
        {
            PlaySound(AudioGroup.Bgm, soundDictionary[AudioGroup.Bgm].sounds[i].name);
            soundDictionary[AudioGroup.Bgm].sounds[i].source.volume = 0f;
        }
    }

    private void Update()
    {
        UpdateBgm();
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

    private string targetBgm;


    public void UpdateBgm()
    {
        for (int i = 0; i < BackgroundMusics.sounds.Length; i++)
        {
            if (BackgroundMusics.sounds[i].name == prevBgm)
            {
                if (BackgroundMusics.sounds[i].source.volume > 0)
                {
                    BackgroundMusics.sounds[i].source.volume -= Time.deltaTime/8;
                }
            }

            if (BackgroundMusics.sounds[i].name == currentBgm) {
                if (BackgroundMusics.sounds[i].source.volume < BackgroundMusics.sounds[i].volume) {
                    BackgroundMusics.sounds[i].source.volume += Time.deltaTime / 8;
                }
            }
        }
    }
    
    
}
