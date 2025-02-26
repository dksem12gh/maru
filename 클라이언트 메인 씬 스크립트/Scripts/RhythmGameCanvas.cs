using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RhythmGameCanvas : MonoBehaviour
{
    [SerializeField] Sprite[] _btnImg;
    [SerializeField] GameObject[] _obj;

    PopupSelectCanvasBtn _popupSelect;
    PopupRhythmModeSelect _rhythmSelect;

    GameObject _prevObj;
    void Start()
    {
        _popupSelect = this.transform.parent.parent.GetComponentInChildren<PopupSelectCanvasBtn>();
        _rhythmSelect = this.transform.GetComponentInChildren<PopupRhythmModeSelect>();
        _prevObj = this.transform.gameObject;

        this.transform.GetComponent<Canvas>().enabled = true;

        SetButtonImg(0, _btnImg[1], false);        
    }


    public void TimeSelect()
    {
        SetButtonImg(0, _btnImg[0], true);
    }

    public void TimeDisable()
    {
        SetButtonImg(0, _btnImg[1], false);
    }

    public void Hide()
    {
        _popupSelect.ExitSelect();
        Managers.PopupCol.popupColliderList.Remove(_prevObj);
    }

    public void NextBtnHide()
    {        
        Managers.PopupCol.popupColliderList.Remove(_prevObj);

        if (_rhythmSelect.selectBtn == 0)
        {
            _popupSelect.PauseBtnOn();
            Managers.UI.Signal.SendSignal("RhythmGame", "AutoMode");
            Managers.SelGameSet.rhythmMode = RhythmGameMode.AutoMode;
            Managers.SelGameSet.rhythmSpeed = RhythmGameSpeed.level01;
            Managers.SelGameSet.playerCount = 1;
            Managers.SelGameSet.selectGameState = SelectGameState.Play;
        }
        else if(_rhythmSelect.selectBtn == 1)
        {
            _popupSelect.PauseBtnOn();
            Managers.UI.Signal.SendSignal("RhythmGame", "MovementMode");
            Managers.SelGameSet.rhythmMode = RhythmGameMode.MovementMode;
        }
        else if (_rhythmSelect.selectBtn == 2)
        {
            _popupSelect.PlaySelect();
            _popupSelect.PauseBtnOn();
            Managers.UI.Signal.SendSignal("RhythmGame", "FreeMode");
            Managers.SelGameSet.rhythmMode = RhythmGameMode.FreeMode;
            Managers.SelGameSet.selectGameState = SelectGameState.Play;            
            Managers.UI.Popup.RhythmMusicElementPrefab(Managers.Local.TextString("UiTextLanguage", "FreeMode"),
                                                        Managers.SelGameSet.selectMusic._musicName);           
        }
    }

    private void SetButtonImg(int index, Sprite img, bool enable)
    {
        Image buttonImage = _obj[index].transform.GetComponentInChildren<Image>();
        BoxCollider col = _obj[index].transform.GetComponent<BoxCollider>();
        buttonImage.sprite = img;
        col.enabled = enable;
    }
}
