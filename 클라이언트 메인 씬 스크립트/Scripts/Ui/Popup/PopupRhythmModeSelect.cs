using Doozy.Runtime.UIManager.Containers;
using UnityEngine;
using UnityEngine.UI;

public class PopupRhythmModeSelect : MonoBehaviour
{
    [SerializeField] RhythmGameCanvas popupTime;
    [SerializeField] GameObject[] _btn;
    [SerializeField] GameObject[] _nextBtn;

    [SerializeField] Sprite[] _autoImg;
    [SerializeField] Sprite[] _moveImg;
    [SerializeField] Sprite[] _freeImg;

    public int selectBtn = -1;

    void Start()
    {
        for (int i = 0; i < _btn.Length; i++)
        {
            int index = i;

            _btn[i].GetComponent<TouchEventHandler>().m_PressedEvent.AddListener(val =>
            {
                OnClickBtn(index);
            });
        }
    }

    public void OnClickBtn(int index)
    {
        Sprite img;
        if (selectBtn == index)
        {            
            if(index == 0)
            {
                img = _autoImg[0];
                SetButtonImg(index, img);
            }
            else if(index == 1)
            {
                img = _moveImg[0];
                SetButtonImg(index, img);
            }
            else if(index == 2)
            {
                img = _freeImg[0];
                SetButtonImg(index, img);
            }

            selectBtn = -1;
            popupTime.TimeDisable();
            _nextBtn[0].SetActive(true);
            _nextBtn[1].SetActive(false);
        }
        else
        {
            if (selectBtn != -1)
            {
                if (selectBtn == 0)
                {
                    img = _autoImg[0];
                    SetButtonImg(selectBtn, img);
                }
                else if (selectBtn == 1)
                {
                    img = _moveImg[0];
                    SetButtonImg(selectBtn, img);
                }
                else if (selectBtn == 2)
                {
                    img = _freeImg[0];
                    SetButtonImg(selectBtn, img);
                }

                popupTime.TimeDisable();
            }

            if (index == 0)
            {
                img = _autoImg[1];
                SetButtonImg(index, img);
            }
            else if (index == 1)
            {
                img = _moveImg[1];
                SetButtonImg(index, img);
            }
            else if (index == 2)
            {
                img = _freeImg[1];
                SetButtonImg(index, img);
            }
            popupTime.TimeSelect();
            selectBtn = index;

            if (selectBtn == 2)
            {
                _nextBtn[0].SetActive(false);
                _nextBtn[1].SetActive(true);
            }
            else
            {
                _nextBtn[0].SetActive(true);
                _nextBtn[1].SetActive(false);
            }
        }
    }

    private void SetButtonImg(int index, Sprite img)
    {
        Image buttonImage = _btn[index].transform.GetComponent<Image>();
        buttonImage.sprite = img;
    }
}
