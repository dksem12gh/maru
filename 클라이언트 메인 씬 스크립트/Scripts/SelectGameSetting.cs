using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SelectGameState
{ 
    None = 0,
    Play,
    Pause,
    UnPause,
    Finish
}

public enum RhythmGameSpeed
{
    None = 0,
    level01,
    level02,
    level03
}
public enum RhythmGameMode
{
    None = 0,
    AutoMode,
    MovementMode,
    FreeMode
}

public class SelectGameSetting : MonoBehaviour
{
    public string gameSelectName;

    public int playerCount;

    public int time1;
    public int time2;
    public int time3;
    public int time4;

    public MusicData selectMusic;
    public RhythmGameMode rhythmMode;
    public RhythmGameSpeed rhythmSpeed;
    public SelectGameState selectGameState;

    public void StateClear()
    {
        selectGameState = SelectGameState.None;
    }

    public void DataClear()
    {
        gameSelectName = "Defalut";
        playerCount = 0;
        time1 = 0;
        time2 = 0;
        time3 = 0;
        time4 = 0;
        selectMusic = null;
        rhythmMode = RhythmGameMode.None;
        rhythmSpeed = RhythmGameSpeed.None;
    }
}
