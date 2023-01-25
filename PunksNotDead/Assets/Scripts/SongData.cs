using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[CreateAssetMenu(menuName = "Song")]
public class SongData : ScriptableObject
{
    public string Songname;
    public string Bandname;
    public PlayableAsset Timeline;
}
