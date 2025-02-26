using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmLevelToggle : MonoBehaviour
{
    TouchEventHandler[] _btn;
    public int selectBtn = -1;

    void Start()
    {
        _btn = this.transform.GetComponentsInChildren<TouchEventHandler>();

        for (int i = 0; i < _btn.Length; i++)
        {
            int index = i;

            _btn[i].m_PressedEvent.AddListener(val =>
            {
                OnClickBtn(index);
            });
        }
    }

    public void OnClickBtn(int index)
    {
        if (selectBtn == index)
        {
            Color tempColor = _btn[index].GetComponent<Image>().color;
            tempColor = Color.white;
            _btn[index].GetComponent<Image>().color = tempColor;
            selectBtn = -1;
            //popupTime.TimeDisable();
        }
        else
        {
            if (selectBtn != -1)
            {
                Color tempColor = _btn[selectBtn].GetComponent<Image>().color;
                tempColor = Color.white;
                _btn[selectBtn].GetComponent<Image>().color = tempColor;
                //popupTime.TimeDisable();
            }

            _btn[index].GetComponent<Image>().color = Color.green;
            //popupTime.TimeSelect();
            selectBtn = index;
        }
    }
}
