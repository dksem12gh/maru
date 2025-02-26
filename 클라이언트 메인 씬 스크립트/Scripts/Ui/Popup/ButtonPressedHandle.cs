using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPressedHandle : MonoBehaviour
{
    [SerializeField] GameObject[] _btn;

    [SerializeField] Color _disColor;

    Image img;
    public BoxCollider col;

    Image img1;
    public BoxCollider col1;

    Image imgRe;
    public BoxCollider colRe;

    Image imgExit;
    public BoxCollider colExit;

    PopupSelectCanvasBtn _popup;

    private void Start()
    {        
        img = _btn[0].transform.GetComponentInChildren<Image>();
        col = _btn[0].transform.GetComponent<BoxCollider>();
        img1 = _btn[1].transform.GetComponentInChildren<Image>();
        col1 = _btn[1].transform.GetComponent<BoxCollider>();
        imgRe = _btn[2].transform.GetComponentInChildren<Image>();
        colRe = _btn[2].transform.GetComponent<BoxCollider>();
        imgExit = _btn[3].transform.GetComponentInChildren<Image>();
        colExit = _btn[3].transform.GetComponent<BoxCollider>();

        _popup = this.transform.parent.GetComponent<PopupSelectCanvasBtn>();

        _btn[2].GetComponent<TouchEventHandler>().m_PressedEvent.AddListener(val =>
        {
            PressedRetry();
        });
    }

    public void PressedImg()
    {
        StartCoroutine(PressDelay());
    }

    public void PressedStart()
    {
        StartCoroutine(PressStartDelay());
    }

    public void PressedRetry()
    {
        StartCoroutine(PressRetryDelay());
    }

    IEnumerator PressDelay()
    {
        img.color = _disColor;
        col.enabled = false;

        imgExit.color = _disColor;
        colExit.enabled = false;

        _btn[0].SetActive(false);              

        _popup.RestartBtnOff();
        _popup.StartImgOn();
        _popup.StartImgDisable();

        Managers.Sound.SoundPause();
        Managers.SubCamera.PauseSceneOn();
        Managers.SelGameSet.selectGameState = SelectGameState.Pause;

        yield return YieldInstructionCache.WaitForSeconds(3f);

        _popup.StartImgOff();

        img.color = Color.white;
        col.enabled = true;
        imgExit.color = Color.white;
        colExit.enabled = true;
        _btn[0].SetActive(false);
        _btn[1].SetActive(true);
    }

    IEnumerator PressStartDelay()
    {
        img1.color = _disColor;
        col1.enabled = false;

        _btn[1].SetActive(false);        
        _popup.RestartBtnOn();
        _popup.StartImgOn();
        _popup.StartImgDisable();

        Managers.Sound.SoundUnPause();
        Managers.SubCamera.PauseSceneOff();
        Managers.SelGameSet.selectGameState = SelectGameState.UnPause;

        yield return YieldInstructionCache.WaitForSeconds(3f);

        _popup.StartImgOff();        

        img1.color = Color.white;
        col1.enabled = true;
        _btn[1].SetActive(false);
        _btn[0].SetActive(true);
    }

    IEnumerator PressRetryDelay()
    {
        img.color = _disColor;        
        col.enabled = false;
        imgRe.color = _disColor;        
        colRe.enabled = false;
        imgExit.color = _disColor;
        colExit.enabled = false;

        if (Managers.SelGameSet.gameSelectName == "AutoMode" |
            Managers.SelGameSet.gameSelectName == "FreeMode" |
            Managers.SelGameSet.gameSelectName == "Movement")
        {
            Managers.UI.Signal.SendSignal_GameScene("RhythmGame");

            Managers.SubCamera.CameraChange();
            Managers.SubCamera.GameSceneOff();

            Managers.SelGameSet.selectGameState = SelectGameState.Pause;                        
            yield return YieldInstructionCache.WaitForSeconds(1);
        }
        else
        {
            Managers.UI.Signal.SendSignal_GameScene(Managers.SelGameSet.gameSelectName);

            Managers.SubCamera.CameraChange();
            Managers.SubCamera.GameSceneOff();

            Managers.SelGameSet.selectGameState = SelectGameState.Pause;
            yield return YieldInstructionCache.WaitForSeconds(1);
        }        
        
        Managers.CanvasCol.SelectColOff();
        Managers.SubCamera.GameSceneOn();
        Managers.UI.Signal.SendSignal_GameScene(Managers.SelGameSet.gameSelectName);
        yield return YieldInstructionCache.WaitForSeconds(0.2f);
        Managers.UI.Signal.SendSignal("RhythmGame", Managers.SelGameSet.gameSelectName);        
        Managers.SelGameSet.selectGameState = SelectGameState.UnPause;
        Managers.SelGameSet.selectGameState = SelectGameState.Play;
        _popup.PauseBtnOn();

        yield return YieldInstructionCache.WaitForSeconds(3.2f);

        _popup.StartImgOff();

        img.color = Color.white;        
        col.enabled = true;
        imgRe.color = Color.white;        
        colRe.enabled = true;
        imgExit.color = Color.white;
        colExit.enabled = true;
    }

    public void PauseBtnSetOff()
    {
        _btn[0].SetActive(false);
        _btn[1].SetActive(false);
    }
}
