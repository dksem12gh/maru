using UnityEngine;
using UnityEngine.UI;

public class RhythmMusicSelectCanvas : MonoBehaviour
{
    [SerializeField] Sprite _nomalPlaySprite;
    [SerializeField] Sprite _nomalExitSprite;
    [SerializeField] Sprite _disableSprite;
    [SerializeField] GameObject[] _obj;
    PopupSelectCanvasBtn _popupSelect;

    void Start()
    {
        _popupSelect = this.transform.parent.parent.GetComponentInChildren<PopupSelectCanvasBtn>();
        this.transform.GetComponent<Canvas>().enabled = true;

        SetButton(0, _disableSprite, false);

    }

    public void MusicSelect()
    {
        SetButton(0, _nomalPlaySprite, true);
    }

    public void MusicDisable()
    {
        SetButton(0, _disableSprite, false);
    }

    public void Exit()
    {
        _popupSelect.ExitSelect();
        Managers.Sound.MusicSoundMute();
        Managers.UI.Signal.SendSignal("RhythmGame", "MusicSelectBack");
        Managers.PopupCol.popupColliderList.Remove(this.gameObject);
    }

    public void Hide()
    {
        Managers.PopupCol.popupColliderList.Remove(this.gameObject);
        Managers.Sound.MusicSoundMute();
        _popupSelect.TimeSelect();
    }

    public void SelectHide()
    {
        Managers.PopupCol.popupColliderList.Remove(this.gameObject);        
        Managers.Sound.MusicSoundMute();
        _popupSelect.PlaySelect();
    }

    private void SetButton(int index, Sprite img, bool boxEnalbe)
    {
        Image buttonImage = _obj[index].transform.GetChild(0).GetComponent<Image>();
        BoxCollider buttonCol = _obj[index].transform.GetComponent<BoxCollider>();
        buttonImage.sprite = img;
        buttonCol.enabled = boxEnalbe;
    }
}
