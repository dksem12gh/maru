using UnityEngine;
using TMPro;
using Doozy.Runtime.UIManager.Containers;

public class MainRhythmCanvasController : MonoBehaviour
{
    [SerializeField] Sprite[] _selectGameImg;
    [SerializeField] Sprite[] _playerCountImg;
    [SerializeField] Sprite[] _popupExitBtnImg;
    [SerializeField] GameObject popParent;
    [SerializeField] GameObject text;

    string mode;
    MainCavasColliderCtrl _colCanvas;
    PopupSelectCanvasBtn _popupCanvasBtn;

    private void Start()
    {
        _colCanvas = this.transform.GetComponent<MainCavasColliderCtrl>();        
    }

    public void SelectFreeMode()
    {
        text.SetActive(false);

        if (popParent.transform.childCount != 0)
        {
            popParent.GetComponentInChildren<UIPopup>().Hide();
        }

        Managers.UI.Popup.ShowRhythmGameSelectPrefap("UiTextLanguage", "FreeMode", "PopupGameFlyerText", "RhythmGameText",
                                        "FreeModeHowTo", _playerCountImg[3],                                        
                                        _selectGameImg[0], onStartClick: RhythmMusicSelectScene, onExitClick: RhythmGameScene);

        Managers.SelGameSet.rhythmMode = RhythmGameMode.FreeMode;        
    }

    public void SelectMovementMode()
    {
        text.SetActive(false);

        if (popParent.transform.childCount != 0)
        {
            popParent.GetComponentInChildren<UIPopup>().Hide();
        }

        Managers.UI.Popup.ShowRhythmGameSelectPrefap("UiTextLanguage", "Movement", "PopupGameFlyerText", "RhythmGameText",
                                                "MovementHowTo", _playerCountImg[1],                                                
                                                _selectGameImg[1], onStartClick: RhythmMusicSelectScene, onExitClick: RhythmGameScene);

        Managers.UI.Signal.SendSignal_GameScene("RhythmGame");
        Managers.SelGameSet.rhythmMode = RhythmGameMode.MovementMode;
        Managers.SelGameSet.playerCount = 2;        
    }

    public void SelectAutoMode()
    {
        text.SetActive(false);
        if (popParent.transform.childCount != 0)
        {
            popParent.GetComponentInChildren<UIPopup>().Hide();
        }

        Managers.UI.Popup.ShowRhythmGameSelectPrefap("UiTextLanguage", "AutoMode", "PopupGameFlyerText", "RhythmGameText",
                                        "AutoModeHowTo", _playerCountImg[1],
                                        _selectGameImg[2], onStartClick: RhythmMusicSelectScene, onExitClick: RhythmGameScene);

        Managers.UI.Signal.SendSignal_GameScene("RhythmGame");        
        Managers.SelGameSet.rhythmMode = RhythmGameMode.AutoMode;
        Managers.SelGameSet.rhythmSpeed = RhythmGameSpeed.level01;
        Managers.SelGameSet.playerCount = 2;        
    }

    void RhythmGameScene(Vector3 pos)
    {
        string gameSelectName = Managers.SelGameSet.gameSelectName;
        string textKey = "";

        switch (gameSelectName)
        {
            case "FreeMode":
                textKey = "FreemodeEnd";
                break;
            case "AutoMode":
                textKey = "AutomodeEnd";
                break;
            case "Movement":
                textKey = "MovementEnd";
                break;            
        }

        string text = Managers.Local.TextString("PopupGameFlyerText", textKey);
        Managers.UI.Popup.ExitPopupPrefab(text, _popupExitBtnImg[0], OnStartClick: ExitBtnAction, OnExitClick: ExitBtn);
    }

    public void ExitBtn(Vector3 pos)
    {
        Managers.CanvasCol.ExitColOn();
        System.GC.Collect();
        _popupCanvasBtn.PopupClose();
    }

    public void ExitBtnAction(Vector3 pos)
    {
        Managers.CanvasCol.ExitColOn();
        _colCanvas.SelectColOn();

        _popupCanvasBtn.ExitSelect();

        Managers.Sound._RhythmMusicSelectSource.clip = null;
        Managers.UI.Signal.SendSignal_GameScene("RhythmGame");
        Managers.UI.Signal.SendSignal("RhythmGame", Managers.SelGameSet.gameSelectName);

        Managers.SubCamera.GameSceneOff();
    }

    public void RhythmMusicSelectScene(Vector3 pos)
    {
        System.GC.Collect();

        _popupCanvasBtn = popParent.transform.GetComponentInChildren<PopupSelectCanvasBtn>();
        _colCanvas.SelectColOff();

        Managers.UI.Signal.SendSignal_GameScene("RhythmGame");

        if (Managers.SelGameSet.gameSelectName == "AutoMode")
        {
            Managers.UI.Signal.SendSignal("RhythmGame", "AutoMode");
            Managers.UI.Popup.RhythmGameMusicSelectPrefab(OnStartClick: RhythmGameStartSend, OnExitClick: RhythmGameBackSend);
        }
        else if (Managers.SelGameSet.gameSelectName == "Movement")
        {            
            Managers.UI.Popup.RhythmGameMusicSelectPrefab(OnStartClick: RhythmGameStartSend, OnExitClick: RhythmGameBackSend);

            Managers.UI.Signal.SendSignal("RhythmGame", "Movement");
        }
        else if(Managers.SelGameSet.gameSelectName == "FreeMode")
        {
            Managers.SelGameSet.selectMusic = null;
            Managers.SelGameSet.rhythmMode = RhythmGameMode.FreeMode;
            Managers.SelGameSet.selectGameState = SelectGameState.Play;
            Managers.SelGameSet.playerCount = 1;
            Managers.UI.Popup.RhythmMusicElementPrefab(Managers.Local.TextString("UiTextLanguage", "FreeMode"));
            Managers.UI.Signal.SendSignal("RhythmGame", "FreeMode");

            Managers.SubCamera.GameSceneOn();
        }        
    }

    public void RhythmLevelSelectScene(Vector3 pos)
    {
        Managers.UI.Popup.RhythmGameLevelSelectPrefab(OnStartClick: RhythmLevelGameStartSend, OnExitClick: RhythmGameBackSend);
    }

    void RhythmLevelGameStartSend(Vector3 pos)
    {
        Managers.UI.Signal.SendSignal("RhythmGame", "LevelSelect");        

        if (Managers.SelGameSet.rhythmMode == RhythmGameMode.MovementMode)
        {
            _popupCanvasBtn = popParent.transform.GetComponentInChildren<PopupSelectCanvasBtn>();
            _popupCanvasBtn.PauseBtnOn();
            mode = Managers.Local.TextString("UiTextLanguage", "Movement");
            Managers.UI.Popup.RhythmMusicElementPrefab(mode, Managers.SelGameSet.selectMusic._musicName);
            Managers.SubCamera.GameSceneOn();
        }        
    }

    void RhythmGameStartSend(Vector3 pos)
    {
        Managers.UI.Signal.SendSignal("RhythmGame","MusicSelect");        

        if(Managers.SelGameSet.rhythmMode == RhythmGameMode.AutoMode)
        {
            _popupCanvasBtn = popParent.transform.GetComponentInChildren<PopupSelectCanvasBtn>();
            Managers.SelGameSet.selectGameState = SelectGameState.Play;           
            mode = Managers.Local.TextString("UiTextLanguage", "AutoMode");
            Managers.UI.Popup.RhythmMusicElementPrefab(mode, Managers.SelGameSet.selectMusic._musicName);
            _popupCanvasBtn.PauseBtnOn();
            Managers.SubCamera.GameSceneOn();
        }
        else if(Managers.SelGameSet.rhythmMode == RhythmGameMode.MovementMode)
        {
            Managers.UI.Signal.SendSignal("RhythmGame", "MovementMode");            
            Managers.SelGameSet.playerCount = 2;
            Managers.UI.Popup.RhythmGameLevelSelectPrefab(OnStartClick: RhythmLevelGameStartSend, OnExitClick: RhythmGameBackSend);
        }
    }
    void RhythmGameBackSend(Vector3 pos)
    {
        _colCanvas.SelectColOn();
        Managers.UI.Signal.SendSignal("RhythmGame", "MusicSelectBack");
    }

    void RhythmRueurnScene(Vector3 pos)
    {
        Managers.UI.Signal.SendSignal_GameScene("Leisure");
    }

    public void PopupSetting()
    {
        Managers.UI.Popup.ShowPrefab("PopupSettingCanvas");
    }

    public void QuitApp()
    {
        string text = Managers.Local.TextString("PopupGameFlyerText", "SystemEnd");
        Managers.UI.Popup.ExitPopupPrefab(text, _popupExitBtnImg[1], OnStartClick: QuitAction);
    }

    public void QuitAction(Vector3 pos)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
