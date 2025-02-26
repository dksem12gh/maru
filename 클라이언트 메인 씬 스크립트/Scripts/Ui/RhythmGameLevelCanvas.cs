using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmGameLevelCanvas : MonoBehaviour
{
    [SerializeField] Sprite[] _btnStartImg;
    [SerializeField] Sprite _btnCloseImg;

    [SerializeField] GameObject[] _obj;

    PopupSelectCanvasBtn _popupSelect;

    void Start()
    {
        _popupSelect = this.transform.parent.parent.GetComponentInChildren<PopupSelectCanvasBtn>();        
        
        this.transform.GetComponent<Canvas>().enabled = true;

        SetButton(0, _btnStartImg[1], false);             
    }


    public void TimeSelect()
    {        
        SetButton(0, _btnStartImg[0], true);                    
    }

    public void TimeDisable()
    {
        SetButton(0, _btnStartImg[1], false);        
    }

    public void Hide()
    {        
        Managers.PopupCol.popupColliderList.Remove(this.gameObject);
        Managers.CanvasCtrl.RhythmMusicSelectScene(Vector3.zero);        
    }

    public void NextBtnHide()
    {
        Managers.PopupCol.popupColliderList.Remove(this.gameObject);
        Managers.SelGameSet.selectGameState = SelectGameState.Play;
        _popupSelect.PlaySelect();
    }

    public void ExitBtn()
    {
        _popupSelect.ExitSelect();
        Managers.CanvasCol.SelectColOn();
        Managers.UI.Signal.SendSignal("RhythmGame", "Exit");
        Managers.PopupCol.popupColliderList.Remove(this.gameObject);
    }

    private void SetButton(int index, Sprite img, bool boxEnalbe)
    {
        Image buttonImage = _obj[index].transform.GetChild(0).GetComponent<Image>();
        BoxCollider buttonCol = _obj[index].transform.GetComponent<BoxCollider>();
        buttonImage.sprite = img;
        buttonCol.enabled = boxEnalbe;
    }
}
