using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveGame
{
    public List<SaveData> VivreLibreOuMourir;
    public List<SaveData> MortAuxCons;

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public SaveGame()
    {
        VivreLibreOuMourir = new List<SaveData>();
        MortAuxCons = new List<SaveData>();
    }
}

[System.Serializable]
public struct SaveData
{
    public string name;
    public int score;

    public SaveData(string _name, int _score)
    {
        this.name = _name;
        this.score = _score;
    }
};