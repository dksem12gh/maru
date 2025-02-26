using Doozy.Runtime.UIManager.Containers;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.Video;

public class PopupContoroller
{   
    //main scene 게임선택시 게임 정보 불러오기
    public void ShowGameSelectPrefap(string gameSceneNameTable,string gameSceneNameKey,//각 게임씬별 localize string
                                        string gameSceneFlyerTable, string gameSceneFlyerKey,//각 게임씬별 flyertext localize string / 게임 인원
                                        string gameSceneHowtoKey, Sprite playerCountImg,                                        
                                        Sprite gameImage = null , //각 게임씬소개 이미지,
                                        VideoClip gameMovie = null,
                                        UnityAction<Vector3> onStartClick = null , UnityAction<Vector3> onExitClick = null) //각 게임소개 프리팹 버튼 이벤트
    {

        Managers.CanvasCol.SelectColOff();
        Managers.CanvasCol.ExerciseColOff();
        Managers.CanvasCol.StretchingColOff();
        Managers.CanvasCol.LeisureColOff();
        //Managers.CanvasCol.PageBtnOff();

        Managers.SelGameSet.gameSelectName = gameSceneNameKey;
        Managers.SelGameSet.selectGameState = SelectGameState.None;
        Managers.Instance._poup = true;

        var sceneName = Managers.Local.TextString(gameSceneNameTable, gameSceneNameKey);
        var howtoText = Managers.Local.TextString(gameSceneFlyerTable, gameSceneHowtoKey);
        var flyerText = Managers.Local.TextString(gameSceneFlyerTable, gameSceneFlyerKey);       

        var popup = UIPopup.Get("PopupGameCanvas");
        popup.SetTexts(sceneName, howtoText, flyerText);
        popup.SetSprites(gameImage,playerCountImg);

        var movie = popup.GetComponentInChildren<VideoPlayer>();
        if (gameMovie != null)
        {            
            movie.clip = gameMovie;
        }
        else
        {
            movie.gameObject.SetActive(false);
        }

        //저장한 프리팹의 특정 컴포넌트를 가져오는데 마법의숫자를 최대한 덜쓰는 방향은 이거뿐인거같다...
        var touchEvents = popup.GetComponentsInChildren<TouchEventHandler>();
        touchEvents[0].m_PressedEvent.AddListener(onStartClick);
        touchEvents[1].m_PressedEvent.AddListener(onExitClick) ;
        touchEvents[2].m_PressedEvent.AddListener(BackBtn);

        popup.Show();

        Managers.PopupCol.popupColliderList.Add(popup.gameObject);
    }

    public void ShowRhythmGameSelectPrefap(string gameSceneNameTable, string gameSceneNameKey,//각 게임씬별 localize string
                                    string gameSceneFlyerTable, string gameSceneFlyerKey,//각 게임씬별 flyertext localize string / 게임 인원
                                    string gameSceneHowtoKey, Sprite playerCountImg,                                    
                                    Sprite gameImage = null, //각 게임씬소개 이미지
                                    UnityAction<Vector3> onStartClick = null, UnityAction<Vector3> onExitClick = null) //각 게임소개 프리팹 버튼 이벤트
    {
        /*Managers.CanvasCol.SelectColOff();
        Managers.CanvasCol.ExerciseColOff();
        Managers.CanvasCol.StretchingColOff();
        Managers.CanvasCol.LeisureColOff();*/

        Managers.SelGameSet.gameSelectName = gameSceneNameKey;
        Managers.SelGameSet.selectGameState = SelectGameState.None;
        Managers.Instance._poup = true;

        var sceneName = Managers.Local.TextString(gameSceneNameTable, gameSceneNameKey);
        var howtoText = Managers.Local.TextString(gameSceneFlyerTable, gameSceneHowtoKey);
        var flyerText = Managers.Local.TextString(gameSceneFlyerTable, gameSceneFlyerKey);        

        var popup = UIPopup.Get("PopupRhythmGameCanvas");        
        popup.SetTexts(sceneName, howtoText, flyerText);
        popup.SetSprites(gameImage, playerCountImg);

        //저장한 프리팹의 특정 컴포넌트를 가져오는데 마법의숫자를 최대한 덜쓰는 방향은 이거뿐인거같다...
        var touchEvents = popup.GetComponentsInChildren<TouchEventHandler>();
        touchEvents[0].m_PressedEvent.AddListener(onStartClick);
        touchEvents[1].m_PressedEvent.AddListener(onExitClick);
        touchEvents[2].m_PressedEvent.AddListener(BackBtn);        

        popup.Show();

        Managers.PopupCol.popupColliderList.Add(popup.gameObject);
    }

    public void ShowGameTimeSelectPrefap(int time1,int time2,int time3,int time4,string title = null,string element = null,
                                            UnityAction<Vector3> OnStartClick = null, UnityAction<Vector3> OnExitClick = null)
    {        
        var popup = UIPopup.Get("TimeSelectCanvas");        
        var unlimited = Managers.Local.TextString("UiTextLanguage", "Unlimited");
        var count = Managers.Local.TextString("UiTextLanguage", title);
        popup.SetTexts(count,time1.ToString(), time2.ToString(), time3.ToString(), unlimited);

        Managers.CanvasCol.ExitColOff();

        var touchEvents = popup.GetComponentsInChildren<TouchEventHandler>();
        //0~3 시간선택 4게임시작버튼
        touchEvents[4].m_PressedEvent.AddListener(OnStartClick);
        touchEvents[5].GetComponent<TouchEventHandler>().m_PressedEvent.AddListener(val =>
        {
            Managers.CanvasCol.ExitColOn();
        });

        popup.Show();

        Managers.PopupCol.popupColliderList.Add(popup.gameObject);        
    }

    public void ShowGameSeconSelectPrefap(int time1, int time2, int time3, int time4, string title = null, string element = null,
                                            UnityAction<Vector3> OnStartClick = null, UnityAction<Vector3> OnExitClick = null)
    {
        var popup = UIPopup.Get("TimeSelectCanvas");

        var count = Managers.Local.TextString("UiTextLanguage", title);
        popup.SetTexts(count, time1.ToString(), time2.ToString(), time3.ToString(), time4.ToString());

        var touchEvents = popup.GetComponentsInChildren<TouchEventHandler>();
        //0~3 시간선택 4게임시작버튼
        touchEvents[4].m_PressedEvent.AddListener(OnStartClick);
        touchEvents[5].GetComponent<TouchEventHandler>().m_PressedEvent.AddListener(val =>
        {
            Managers.CanvasCol.ExitColOn();
        });
        popup.Show();
        Managers.CanvasCol.ExitColOff();
        Managers.PopupCol.popupColliderList.Add(popup.gameObject);
    }

    public void ShowGameMiniSelectPrefap(int time1, int time2, int time3, string title = null, string element = null,
                                        UnityAction<Vector3> OnStartClick = null, UnityAction<Vector3> OnExitClick = null)
    {
        var popup = UIPopup.Get("MiniSelectCanvas");

        var count = Managers.Local.TextString("UiTextLanguage", title);
        popup.SetTexts(count, time1.ToString(), time2.ToString(), time3.ToString());

        var touchEvents = popup.GetComponentsInChildren<TouchEventHandler>();
        //0~3 시간선택 4게임시작버튼
        touchEvents[3].m_PressedEvent.AddListener(OnStartClick);
        touchEvents[4].GetComponent<TouchEventHandler>().m_PressedEvent.AddListener(val =>
        {
            Managers.CanvasCol.ExitColOn();
        });

        popup.Show();
        Managers.CanvasCol.ExitColOff();
        Managers.PopupCol.popupColliderList.Add(popup.gameObject);
    }

    public void ShowGameLanguageSelectPrefap(int time1, int time2, int time3, string title = null, string element = null,
                                    UnityAction<Vector3> OnStartClick = null, UnityAction<Vector3> OnExitClick = null)
    {
        var popup = UIPopup.Get("MiniSelectCanvas");

        var count = Managers.Local.TextString("UiTextLanguage", title);
        //popup.SetTexts(count, time1.ToString(), time2.ToString(), time3.ToString());
        popup.SetTexts(count, "숫자", "영어", "한글");


        var touchEvents = popup.GetComponentsInChildren<TouchEventHandler>();
        //0~3 시간선택 4게임시작버튼
        touchEvents[3].m_PressedEvent.AddListener(OnStartClick);
        touchEvents[4].GetComponent<TouchEventHandler>().m_PressedEvent.AddListener(val =>
        {
            Managers.CanvasCol.ExitColOn();
        });

        popup.Show();
        Managers.CanvasCol.ExitColOff();
        Managers.PopupCol.popupColliderList.Add(popup.gameObject);
    }

    public void ExitPopupPrefab(string popupText, Sprite btnImg, UnityAction<Vector3> OnStartClick = null , UnityAction<Vector3> OnExitClick = null)
    {
        Managers.CanvasCol.ExitColOff();

        var popup = UIPopup.Get("ExitPopup");
        popup.SetTexts(popupText);
        popup.SetSprites(btnImg);

        var touchEvents = popup.GetComponentsInChildren<TouchEventHandler>();        
        touchEvents[0].m_PressedEvent.AddListener(OnStartClick);
        touchEvents[1].m_PressedEvent.AddListener(OnExitClick);
      
        touchEvents[1].GetComponent<TouchEventHandler>().m_PressedEvent.AddListener(val =>
        {
            ButtonPressedHandle temp = popup.transform.parent.GetComponentInChildren<ButtonPressedHandle>();
            if (!temp) return;
            temp.PauseBtnSetOff();
            Managers.CanvasCol.ExitColOn();
        });

        popup.Show();
        Managers.CanvasCol.ExitColOff();
        Managers.PopupCol.popupColliderList.Add(popup.gameObject);
    }

    public void QuitPopupPrefab(string popupText, Sprite btnImg, UnityAction<Vector3> OnStartClick = null , UnityAction<Vector3> OnExitClick = null)
    {
        Managers.CanvasCol.SelectColOff();
        Managers.CanvasCol.ExerciseColOff();
        Managers.CanvasCol.StretchingColOff();
        Managers.CanvasCol.LeisureColOff();        

        var popup = UIPopup.Get("QuitPopup");
        popup.SetTexts(popupText);
        popup.SetSprites(btnImg);

        var touchEvents = popup.GetComponentsInChildren<TouchEventHandler>();
        touchEvents[0].m_PressedEvent.AddListener(OnStartClick);        
        touchEvents[1].m_PressedEvent.AddListener(QuitBack);

        touchEvents[1].m_PressedEvent.AddListener(val =>
        {
            PopupSelectCanvasBtn temp = popup.transform.parent.parent.GetComponentInChildren<PopupSelectCanvasBtn>();            
            if (!temp) return;

            if(Managers.SelGameSet.selectGameState == SelectGameState.None)
            {
                temp.NoneExitBtnSet();
            }
            else if (Managers.SelGameSet.selectGameState == SelectGameState.Play)
            {
                temp.PlayExitBtnSet();
            }
            else if (Managers.SelGameSet.selectGameState == SelectGameState.Pause)
            {
                temp.PauseExitBtnSet();
            }

        });


        popup.Show();

        Managers.PopupCol.popupColliderList.Add(popup.gameObject);
    }

    public void RhythmMusicElementPrefab(string modeName,string selectMusicName = null)
    {
        var popup = UIPopup.Get("MusicElement");
        popup.SetTexts(modeName, selectMusicName);
        popup.Show();
    }

    public void RhythmModeSelectPrefab(UnityAction<Vector3> OnStartClick = null, UnityAction<Vector3> OnExitClick = null)
    {
        var popup = UIPopup.Get("RhythmModeSelectCanvas");

        var touchEvents = popup.GetComponentsInChildren<TouchEventHandler>();

        touchEvents[3].m_PressedEvent.AddListener(OnStartClick);
        touchEvents[4].m_PressedEvent.AddListener(OnExitClick);

        popup.Show();

        Managers.PopupCol.popupColliderList.Add(popup.gameObject);
    }

    public void RhythmGameMusicSelectPrefab(UnityAction<Vector3> OnStartClick = null, UnityAction<Vector3> OnExitClick = null)
    {
        var popup = UIPopup.Get("RhythmMusicSelectCanvas");

        var popupEvent = popup.GetComponentsInChildren<TouchEventHandler>();

        popupEvent[0].m_PressedEvent.AddListener(OnStartClick);
        popupEvent[1].m_PressedEvent.AddListener(OnExitClick);

        popup.Show();
        Managers.CanvasCol.ExitColOff();
        Managers.PopupCol.popupColliderList.Add(popup.gameObject);
    }

    GameObject exitPop;

    public void RhythmGameLevelSelectPrefab(UnityAction<Vector3> OnStartClick = null, UnityAction<Vector3> OnExitClick = null)
    {       
        var popup = UIPopup.Get("RhythmGameLevelCanvas");

        var popupEvent = popup.GetComponentsInChildren<TouchEventHandler>();

        popupEvent[0].m_PressedEvent.AddListener(OnStartClick);
        popupEvent[1].m_PressedEvent.AddListener(OnExitClick);

        popup.Show();

        exitPop = popup.gameObject;
        Managers.CanvasCol.ExitColOff();
        Managers.PopupCol.popupColliderList.Add(popup.gameObject);
    }

    public void QuitBack(Vector3 pos)
    {
        Managers.CanvasCol.ExitColOn();

        Managers.PopupCol.popupColliderList.Remove(Managers.PopupCol.popupColliderList.GetLastItem());

        if (Managers.Instance._poup)
        {
            Managers.CanvasCol.SelectColOff();
            Managers.CanvasCol.ExerciseColOff();
            Managers.CanvasCol.StretchingColOff();
            Managers.CanvasCol.LeisureColOff();
        }
        else
        {
            Managers.CanvasCol.SelectColOn();

            if (Managers.SelGameSet.gameSelectName == "AutoMode" |
                Managers.SelGameSet.gameSelectName == "Movement" |
                Managers.SelGameSet.gameSelectName == "FreeMode")
            {
                Managers.UI.Signal.SendSignal("RhythmGame", "MusicSelectBack");
            }

            switch (Managers.CanvasCol._name)
            {
                case "TrainingBtn":
                    Managers.CanvasCol.ExerciseColOn();
                    Managers.CanvasCol.StretchingColOff();
                    Managers.CanvasCol.LeisureColOff();
                    break;
                case "StretchingBtn":
                    Managers.CanvasCol.ExerciseColOff();
                    Managers.CanvasCol.StretchingColOn();
                    Managers.CanvasCol.LeisureColOff();
                    break;
                case "MiniGameBtn":
                    Managers.CanvasCol.ExerciseColOff();
                    Managers.CanvasCol.StretchingColOff();
                    Managers.CanvasCol.LeisureColOn();
                    break;
            }
        }
    }

    public void BackBtn(Vector3 pos)
    {
        Managers.CanvasCol._exercise.SetActive(true);
        Managers.CanvasCol._stretching.SetActive(true);
        Managers.CanvasCol._leisure.SetActive(true);

        Managers.CanvasCol.SelectColOn();
        Managers.Instance._poup = false;

        if (Managers.SelGameSet.gameSelectName == "AutoMode" |
            Managers.SelGameSet.gameSelectName == "Movement"|
            Managers.SelGameSet.gameSelectName == "FreeMode")
        {
            Managers.UI.Signal.SendSignal("RhythmGame", "MusicSelectBack");
        }

        switch(Managers.CanvasCol._name)
        {
            case "TrainingBtn":
                Managers.CanvasCol.ExerciseColOn();
                Managers.CanvasCol.StretchingColOff();
                Managers.CanvasCol.LeisureColOff();
                break;
            case "StretchingBtn":
                Managers.CanvasCol.ExerciseColOff();
                Managers.CanvasCol.StretchingColOn();
                Managers.CanvasCol.LeisureColOff();
                break;
            case "MiniGameBtn":
                Managers.CanvasCol.ExerciseColOff();
                Managers.CanvasCol.StretchingColOff();
                Managers.CanvasCol.LeisureColOn();
                break;
        }
    }

    public void ShowPrefab(string name)
    {
        Managers.CanvasCol.SelectColOff();
        Managers.CanvasCol.ExerciseColOff();
        Managers.CanvasCol.StretchingColOff();
        Managers.CanvasCol.LeisureColOff();

        var popup = UIPopup.Get(name);
        popup.Show();


    }

    public void HidePrefab(string popupTagName)
    {
        var popup = UIPopup.Get(popupTagName);
        popup.GetComponent<UIPopup>().Hide();        
    }

    //public void RhytmeGame



    /*
     * ... 필요시 더 추가
     */
}
