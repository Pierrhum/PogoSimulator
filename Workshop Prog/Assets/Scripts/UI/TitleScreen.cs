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
    public List<PlayableDirector> Musics;
    public UIDissolve Overlay;
    public TextMeshProUGUI Title;
    public TMP_Dropdown Songs;
    public Camera PlayerCamera;
    public Camera TitleScreenCamera;
    private Vector3 initTitleScreenCameraPos;
    private Vector3 initTitleScreenCameraEul;
    public GameObject Canvas;

    private int lastSong = 0;

    private void Start()
    {
        HUD.instance.GameHUD.SetActive(false);
        Title.font.material.SetFloat("_FaceDilate",-1);
        Title.font.material.SetFloat("_GlowPower",0);
        initTitleScreenCameraPos = TitleScreenCamera.transform.localPosition;
        initTitleScreenCameraEul = TitleScreenCamera.transform.localEulerAngles;
        StartCoroutine(OpeningCoroutine(1f));
    }

    private IEnumerator OpeningCoroutine(float duration)
    {
        yield return StartCoroutine(Fade(2f));
        float timer = 0f;
        while (timer < duration)
        {
            Title.font.material.SetFloat("_FaceDilate",Mathf.Lerp(-1,0, timer / duration));
            Title.font.material.SetFloat("_GlowPower",Mathf.Lerp(0,0.75f, timer / duration));
            timer += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private IEnumerator Fade(float duration)
    {
        float timer = 0f;
        while (timer < 2f)
        {
            Overlay.width = Overlay.effectFactor = timer / duration;
            timer += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
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
        Musics[Songs.value].Stop();
        Musics[Songs.value].Play();
    }

    public void UpdateScores()
    {
        
    }
    
    public void DropdownValueChanged()
    {
        if (Songs.value < Musics.Count)
        {
            Musics[lastSong].Stop();
            Musics[Songs.value].Play();
            lastSong = Songs.value;
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
        HUD.instance.GameHUD.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
