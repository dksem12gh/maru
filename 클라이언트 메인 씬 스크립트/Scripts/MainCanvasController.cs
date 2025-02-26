using UnityEngine;
using TMPro;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine.Video;
using System;

public class MainCanvasController : MonoBehaviour
{
    [SerializeField] Sprite[] _selectGameImg;
    [SerializeField] Sprite[] _playerCountImg;
    [SerializeField] Sprite[] _popupExitBtnImg;
    [SerializeField] VideoClip[] _videoClip;
    [SerializeField] GameObject popParent;
    [SerializeField] UIPopup _popParentUI;

    string mode;
    MainCavasColliderCtrl _colCanvas;
    PopupSelectCanvasBtn _popupCanvasBtn;
    private void Awake()
    {
        _colCanvas = this.transform.GetComponent<MainCavasColliderCtrl>();        
    }


    //시간,횟수,길이 등 게임시작 전 팝업
    void MiniScene(Vector3 pos)
    {
        int num1 = Managers.SelGameSet.time1;
        int num2 = Managers.SelGameSet.time2;
        int num3 = Managers.SelGameSet.time3;

        Managers.UI.Popup.ShowGameMiniSelectPrefap(num1, num2, num3, "SelectLevel", "Minute", OnStartClick: StartScene);
    }
    void MiniLenthScene(Vector3 pos)
    {
        int num1 = Managers.SelGameSet.time1;
        int num2 = Managers.SelGameSet.time2;
        int num3 = Managers.SelGameSet.time3;

        Managers.UI.Popup.ShowGameMiniSelectPrefap(num1, num2, num3, "SelectLength", "Minute", OnStartClick: StartScene);
    }

    void CountMiniScene(Vector3 pos)
    {
        int num1 = Managers.SelGameSet.time1;
        int num2 = Managers.SelGameSet.time2;
        int num3 = Managers.SelGameSet.time3;

        Managers.UI.Popup.ShowGameMiniSelectPrefap(num1, num2, num3, "CountSelect", "Count", OnStartClick: StartScene);
    }

    void LevelMiniScene(Vector3 pos)
    {
        int num1 = Managers.SelGameSet.time1;
        int num2 = Managers.SelGameSet.time2;
        int num3 = Managers.SelGameSet.time3;

        Managers.UI.Popup.ShowGameMiniSelectPrefap(num1, num2, num3, "SelectLevel", "Level", OnStartClick: StartScene);
    }

    void TimeScene(Vector3 pos)
    {
        int num1 = Managers.SelGameSet.time1;
        int num2 = Managers.SelGameSet.time2;
        int num3 = Managers.SelGameSet.time3;
        int num4 = Managers.SelGameSet.time4;

        Managers.UI.Popup.ShowGameTimeSelectPrefap(num1, num2, num3, num4, "TimeSelect", "Minute", OnStartClick: StartScene);
    }

    void SeconScene(Vector3 pos)
    {
        int num1 = Managers.SelGameSet.time1;
        int num2 = Managers.SelGameSet.time2;
        int num3 = Managers.SelGameSet.time3;
        int num4 = Managers.SelGameSet.time4;

        Managers.UI.Popup.ShowGameSeconSelectPrefap(num1, num2, num3, num4, "SeconSelect", "Minute", OnStartClick: StartScene);
    }

    void CountScene(Vector3 pos)
    {
        int num1 = Managers.SelGameSet.time1;
        int num2 = Managers.SelGameSet.time2;
        int num3 = Managers.SelGameSet.time3;
        int num4 = Managers.SelGameSet.time4;

        Managers.UI.Popup.ShowGameTimeSelectPrefap(num1, num2, num3, num4, "CountSelect", "Count", OnStartClick: StartScene);
    }
    void LengthMScene(Vector3 pos)
    {
        int num1 = Managers.SelGameSet.time1;
        int num2 = Managers.SelGameSet.time2;
        int num3 = Managers.SelGameSet.time3;
        int num4 = Managers.SelGameSet.time4;

        Managers.UI.Popup.ShowGameTimeSelectPrefap(num1, num2, num3, num4, "SelectLength", "LengthM", OnStartClick: StartScene);
    }

    void Language(Vector3 pos)
    {
        int num1 = Managers.SelGameSet.time1;
        int num2 = Managers.SelGameSet.time2;
        int num3 = Managers.SelGameSet.time3;

        Managers.UI.Popup.ShowGameLanguageSelectPrefap(num1, num2, num3, "SelectLanguage", "Language", OnStartClick: StartScene);
    }
    //월욜날순서대로정리 이미지
    void LengthCMScene(Vector3 pos)
    {
        int num1 = Managers.SelGameSet.time1;
        int num2 = Managers.SelGameSet.time2;
        int num3 = Managers.SelGameSet.time3;
        int num4 = Managers.SelGameSet.time4;

        Managers.UI.Popup.ShowGameTimeSelectPrefap(num1, num2, num3, num4, "SelectLength", "LengthCM", OnStartClick: StartScene);
    }

    //시작
    void StartScene(Vector3 pos)
    {
        _popupCanvasBtn = popParent.transform.GetComponentInChildren<PopupSelectCanvasBtn>();
        _popupCanvasBtn.PauseBtnOn();
        Managers.SubCamera.GameSceneOn();
        Managers.CanvasCol.ExitColOn();
        Managers.UI.Signal.SendSignal("RestartGame", Managers.SelGameSet.gameSelectName);
        Managers.SelGameSet.selectGameState = SelectGameState.Play;

        /*if (FirebaseAuthManager.Instance.UserId == null)
        {
            
        }
        else
        {

        }*/
    }

    // Exercise
    #region     

    public void SelectWalk()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 3;
        Managers.SelGameSet.time3 = 5;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "Walk", "PopupGameFlyerText", "WalkText",
                                                "WalkHowTo", _playerCountImg[0],
                                                _selectGameImg[0], _videoClip[0],
                                                onStartClick: CountScene, onExitClick: ExitGameScene);
    }
    public void SelectLeftandrightjumping()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 3;
        Managers.SelGameSet.time3 = 5;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "Leftandrightjumping", "PopupGameFlyerText", "LeftandrightjumpingText",
                                                "LeftandrightjumpingHowTo", _playerCountImg[1],
                                                _selectGameImg[1], _videoClip[1], 
                                                onStartClick: CountScene, onExitClick: ExitGameScene);
    }

    public void SelectRunningInPlace()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 3;
        Managers.SelGameSet.time3 = 5;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "RunningInPlace", "PopupGameFlyerText", "RunningInPlaceText",
                                                "RunningInPlaceHowTo", _playerCountImg[1],
                                                _selectGameImg[2], _videoClip[2],
                                                onStartClick: TimeScene, onExitClick: ExitGameScene);
    }

    public void SelectSideStep()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 3;
        Managers.SelGameSet.time3 = 5;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "SideStep", "PopupGameFlyerText", "SideStepText",
                                                "SideStepHowTo", _playerCountImg[1],
                                                _selectGameImg[3], _videoClip[3], 
                                                onStartClick: TimeScene, onExitClick: ExitGameScene);
    }

    public void SelectRoundTrip()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 3;
        Managers.SelGameSet.time3 = 5;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "RoundTrip", "PopupGameFlyerText", "RoundTripText",
                                                "RoundTripHowTo", _playerCountImg[1],
                                                _selectGameImg[4], _videoClip[4],
                                                onStartClick: TimeScene, onExitClick: ExitGameScene);
    }

    public void SelectCatchUp()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 3;
        Managers.SelGameSet.time3 = 5;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "CatchUp", "PopupGameFlyerText", "CatchUpText",
                                                "CatchUpHowTo", _playerCountImg[0],
                                                _selectGameImg[5], _videoClip[5], 
                                                onStartClick: TimeScene, onExitClick: ExitGameScene);
    }

    public void SelectSidetoSideJump()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 3;
        Managers.SelGameSet.time3 = 5;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "Sidetosidejump", "PopupGameFlyerText", "Side to Side Jump txt",
                                                "Side to Side Jump Howto", _playerCountImg[1],
                                                _selectGameImg[6], _videoClip[6],
                                                onStartClick: TimeScene, onExitClick: ExitGameScene);
    }

    public void SelectJumpbackandforth()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 3;
        Managers.SelGameSet.time3 = 5;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "Jumpbackandforth", "PopupGameFlyerText", "JumpbackandforthText",
                                                "JumpbackandforthHowto", _playerCountImg[1],
                                                _selectGameImg[7], _videoClip[7], 
                                                onStartClick: TimeScene, onExitClick: ExitGameScene);
    }

    public void SelectZigzagRunning()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 3;
        Managers.SelGameSet.time3 = 5;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "ZigzagRunning", "PopupGameFlyerText", "ZigzagRunningText",
                                                "ZigzagRunningHowTo", _playerCountImg[1],
                                                _selectGameImg[8], _videoClip[8],
                                                onStartClick: TimeScene, onExitClick: ExitGameScene);
    }
    public void SelectZigzagRunning2()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 3;
        Managers.SelGameSet.time3 = 5;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "ZigzagRunning2", "PopupGameFlyerText", "ZigzagRunning2Text",
                                                "ZigzagRunning2HowTo", _playerCountImg[1],
                                                _selectGameImg[9], _videoClip[8],
                                                onStartClick: TimeScene, onExitClick: ExitGameScene);
    }

    public void SelectLiedownandchangefeet()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 3;
        Managers.SelGameSet.time3 = 5;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "Liedownandchangefeet", "PopupGameFlyerText", "Lie down and change feetText",
                                                "Lie down and change feetHowto", _playerCountImg[1],
                                                _selectGameImg[10], _videoClip[9],
                                                onStartClick: TimeScene, onExitClick: ExitGameScene);
    }

    public void SelectSitUp()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 3;
        Managers.SelGameSet.time3 = 5;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "SitUp", "PopupGameFlyerText", "Sit UpText",
                                                "Sit UpHowto", _playerCountImg[1],
                                                _selectGameImg[11], _videoClip[10],
                                                onStartClick: TimeScene, onExitClick: ExitGameScene);
    }
    public void SelectJumpingJacks()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 3;
        Managers.SelGameSet.time3 = 5;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "JumpingJacks", "PopupGameFlyerText", "Jumping JacksText",
                                                "Jumping JacksHowto", _playerCountImg[1],
                                                _selectGameImg[12], _videoClip[11],
                                                onStartClick: CountScene, onExitClick: ExitGameScene);
    }

    public void SelectTheactionofgoingupanddown()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 3;
        Managers.SelGameSet.time3 = 5;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "Theactionofgoingupanddown", "PopupGameFlyerText", "TheactionofgoingupanddownText",
                                                "TheactionofgoingupanddownHowto", _playerCountImg[1],
                                                _selectGameImg[13], _videoClip[12],
                                                onStartClick: TimeScene, onExitClick: ExitGameScene);
    }

    public void SelectSupinelegraise()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 3;
        Managers.SelGameSet.time3 = 5;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "Supinelegraise", "PopupGameFlyerText", "Supine leg raiseText",
                                                "Supine leg raiseHowto", _playerCountImg[1],
                                                _selectGameImg[14], _videoClip[13],
                                                onStartClick: CountScene, onExitClick: ExitGameScene);
    }
    public void SelectZigzagJump()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 3;
        Managers.SelGameSet.time3 = 5;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "ZigzagJump", "PopupGameFlyerText", "Zigzag JumpText",
                                                "Zigzag JumpHowto", _playerCountImg[1],
                                                _selectGameImg[15], _videoClip[14],
                                                onStartClick: TimeScene, onExitClick: ExitGameScene);
    }
    public void SelectArmWarking()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 3;
        Managers.SelGameSet.time3 = 5;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "ArmWarking", "PopupGameFlyerText", "Arm WarkingText",
                                                "Arm WarkingHowto", _playerCountImg[1],
                                                _selectGameImg[16], _videoClip[15],
                                                onStartClick: TimeScene, onExitClick: ExitGameScene);
    }
    #endregion // Exercise

    //Stretching
    #region
    public void SelectPlank()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 3;
        Managers.SelGameSet.time3 = 5;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "Plank", "PopupGameFlyerText", "PlankText",
                                                "PlankHowTo", _playerCountImg[1],
                                                _selectGameImg[17], _videoClip[16],
                                                onStartClick: TimeScene, onExitClick: ExitGameScene);
    }
    public void SelectLunge()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 3;
        Managers.SelGameSet.time3 = 5;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "Lunge", "PopupGameFlyerText", "LungeText",
                                                "LungeHowTo", _playerCountImg[1],
                                                _selectGameImg[18], _videoClip[17],
                                                onStartClick: CountScene, onExitClick: ExitGameScene);
    }

    public void SelectBurpee()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 3;
        Managers.SelGameSet.time3 = 5;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "Burpee", "PopupGameFlyerText", "BurpeeText",
                                                "BurpeeHowTo", _playerCountImg[1],
                                                _selectGameImg[19], _videoClip[18],
                                                onStartClick: CountScene, onExitClick: ExitGameScene);
    }

    public void SelectBalanceonefoot()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 3;
        Managers.SelGameSet.time3 = 5;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "Balanceonefoot", "PopupGameFlyerText", "Balance one foot txt",
                                                "Balance one foot Howto", _playerCountImg[1],
                                                _selectGameImg[20], _videoClip[19],
                                                onStartClick: TimeScene, onExitClick: ExitGameScene);
    }
    public void SelectSitdownandbalance()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 3;
        Managers.SelGameSet.time3 = 5;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "Sitdownandbalance", "PopupGameFlyerText", "Sit down and balance txt",
                                                "Sit down and balance Howto", _playerCountImg[1],
                                                _selectGameImg[21], _videoClip[20],
                                                onStartClick: TimeScene, onExitClick: ExitGameScene);
    }
    public void SelectMaintainingflexibility()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 3;
        Managers.SelGameSet.time3 = 5;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "Maintainingflexibility", "PopupGameFlyerText", "Maintaining flexibility txt",
                                                "Maintaining flexibility Howto", _playerCountImg[1],
                                                _selectGameImg[22], _videoClip[21],
                                                onStartClick: TimeScene, onExitClick: ExitGameScene);
    }

    public void SelectCrossArmLeg()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 3;
        Managers.SelGameSet.time3 = 5;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "Crossarmleg", "PopupGameFlyerText", "Cross arm legText",
                                                "Cross arm legHowTo", _playerCountImg[1],
                                                _selectGameImg[23], _videoClip[22],
                                                onStartClick: CountScene, onExitClick: ExitGameScene);
    }

    public void SelectGastrocnemius()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 3;
        Managers.SelGameSet.time3 = 5;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "Gastrocnemius", "PopupGameFlyerText", "GastrocnemiusText",
                                                "GastrocnemiusHowTo", _playerCountImg[1],
                                                _selectGameImg[24], _videoClip[23],
                                                onStartClick: CountScene, onExitClick: ExitGameScene);
    }

    public void SelectDothesplits()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 2;
        Managers.SelGameSet.time3 = 3;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "Dothesplits", "PopupGameFlyerText", "DothesplitsText",
                                                "DothesplitsHowto", _playerCountImg[1],
                                                _selectGameImg[25], _videoClip[24],
                                                onStartClick: TimeScene, onExitClick: ExitGameScene);
    }

    public void SelectDothemiddlesplits()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 2;
        Managers.SelGameSet.time3 = 3;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "Dothemiddlesplits", "PopupGameFlyerText", "DothemiddlesplitsText",
                                                "DothemiddlesplitsHowto", _playerCountImg[1],
                                                _selectGameImg[26], _videoClip[25],
                                                onStartClick: TimeScene, onExitClick: ExitGameScene);
    }

    public void SelectGettingdownononeknees()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 2;
        Managers.SelGameSet.time3 = 3;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "Gettingdownononeknees", "PopupGameFlyerText", "Getting down on one kneesText",
                                                "Getting down on one kneesHowto", _playerCountImg[1],
                                                _selectGameImg[27], _videoClip[26],
                                                onStartClick: TimeScene, onExitClick: ExitGameScene);
    }
    public void SelectBoundanglePose()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 2;
        Managers.SelGameSet.time3 = 3;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "BoundanglePose", "PopupGameFlyerText", "Bound angle PoseText",
                                                "Bound angle PoseHowto", _playerCountImg[1],
                                                _selectGameImg[28], _videoClip[27],
                                                onStartClick: TimeScene, onExitClick: ExitGameScene);
    }

    public void SelectOnefootplank()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 2;
        Managers.SelGameSet.time3 = 3;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "Onefootplank", "PopupGameFlyerText", "One foot plankText",
                                                "One foot plankHowto", _playerCountImg[1],
                                                _selectGameImg[29], _videoClip[28],
                                                onStartClick: TimeScene, onExitClick: ExitGameScene);
    }

    public void SelectCobraPose()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 2;
        Managers.SelGameSet.time3 = 3;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "CobraPose", "PopupGameFlyerText", "Cobra PoseText",
                                                "Cobra PoseHowto", _playerCountImg[1],
                                                _selectGameImg[30], _videoClip[29],
                                                onStartClick: TimeScene, onExitClick: ExitGameScene);
    }
    public void SelectPuppyPose()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 2;
        Managers.SelGameSet.time3 = 3;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "PuppyPose", "PopupGameFlyerText", "Puppy PoseText",
                                                "Puppy PoseHowto", _playerCountImg[1],
                                                _selectGameImg[31], _videoClip[30],
                                                onStartClick: TimeScene, onExitClick: ExitGameScene);
    }

    public void SelectPlowpose()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 2;
        Managers.SelGameSet.time3 = 3;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "Plowpose", "PopupGameFlyerText", "Plow poseText",
                                                "Plow poseHowto", _playerCountImg[1],
                                                _selectGameImg[32], _videoClip[31],
                                                onStartClick: TimeScene, onExitClick: ExitGameScene);
    }

    public void SelectLyingdownandwalking()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 2;
        Managers.SelGameSet.time3 = 3;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "Lyingdownandwalking", "PopupGameFlyerText", "Lying down and walkingText",
                                                "Lying down and walkingHowto", _playerCountImg[1],
                                                _selectGameImg[33], _videoClip[32],
                                                onStartClick: TimeScene, onExitClick: ExitGameScene);
    }
    #endregion

    //Leisure
    #region
    public void SelectPlanetSaver()
    {
        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "PlanetSaver", "PopupGameFlyerText", "PlanetSaverText",
                                                "PlanetSaverHowTo", _playerCountImg[1],
                                                _selectGameImg[34], onStartClick: StartScene, onExitClick: ExitGameScene);
    }

    public void SelectSequentialStep()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 2;
        Managers.SelGameSet.time3 = 3;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "SequentialStep", "PopupGameFlyerText", "SequentialStepText",
                                                "SequentialStepHowTo", _playerCountImg[1],
                                                _selectGameImg[35], onStartClick: Language, onExitClick: ExitGameScene);
    }

    public void SelectAlacrityTest()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 2;
        Managers.SelGameSet.time3 = 3;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "AlacrityTest", "PopupGameFlyerText", "AlacrityTestText",
                                                "AlacrityTestHowTo", _playerCountImg[1],
                                                _selectGameImg[36], onStartClick: MiniScene, onExitClick: ExitGameScene);
    }
    public void SelectFindPictuer()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 3;
        Managers.SelGameSet.time3 = 5;
        Managers.SelGameSet.time4 = 10;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "FindPictuer", "PopupGameFlyerText", "FindPictuerText",
                                                "FindPictuerHowTo", _playerCountImg[1],
                                                _selectGameImg[37], onStartClick: SeconScene, onExitClick: ExitGameScene);
    }

    public void SelectAvoidObstacles()
    {
        Managers.SelGameSet.time1 = 100;
        Managers.SelGameSet.time2 = 150;
        Managers.SelGameSet.time3 = 200;
        Managers.SelGameSet.time4 = -1;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "AvoidObstacles", "PopupGameFlyerText", "AvoidObstaclesText",
                                                "AvoidObstaclesHowTo", _playerCountImg[1],
                                                _selectGameImg[38], onStartClick: MiniLenthScene, onExitClick: ExitGameScene);
    }

    public void SelectColorMatching()
    {
        Managers.SelGameSet.time1 = 5;
        Managers.SelGameSet.time2 = 10;
        Managers.SelGameSet.time3 = 15;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "ColorMatching", "PopupGameFlyerText", "ColorMatchingText",
                                                "ColorMatchingHowto", _playerCountImg[1],
                                                _selectGameImg[39], onStartClick: CountMiniScene, onExitClick: ExitGameScene);
    }

    public void SelectRockPaperScissors()
    {
        Managers.SelGameSet.time1 = 5;
        Managers.SelGameSet.time2 = 10;
        Managers.SelGameSet.time3 = 15;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "RockPaperScissors", "PopupGameFlyerText", "RockPaperScissorsText",
                                                "RockPaperScissorsHowto", _playerCountImg[1],
                                                _selectGameImg[40], onStartClick: CountMiniScene, onExitClick: ExitGameScene);
    }
    public void SelectFlagUpDown()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 2;
        Managers.SelGameSet.time3 = 3;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "FlagUpDown", "PopupGameFlyerText", "Flag Up DownText",
                                                "Flag Up DownHowTo", _playerCountImg[1],
                                                _selectGameImg[41], onStartClick: LevelMiniScene, onExitClick: ExitGameScene);
    }

    public void SelectGuessingnumberorder()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 2;
        Managers.SelGameSet.time3 = 3;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "Guessingnumberorder", "PopupGameFlyerText", "GuessingnumberorderText",
                                                "GuessingnumberorderHowto", _playerCountImg[1],
                                                _selectGameImg[42], onStartClick: LevelMiniScene, onExitClick: ExitGameScene);
    }

    public void SelectPumppadChallenge()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 2;
        Managers.SelGameSet.time3 = 3;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "PumppadChallenge", "PopupGameFlyerText", "PumppadChallengeText",
                                                "PumppadChallengeHowto", _playerCountImg[1],
                                                _selectGameImg[43], onStartClick: LevelMiniScene, onExitClick: ExitGameScene);
    }

    public void SelectSnakeGame()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 2;
        Managers.SelGameSet.time3 = 3;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "SnakeGame", "PopupGameFlyerText", "SnakeGameText",
                                                "SnakeGameHowto", _playerCountImg[1],
                                                _selectGameImg[44], onStartClick: LevelMiniScene, onExitClick: ExitGameScene);
    }

    public void SelectFindingHiddenStrawberries()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 2;
        Managers.SelGameSet.time3 = 3;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "FindingHiddenStrawberries", "PopupGameFlyerText", "FindingHiddenStrawberriesText",
                                                "FindingHiddenStrawberriesHowto", _playerCountImg[1],
                                                _selectGameImg[45], onStartClick: LevelMiniScene, onExitClick: ExitGameScene);
    }
    public void SelectPingpongGame()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 2;
        Managers.SelGameSet.time3 = 3;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "PingpongGame", "PopupGameFlyerText", "PingpongGameText", 
                                                "PingpongGameHowto", _playerCountImg[1],
                                                _selectGameImg[46], onStartClick: LevelMiniScene, onExitClick: ExitGameScene);
    }

    public void SelectDiceGame()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 2;
        Managers.SelGameSet.time3 = 3;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "DiceGame", "PopupGameFlyerText", "PingpongGameText",
                                                "PingpongGameHowto", _playerCountImg[0],
                                                _selectGameImg[46], onStartClick: StartScene, onExitClick: ExitGameScene);
    }
    public void SelectDiceGame2()
    {
        Managers.SelGameSet.time1 = 1;
        Managers.SelGameSet.time2 = 2;
        Managers.SelGameSet.time3 = 3;

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "DiceGame2", "PopupGameFlyerText", "PingpongGameText",
                                                "PingpongGameHowto", _playerCountImg[1],
                                                _selectGameImg[46], onStartClick: StartScene, onExitClick: ExitGameScene);
    }

    public void SelectMovementMode()
    {
        if (popParent.transform.childCount != 0)
        {
            _popParentUI = popParent.GetComponentInChildren<UIPopup>();
            _popParentUI.Hide();
        }

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "Movement", "PopupGameFlyerText", "RhythmGameText", 
                                                "MovementHowTo", _playerCountImg[1],
                                                _selectGameImg[49], onStartClick: RhythmMusicSelectScene, onExitClick: ExitGameScene);

        Managers.UI.Signal.SendSignal("MainFlow", "Movement");
        Managers.SelGameSet.rhythmMode = RhythmGameMode.MovementMode;
        Managers.SelGameSet.playerCount = 2;
    }

    public void SelectAutoMode()
    {
        if (popParent.transform.childCount != 0)
        {
            _popParentUI = popParent.GetComponentInChildren<UIPopup>();
            _popParentUI.Hide();
        }

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "AutoMode", "PopupGameFlyerText", "RhythmGameText",
                                        "AutoModeHowTo", _playerCountImg[1],
                                        _selectGameImg[47], onStartClick: RhythmMusicSelectScene, onExitClick: ExitGameScene);

        Managers.UI.Signal.SendSignal("MainFlow", "AutoMode");
        Managers.SelGameSet.rhythmMode = RhythmGameMode.AutoMode;
        Managers.SelGameSet.rhythmSpeed = RhythmGameSpeed.level01;
        Managers.SelGameSet.playerCount = 2;
    }
    public void SelectFreeMode()
    {
        if (popParent.transform.childCount != 0)
        {            
            popParent.GetComponentInChildren<UIPopup>().Hide();
        }

        Managers.UI.Popup.ShowGameSelectPrefap("UiTextLanguage", "FreeMode", "PopupGameFlyerText", "RhythmGameText",
                                        "FreeModeHowTo", _playerCountImg[3],
                                        _selectGameImg[48], onStartClick: RhythmMusicSelectScene, onExitClick: ExitGameScene);

        Managers.UI.Signal.SendSignal("MainFlow", "FreeMode");
        Managers.SelGameSet.rhythmMode = RhythmGameMode.FreeMode;
    }

    void ExitGameScene(Vector3 pos)
    {
        string gameSelectName = Managers.SelGameSet.gameSelectName;
        string text = Managers.Local.TextString("ExitPopupText", gameSelectName);
        Managers.UI.Popup.ExitPopupPrefab(text, _popupExitBtnImg[0], OnStartClick: ExitBtnAction, OnExitClick: ExitBtn);
    }
    public void ExitBtn(Vector3 pos)
    {
        System.GC.Collect();        
        _popupCanvasBtn.PopupClose();
        Managers.CanvasCol.ExitColOn();
    }

    public void ExitBtnAction(Vector3 pos)
    {
        _colCanvas.SelectColOn();

        _popupCanvasBtn = popParent.transform.GetComponentInChildren<PopupSelectCanvasBtn>();        
        _popupCanvasBtn.ExitSelect();

        Managers.Sound._RhythmMusicSelectSource.clip = null;
        Managers.UI.Signal.SendSignal("MainFlow", Managers.SelGameSet.gameSelectName);
        
        //Managers.SelGameSet.selectGameState = SelectGameState.None;        
        //if (Managers.SelGameSet.rhythmMode == RhythmGameMode.AutoMode |
        //    Managers.SelGameSet.rhythmMode == RhythmGameMode.MovementMode)
        //{
        //    Managers.SelGameSet.rhythmMode = RhythmGameMode.None;
        //}

        Managers.CanvasCol.SelectColOff();
        Managers.CanvasCol.ExerciseColOff();
        Managers.CanvasCol.StretchingColOff();
        Managers.CanvasCol.LeisureColOff();

        Managers.SubCamera.GameSceneOff();
        Managers.CanvasCol.ExitColOn();

        GC.Collect();
        Resources.UnloadUnusedAssets();
        StopAllCoroutines();
    }    

    public void RhythmMusicSelectScene(Vector3 pos)
    {
        System.GC.Collect();

        _popupCanvasBtn = popParent.transform.GetComponentInChildren<PopupSelectCanvasBtn>();
        _colCanvas.SelectColOff();

        Managers.UI.Signal.SendSignal_GameScene("RhythmGame");

        if (Managers.SelGameSet.gameSelectName == "AutoMode")
        {
            Managers.UI.Signal.SendSignal("MainFlow", "AutoMode");
            Managers.UI.Popup.RhythmGameMusicSelectPrefab(OnStartClick: RhythmGameStartSend, OnExitClick: RhythmGameBackSend);
        }
        else if (Managers.SelGameSet.gameSelectName == "Movement")
        {
            Managers.UI.Signal.SendSignal("MainFlow", "Movement");
            Managers.UI.Popup.RhythmGameMusicSelectPrefab(OnStartClick: RhythmGameStartSend, OnExitClick: RhythmGameBackSend);
        }
        else if (Managers.SelGameSet.gameSelectName == "FreeMode")
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
        _popupCanvasBtn.PauseBtnOn();

        if (Managers.SelGameSet.gameSelectName == "Movement")
        {
            mode = Managers.Local.TextString("UiTextLanguage", "Movement");
            Managers.SelGameSet.selectGameState = SelectGameState.Play;
            Managers.UI.Popup.RhythmMusicElementPrefab(mode, Managers.SelGameSet.selectMusic._musicName);
        }
        Managers.SubCamera.GameSceneOn();
        Managers.UI.Signal.SendSignal("RhythmGame", "Movement");
    }

    void RhythmGameStartSend(Vector3 pos)
    {                
        if (Managers.SelGameSet.gameSelectName == "AutoMode")
        {
            _popupCanvasBtn.PauseBtnOn();

            mode = Managers.Local.TextString("UiTextLanguage", "AutoMode");
            Managers.UI.Signal.SendSignal("RhythmGame", "AutoMode");
            Managers.SelGameSet.selectGameState = SelectGameState.Play;
            Managers.UI.Popup.RhythmMusicElementPrefab(mode, Managers.SelGameSet.selectMusic._musicName);
            Managers.CanvasCol.ExitColOn();
            Managers.SubCamera.GameSceneOn();
        }
        else if (Managers.SelGameSet.gameSelectName == "Movement")
        {
            Managers.UI.Signal.SendSignal("RhythmGame", "MusicSelect");
            Managers.SelGameSet.playerCount = 2;
            Managers.UI.Popup.RhythmGameLevelSelectPrefab(OnStartClick: RhythmLevelGameStartSend, OnExitClick: RhythmGameBackSend);
        }
    }
    void RhythmGameBackSend(Vector3 pos)
    {
        _colCanvas.SelectColOn();
        Managers.CanvasCol.ExitColOff();
        if (Managers.SelGameSet.rhythmMode != RhythmGameMode.MovementMode)
        {
            Managers.UI.Signal.SendSignal("RhythmGame", "MusicSelectBack");
        }
    }
    #endregion

    public void PopupSetting()
    {
        Managers.UI.Popup.ShowPrefab("PopupSettingCanvas");
    }

    public void QuitApp()
    {
        string text = Managers.Local.TextString("PopupGameFlyerText", "SystemEnd");
        Managers.UI.Popup.QuitPopupPrefab(text, _popupExitBtnImg[1], OnStartClick: QuitAction);
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
