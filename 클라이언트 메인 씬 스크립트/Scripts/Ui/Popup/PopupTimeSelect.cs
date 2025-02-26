using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PopupTimeSelect : MonoBehaviour
{        
    [SerializeField] GameObject[] _obj;
    [SerializeField] Sprite[] _objImg;

    PopupSelectCanvasBtn _popupSelect;

    void Start()
    {
        _popupSelect = this.transform.parent.parent.GetComponentInChildren<PopupSelectCanvasBtn>();
        this.transform.GetComponent<Canvas>().enabled = true;

        SetButtonImg(0, _objImg[1], false);       
    }

    public void TimeSelect()
    {
        SetButtonImg(0, _objImg[0], true);
    }

    public void TimeDisable()
    {
        SetButtonImg(0, _objImg[1], false);
    }

    public void Hide()
    {
        _popupSelect.ExitSelect();        
    }

    public void SelectHide()
    {
        _popupSelect.PlaySelect();
        _popupSelect.PauseBtnOn();
    }

    private void SetButtonImg(int index, Sprite img , bool enable)
    {
        Image buttonImage = _obj[index].transform.GetComponent<Image>();
        BoxCollider col = _obj[index].transform.GetComponent<BoxCollider>();
        buttonImage.sprite = img;
        col.enabled = enable;
    }
}
