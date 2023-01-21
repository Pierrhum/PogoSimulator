using System;
using System.Collections;
using System.Collections.Generic;
using Coffee.UIEffects;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    public PlayerController Player;
    public TMP_Dropdown Songs;
    public Camera TitleScreenCamera;
    public GameObject Canvas;
    public List<TMPFontAnimator> FontAnimators;
    
    [SerializeField] private PogoSpawner _spawner;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        GameManager.instance.MusicIndex = Songs.value;
    }

    private void StartFontAnimation(int TMPFontIndex)
    {
            FontAnimators[TMPFontIndex].StartAnimation();
    }    
    private void StopFontAnimation(int TMPFontIndex)
    {
        FontAnimators[TMPFontIndex].StopAnimation();
    }
    public void Play()
    {
        GameManager.instance.MusicIndex = Songs.value;
        AudioManager.instance.Play(Type.Gaddem);
        Cursor.visible = false;
        Canvas.SetActive(false);
        ReplayMusic();
        _animator.SetTrigger("Play");
        
        GameManager.instance.Reset();
        for(int i=0; i < _spawner.MaxAI; i++)
            _spawner.AddPogoGuy();
    }

    public void ReplayMusic()
    {
        GameManager.instance.MusicDirector.Stop();
        GameManager.instance.MusicDirector.Play();
    }

    public void DropdownValueChanged()
    {
        if (Songs.value < GameManager.instance.Musics.Count)
        {
            GameManager.instance.MusicDirector.Stop();
            GameManager.instance.MusicDirector.playableAsset = GameManager.instance.Musics[Songs.value];
            GameManager.instance.MusicDirector.Play();
            GameManager.instance.MusicIndex = Songs.value;
            GameManager.instance.BestScores.UpdateSongScores();
        }
    }

    private void EndPlayAnimEvent()
    {
        Player.CanPlay = true;
        TitleScreenCamera.enabled = false;
        Player.Camera.enabled = true;
        Player.FaceCamera.enabled = true;
        HUD.instance.GameHUD.SetActive(true);
    }

    public void HowToPlay()
    {
        _animator.SetTrigger("HowToPlay");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
