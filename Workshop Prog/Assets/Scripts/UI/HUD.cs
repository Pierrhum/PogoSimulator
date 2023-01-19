using System;
using System.Collections;
using System.Collections.Generic;
using Coffee.UIEffects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD instance;
    public EndGameScreen EndGameScreen;
    public GameObject GameHUD;

    [SerializeField] private TextMeshProUGUI Score;
    [SerializeField] private Image HealthBar;

    private void Awake()
    {
        EndGameScreen = GetComponent<EndGameScreen>();
        GameHUD.SetActive(false);
        instance = this;
    }

    public void SetScore(int _score)
    {
        Score.SetText("" + _score);
    }

    public void UpdateHealthBar(float amount)
    {
        HealthBar.fillAmount = amount;
    }

    
}
