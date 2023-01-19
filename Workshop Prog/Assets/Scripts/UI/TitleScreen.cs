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
    public PlayableDirector CameraTimeline;
    public TMP_Dropdown Songs;
    public Camera TitleScreenCamera;
    public GameObject Canvas;
    public List<TMPFontAnimator> FontAnimators;
    
    private Vector3 initTitleScreenCameraPos;
    private Vector3 initTitleScreenCameraEul;

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
        Canvas.SetActive(false);
        ReplayMusic();
        StartCoroutine(GoToPlayerCameraCoroutine(.5f));
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

    private IEnumerator GoToPlayerCameraCoroutine(float duration)
    {
        CameraTimeline.Play();
        yield return new WaitForSeconds((float)CameraTimeline.duration);

        Player.CanPlay = true;
        CameraTimeline.Pause();
        CameraTimeline.time = 0;
        CameraTimeline.Evaluate();
        TitleScreenCamera.enabled = false;
        Player.Camera.enabled = true;
        Player.FaceCamera.enabled = true;
        HUD.instance.GameHUD.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
