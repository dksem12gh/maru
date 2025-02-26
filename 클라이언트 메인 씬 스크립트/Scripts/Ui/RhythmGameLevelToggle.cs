using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmGameLevelToggle : MonoBehaviour
{
    [SerializeField] RhythmGameLevelCanvas levelCanvas;
    [SerializeField] GameObject[] _btn;

    [SerializeField] Sprite[] _btn01Sprite;
    [SerializeField] Sprite[] _btn02Sprite;    

    public int selectBtn = -1;
    public bool select = false;

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
                SetButton(index, _btn01Sprite[0]);
            }
            else
            {
                SetButton(index, _btn02Sprite[0]);
            }

            selectBtn = -1;
            select = false;
            levelCanvas.TimeDisable();
        }
        else
        {
            if (selectBtn != -1)
            {
                if (selectBtn == 0)
                {
                    SetButton(selectBtn, _btn01Sprite[0]);
                }
                else
                {
                    SetButton(selectBtn, _btn02Sprite[0]);
                }

                select = false;                
                levelCanvas.TimeDisable();
            }            
            
            select = true;
            levelCanvas.TimeSelect();
            selectBtn = index;
            Managers.SelGameSet.playerCount = selectBtn + 1;

            if (index == 0)
            {
                SetButton(index, _btn01Sprite[1]);
            }
            else
            {
                SetButton(index, _btn02Sprite[1]);
            }
        }
    }

    private void SetButton(int index, Sprite img)
    {
        Image buttonImage = _btn[index].transform.GetComponent<Image>();        
        buttonImage.sprite = img;        
    }
}
