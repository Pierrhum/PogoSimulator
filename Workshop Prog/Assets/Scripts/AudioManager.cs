using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type {Gaddem};

[System.Serializable]
public class Sound
{
    public Type type;
    public AudioClip SFX;
}

public class AudioManager : MonoBehaviour
{
    private AudioSource Source;
    public List<Sound> SFX;
    public static AudioManager instance;

    private void Awake()
    {
        Source = GetComponent<AudioSource>();

        instance = this;
    }

    public void Play(Type type)
    {
        AudioClip sound = SFX.Find(s => s.type.Equals(type)).SFX;
        Source.PlayOneShot(sound);
    }
}
