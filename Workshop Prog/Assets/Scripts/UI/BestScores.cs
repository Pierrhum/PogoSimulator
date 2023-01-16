using System;
using System.Collections;
using System.Collections.Generic;
using Coffee.UIEffects;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public enum MusicName
{
    VivreLibreOuMourir = 0,
    
}

public class BestScores : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject First;
    [SerializeField] private GameObject Second;
    [SerializeField] private GameObject Third;
    
    [SerializeField] private List<UIHsvModifier> Player1Name;
    [SerializeField] private List<UIHsvModifier> Player2Name;
    [SerializeField] private List<UIHsvModifier> Player3Name;

    [SerializeField] private TextMeshProUGUI Player1Score;
    [SerializeField] private TextMeshProUGUI Player2Score;
    [SerializeField] private TextMeshProUGUI Player3Score;

    [NonSerialized] public List<SaveData> VivreLibreOuMourir;

    private void Awake()
    {
        VivreLibreOuMourir = new List<SaveData>();
    }
    
    private void Start()
    {
        UpdateSongScores();
    }
    

    public void UpdateSongScores()
    {
        MusicName _name = (MusicName) GameManager.instance.MusicIndex;
        switch (_name)
        {
            case MusicName.VivreLibreOuMourir:
                for(int rank=0; rank < 3; rank++)
                    if(rank < VivreLibreOuMourir.Count)
                        SetRank(rank, VivreLibreOuMourir[rank].name, VivreLibreOuMourir[rank].score);
                    else {
                        if(rank == 0) First.SetActive(false);
                        else if (rank == 1) Second.SetActive(false);
                        else Third.SetActive(false);
                    }
                break;
        }
    }

    private void SetRank(int rank, string name, int score)
    {

        if (rank == 0)
        {
            First.SetActive(true);
            SetPlayerName(Player1Name, name);
            Player1Score.text = "" + score;
        }
        else if(rank==1)
        {
            Second.SetActive(true);
            SetPlayerName(Player2Name, name);
            Player2Score.text = "" + score;
        }
        else if(rank==2)
        {
            Third.SetActive(true);
            SetPlayerName(Player3Name, name);
            Player3Score.text = "" + score;
        }
        
    }

    private void SetPlayerName(List<UIHsvModifier> modifiers, string name)
    {
        modifiers.ForEach(hsv =>
        {
            hsv.hue = Random.Range(-0.5f, 0.5f);
            hsv.saturation = Random.Range(-0.3f, 0.3f);
            hsv.value = Random.Range(-0.1f, 0.1f);
            hsv.gameObject.transform.eulerAngles = new Vector3(0, 0, Random.Range(-35f, 35f));
        });
        modifiers[0].GetComponentInChildren<TextMeshProUGUI>().text = "" + name[0];
        modifiers[1].GetComponentInChildren<TextMeshProUGUI>().text = "" + name[1];
        modifiers[2].GetComponentInChildren<TextMeshProUGUI>().text = "" + name[2];
    }

    public void AddBestScore(MusicName _name, SaveData _newData)
    {
        switch (_name)
        {
            case MusicName.VivreLibreOuMourir:
                VivreLibreOuMourir.Add(_newData);
                VivreLibreOuMourir.Sort((s1, s2) => s2.score.CompareTo(s1.score));
                if(VivreLibreOuMourir.Count > 3)
                    VivreLibreOuMourir.RemoveAt(3);
                break;
            default:
                break;
        }
        SaveManager.instance.SaveGame();
    }

    public void LoadData(SaveGame data)
    {
        VivreLibreOuMourir = data.VivreLibreOuMourir;
    }

    public void SaveData(SaveGame data)
    {
        data.VivreLibreOuMourir = VivreLibreOuMourir;
    }
}
