using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmGameLevelToggleD : MonoBehaviour
{
    [SerializeField] RhythmGameLevelCanvas levelCanvas;
    [SerializeField] GameObject[] _btn;

    [SerializeField] Sprite[] _btn01Img;
    [SerializeField] Sprite[] _btn02Img;
    [SerializeField] Sprite[] _btn03Img;

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
        if (selectBtn == index)
        {
            if (index == 0)
            {
                SetButton(index, _btn01Img[0]);
            }
            else if(index == 1)
            {
                SetButton(index, _btn02Img[0]);
            }
            else if(index == 2)
            {
                SetButton(index, _btn03Img[0]);
            }
            
            selectBtn = -1;
            levelCanvas.TimeDisable();
        }
        else
        {
            if (selectBtn != -1)
            {
                if (selectBtn == 0)
                {
                    SetButton(selectBtn, _btn01Img[0]);
                }
                else if (selectBtn == 1)
                {
                    SetButton(selectBtn, _btn02Img[0]);
                }
                else if (selectBtn == 2)
                {
                    SetButton(selectBtn, _btn03Img[0]);
                }
                
                levelCanvas.TimeDisable();
            }
           
            levelCanvas.TimeSelect();
            selectBtn = index;

            switch(index)
            {
                case 0:
                    SetButton(index, _btn01Img[1]);
                    Managers.SelGameSet.rhythmSpeed = RhythmGameSpeed.level01;
                    break;
                case 1:
                    SetButton(index, _btn02Img[1]);
                    Managers.SelGameSet.rhythmSpeed = RhythmGameSpeed.level02;
                    break;
                case 2:
                    SetButton(index, _btn03Img[1]);
                    Managers.SelGameSet.rhythmSpeed = RhythmGameSpeed.level03;
                    break;
            }            
        }
    }
    private void SetButton(int index, Sprite img)
    {
        Image buttonImage = _btn[index].transform.GetComponent<Image>();
        buttonImage.sprite = img;
    }
}
