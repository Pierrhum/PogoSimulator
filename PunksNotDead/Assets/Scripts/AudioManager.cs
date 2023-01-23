using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Type
{
    Gaddem,
    Hurt,
    Charge,
    Fall,
    StartGetUp,
    EndGetUp
};

[System.Serializable]
public class Sound
{
    public Type type;
    public List<AudioClip> SFX;
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
        Sound _sound = SFX.Find(s => s.type.Equals(type));
        AudioClip _SFX;
        if (_sound.SFX.Count > 1)
            _SFX = _sound.SFX[Random.Range(0, _sound.SFX.Count)];
        else
            _SFX = _sound.SFX[0];
        Source.PlayOneShot(_SFX);
    }
}
