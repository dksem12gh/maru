using UnityEngine;

public class Managers : SingletonMono<Managers>
{
    public enum PLAYERCOUNT
    { 
        PLAYER01 = 0,
        PLAYER02
    }

    /*     
     * main(kiosk)씬과 함께 게임씬을 additive 를 하는쪽으로     
     * 메인 카메라는 main씬에서 kiosk캔버스가 가져가니 게임씬에선 메인카메라를 선언하면안됨
     * kiosk캔버스에서 display1 화면을 가져가니 게임씬 캔버스는 display2를 고정적으로 가져가야함.
     */

    UiManager _ui = new UiManager();
    Localize _localize = new Localize();
    [SerializeField] DataManager _data;
    [SerializeField] SoundManager _sound;
    [SerializeField] MainCavasColliderCtrl _col;
    [SerializeField] MainCanvasController _mainCanvasController;
    [SerializeField] PopupColliderManager _popupCol;    
    [SerializeField] SelectGameSetting _selectGameSetting;
    [SerializeField] SubCameraManager _subCameraMgr;

    //리듬씬전용 
    //[SerializeField] MainRhythmCanvasController _mainRhythmCanvasController;


    int _gameTime;
    int _gameLevel = 0;
    public bool _poup = false;

   // public enum PLAYERCOUNT _player;

    public static int GameTime { get { return Instance._gameTime; } set { Instance._gameTime = value; } }
    public static int GameLevel { get { return Instance._gameLevel; } set { Instance._gameLevel = value; } }

    //게임레벨,모드,인원,사운드 등등 받아오는 클래스
    public static SelectGameSetting SelGameSet { get { return Instance._selectGameSetting; } set { Instance._selectGameSetting = value; } }

    public static UiManager UI { get { return Instance._ui; } }
    public static Localize Local { get { return Instance._localize; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static MainCavasColliderCtrl CanvasCol { get { return Instance._col; } }
    public static MainCanvasController CanvasCtrl { get { return Instance._mainCanvasController; } }
    public static DataManager Data { get { return Instance._data; } }
    public static PopupColliderManager PopupCol { get { return Instance._popupCol; } }
    public static SubCameraManager SubCamera { get { return Instance._subCameraMgr; } }

    //public static MainRhythmCanvasController RhythmCavasCtrl { get { return Instance._mainRhythmCanvasController; } }
}
