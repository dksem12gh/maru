using Doozy.Runtime.UIManager.Containers;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PopupSelectCanvasBtn : MonoBehaviour
{
    public GameObject[] _obj;
    [SerializeField] Color _nomalColor;
    [SerializeField] Color _disableColor;    
    [SerializeField] GameObject _subObj;
    [SerializeField] Image _playerImg;

    private void Start()
    {
        _playerImg.SetNativeSize();

        SetButton(0, _nomalColor, true);
        SetButton(1, _disableColor, false);
        SetButton(2, _nomalColor, true);
        SetButton(3, _disableColor, false);
    }

    public void TimeSelect()
    {
        SetButton(0, _nomalColor, true);
        SetButton(1, _disableColor, false);
        SetButton(2, _nomalColor, true);
        SetButton(3, _disableColor, false);
    }

    public void PlaySelect()
    {
        SetButton(0, _disableColor, false);
        SetButton(1, _nomalColor, true);
        SetButton(2, _disableColor, false);

        if (Managers.SelGameSet.rhythmMode == RhythmGameMode.FreeMode)
        {
            SetButton(3, _disableColor, false);
        }
        else
        {
            SetButton(3, _nomalColor, true);
        }
    }

    public void PopupClose()
    {
        ButtonPressedHandle handle = transform.GetComponentInChildren<ButtonPressedHandle>();

        if (Managers.SelGameSet.selectGameState == SelectGameState.Pause)
        {
            SetButton(0, _disableColor, false);
            SetButton(1, _nomalColor, true);
            SetButton(3, _disableColor, false);
            handle.col1.enabled = true;
        }
        else
        {
            handle.col.enabled = true;
            SetButton(0, _disableColor, false);
            SetButton(1, _nomalColor, true);

            if (Managers.SelGameSet.rhythmMode == RhythmGameMode.FreeMode)
            {
                SetButton(3, _disableColor, false);
            }
            else
            {
                SetButton(3, _nomalColor, true);
            }
        }
    }

    public void ExitSelect()
    {
        Managers.GameTime = 0;
        Time.timeScale = 1;

        PauseBtnOff();
        StartImgOn();
        SetButton(0, _nomalColor, true);
        SetButton(1, _disableColor, false);
        SetButton(2, _nomalColor, true);
        SetButton(3, _disableColor, false);
        _obj[6].SetActive(false);        

        if (!_subObj.GetComponentInChildren<UIPopup>()) return;

        _subObj.GetComponentInChildren<UIPopup>().Hide();
    }
    
    public void BackSelect()
    {
        Time.timeScale = 1;        
        Managers.PopupCol.popupColliderList.Clear();
    }

    public void PauseBtnOn()
    {        
        _obj[4].SetActive(true);        
        StartCoroutine("Del");        
    }    

    public void btnStop()
    {
        StopAllCoroutines();
    }

    IEnumerator Del()
    {
        SetButton(3, _disableColor, false);
        SetButton(1, _disableColor, false);
        yield return YieldInstructionCache.WaitForSeconds(4.5f);
        SetButton(3, _nomalColor, true);
        SetButton(1, _nomalColor, true);
        _obj[0].SetActive(false);
        StartImgOff();
        _obj[4].transform.GetChild(0).gameObject.SetActive(true);
    }

    public void PauseBtnOff()
    {
        _obj[4].transform.GetChild(0).gameObject.SetActive(false);
    }

    public void RestartBtnOn()
    {
        SetButton(3, _nomalColor, true);
    }
    public void RestartBtnOff()
    {
        SetButton(3, _disableColor, false);
    }

    public void StartImgEnable()
    {        
        SetButton(0, _nomalColor, false);
    }

    public void StartImgDisable()
    {
        SetButton(0, _disableColor, false);
    }

    public void StartImgOn()
    {
        _obj[0].SetActive(true);
    }
    public void StartImgOff()
    {
        _obj[0].SetActive(false);
    }

    public void ExitBtnDisable()
    {
        SetButton(2, _disableColor, false);
    }

    IEnumerator Re()
    {        
        var ui = _subObj.GetComponentInChildren<UIPopup>();
        if (ui != null)
        {
            ui.Hide();
        }
  
        yield return YieldInstructionCache.WaitForSeconds(0.5f);

        if (Managers.SelGameSet.gameSelectName == "AutoMode")
        {
            Managers.UI.Popup.RhythmMusicElementPrefab(Managers.Local.TextString("UiTextLanguage", "AutoMode"), Managers.SelGameSet.selectMusic._musicName);
        }
        else if(Managers.SelGameSet.gameSelectName == "Movement")
        {
            Managers.UI.Popup.RhythmMusicElementPrefab(Managers.Local.TextString("UiTextLanguage", "Movement"), Managers.SelGameSet.selectMusic._musicName);
        }
    }

    public void ReStart()
    {
        StopAllCoroutines();
        StartCoroutine(Re());
    }

    private void SetButton(int index, Color cor, bool enable)
    {
        Image[] buttonImage = _obj[index].transform.GetComponentsInChildren<Image>();        
        BoxCollider col = _obj[index].transform.GetComponent<BoxCollider>();
        buttonImage[0].color = cor;        
        col.enabled = enable;
    }
    private void SetColButton(int index, bool enable)
    {        
        BoxCollider col = _obj[index].transform.GetComponent<BoxCollider>();        
        col.enabled = enable;
    }

    public void NoneExitBtnSet()
    {        
        SetButton(1, _disableColor, false);        
        SetButton(3, _disableColor, false);
    }

    public void PlayExitBtnSet()
    {
        SetButton(2, _disableColor, false);
    }

    public void PauseExitBtnSet()
    {
        SetColButton(2, false);
        SetColButton(3, false);
    }
}
