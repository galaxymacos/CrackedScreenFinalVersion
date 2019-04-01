using System;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class Sound
{
    public string name;
    
    public AudioClip clip;

    [Range(0f, 1f)] [SerializeField] internal float volume = 0.8f;
    [Range(0.1f, 3f)] [SerializeField] public float pitch = 1f;

    public bool loop;
    
    [HideInInspector]
    public AudioSource source;
}
