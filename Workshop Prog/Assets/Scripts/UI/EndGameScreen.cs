using System;
using System.Collections;
using System.Collections.Generic;
using Coffee.UIEffects;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour
{
    // Game
    public PlayerController Player;
    public Singer Singer;
    public Camera EndCamera;
    private Transform InitEndCameraTransform;
    
    // UI
    public Image Overlay;
    public GameObject TitleScreen;
    public GameObject Canvas;
    public TextMeshProUGUI HighScore;
    public TextMeshProUGUI ScoreText;

    public TextMeshProUGUI Letter1;
    public TextMeshProUGUI Letter2;
    public TextMeshProUGUI Letter3;

    private void Awake()
    {
        Canvas.SetActive(false);
        InitEndCameraTransform = EndCamera.transform;
        EndCamera.enabled = false;
    }

    public IEnumerator EndGameAppearance(int Score, float duration)
    {
        Debug.Log("called");
        HUD.instance.GameHUD.SetActive(false);
        float timer = 0f;
        UITransitionEffect transition = Overlay.GetComponent<UITransitionEffect>();

        while (timer < duration)
        {
            transition.effectFactor = Mathf.Lerp(0f,1f, timer / duration);
            timer += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        transition.effectFactor = 1f;

        HighScore.gameObject.SetActive(GameManager.instance.isBestScore(Score));
        Player.CanPlay = false;
        ScoreText.text = "" + Score;
        Singer.EndState();
        EndCamera.enabled = true;
        Canvas.SetActive(true);
        
        yield return new WaitForSeconds(1f);
        
        timer = 0f;
        while (timer < duration)
        {
            transition.effectFactor = Mathf.Lerp(1f,0f, timer / duration);
            timer += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        transition.effectFactor = 0f;
        
    }

    public void MainMenuButton()
    {
        // Save Data
        int Score = Int32.Parse(ScoreText.text);
        string PlayerName = Letter1.text + Letter2.text + Letter3.text;
        GameManager.instance.BestScores.AddBestScore((MusicName)GameManager.instance.MusicIndex, new SaveData(PlayerName, Score));
        StartCoroutine(MainMenuCoroutine());
    }

    private IEnumerator MainMenuCoroutine()
    {
        GameManager.instance.Reset();
        GameManager.instance.BestScores.UpdateSongScores();
        TitleScreen.GetComponent<TitleScreen>().ReplayMusic();
        Canvas.SetActive(false);
        EndCamera.GetComponent<PlayableDirector>().Play();
        float f = (float)EndCamera.GetComponent<PlayableDirector>().duration;
        yield return new WaitForSeconds(f);
        
        EndCamera.GetComponent<PlayableDirector>().Pause();
        EndCamera.GetComponent<PlayableDirector>().time = 0;
        EndCamera.GetComponent<PlayableDirector>().Evaluate();
        EndCamera.enabled = false;
        TitleScreen.GetComponent<TitleScreen>().Canvas.SetActive(true);
        TitleScreen.GetComponent<TitleScreen>().TitleScreenCamera.enabled = true;
    }
}
