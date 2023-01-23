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
    MortAuxCons = 1,
    
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
    [NonSerialized] public List<SaveData> MortAuxCons;

    private void Awake()
    {
        VivreLibreOuMourir = new List<SaveData>();
        MortAuxCons = new List<SaveData>();
    }
    
    public void UpdateSongScores()
    {
        MusicName _name = (MusicName) GameManager.instance.MusicIndex;
        List<SaveData> SongScores = new List<SaveData>();
        switch (_name)
        {
            case MusicName.VivreLibreOuMourir:
                SongScores = VivreLibreOuMourir;
                break;
            case MusicName.MortAuxCons:
                SongScores = MortAuxCons;
                break;
        }

        for (int rank = 0; rank < 3; rank++)
        {
            if(rank < SongScores.Count)
                SetRank(rank, SongScores[rank].name, SongScores[rank].score);
            else
            {
                if (rank == 0) SetActiveForChilds(First.transform, false);
                else if (rank == 1) SetActiveForChilds(Second.transform, false);
                else SetActiveForChilds(Third.transform, false);
            }
        }
    }

    private void SetActiveForChilds(Transform _transform, bool isActive)
    {
        for(int i=0; i < _transform.childCount; i++)
            _transform.GetChild(i).gameObject.SetActive(isActive);
    }

    private void SetRank(int rank, string name, int score)
    {

        if (rank == 0)
        {
            SetActiveForChilds(First.transform, true);
            SetPlayerName(Player1Name, name);
            Player1Score.text = "" + score;
        }
        else if(rank==1)
        {
            SetActiveForChilds(Second.transform ,true);
            SetPlayerName(Player2Name, name);
            Player2Score.text = "" + score;
        }
        else if(rank==2)
        {
            SetActiveForChilds(Third.transform, true);
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
            case MusicName.MortAuxCons:
                MortAuxCons.Add(_newData);
                MortAuxCons.Sort((s1, s2) => s2.score.CompareTo(s1.score));
                if(MortAuxCons.Count > 3)
                    MortAuxCons.RemoveAt(3);
                break;
            default:
                break;
        }
        SaveManager.instance.SaveGame();
    }

    public void LoadData(SaveGame data)
    {
        VivreLibreOuMourir = data.VivreLibreOuMourir;
        MortAuxCons = data.MortAuxCons;
    }

    public void SaveData(SaveGame data)
    {
        data.VivreLibreOuMourir = VivreLibreOuMourir;
        data.MortAuxCons = MortAuxCons;
    }
}
