using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ticket : MonoBehaviour
{
    public List<SongData> Songs;
    public TextMeshProUGUI Bandname;
    public TextMeshProUGUI Songname;
    public TextMeshProUGUI Month;
    public TextMeshProUGUI Day;
    public TextMeshProUGUI Year;

    private List<string> MonthName = new List<string>() { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "NOV", "DEC"};

    private void Start()
    {
        UpdateTicket(GameManager.instance.MusicIndex);
        GameManager.instance.ReplayMusic();
    }

    public void UpdateTicket(int SongID)
    {
        SongData CurrentSong = Songs[SongID];
        
        // Song data
        Bandname.text = CurrentSong.Bandname;
        Songname.text = CurrentSong.Songname;
        
        // Date
        DateTime date = DateTime.Today;
        Month.text = MonthName[date.Month];
        Day.text = "" + date.Day;
        Year.text = "" + date.Year;
    }

    public void NextTicket()
    {
        if (GameManager.instance.MusicIndex+1 < Songs.Count)
        {
            GameManager.instance.MusicIndex++;
            GameManager.instance.ReplayMusic();
            UpdateTicket(GameManager.instance.MusicIndex);
            GameManager.instance.BestScores.UpdateSongScores();
        }
    }
    
    public void PreviousTicket()
    {
        if (GameManager.instance.MusicIndex-1 >= 0)
        {
            GameManager.instance.MusicIndex--;
            GameManager.instance.ReplayMusic();
            UpdateTicket(GameManager.instance.MusicIndex);
            GameManager.instance.BestScores.UpdateSongScores();
        }
    }
}
