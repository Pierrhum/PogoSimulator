using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public BestScores BestScores;

    public PlayerController Player;
    public PlayableDirector MusicDirector;
    public List<SongData> Songs;
    public List<GameObject> StageGuys;
    public List<NPCDancer> Public;
    public List<PogoAI> PogoGuys;
    public bool Freezed = false;
    [NonSerialized] public int MusicIndex;
    

    private void Awake()
    {
        Public = new List<NPCDancer>();
        PogoGuys = new List<PogoAI>();
        Cursor.lockState = CursorLockMode.Confined;
        instance = this;
    }

    public void FreezePeople()
    {
        MusicDirector.Pause();
        StageGuys.ForEach(s => s.GetComponent<Animator>().speed = 0);
        Public.ForEach(p => p.GetComponent<Animator>().speed = 0);
        PogoGuys.ForEach(p =>
        {
            p.GetComponent<Animator>().speed = 0;
            p.GetComponent<NavMeshAgent>().isStopped = true;
        });
        
        Freezed = true;
    }

    public void UnFreezePeople()
    {
        MusicDirector.Resume();
        StageGuys.ForEach(s => s.GetComponent<Animator>().speed = 1);
        Public.ForEach(p => p.GetComponent<Animator>().speed = 1);
        PogoGuys.ForEach(p =>
        {
            p.GetComponent<Animator>().speed = 1;
            p.GetComponent<NavMeshAgent>().isStopped = false;
        });
        
        Freezed = false;
    }

    public void EndGame()
    {
        FreezePeople();
        StartCoroutine(HUD.instance.EndGameScreen.EndGameAppearance(Player.Score, 1f));
    }

    public bool isBestScore(int Score)
    {
        return true;
    }

    public void Reset()
    {
        Player.Reset();
        PogoGuys.ForEach(p =>
        {
            p.StopAllCoroutines();
            Destroy(p.gameObject);
        });
        PogoGuys.Clear();
        UnFreezePeople();
    }

    public void ReplayMusic()
    {
        MusicDirector.Stop();
        MusicDirector.playableAsset = Songs[MusicIndex].Timeline;
        MusicDirector.Play();
    }
}
