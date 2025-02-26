using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupTimeSelectToggle : MonoBehaviour
{
    [SerializeField] private PopupTimeSelect popupTime;
    [SerializeField] private GameObject[] buttons;

    [SerializeField] Sprite[] _selectImg;

    private int selectedButtonIndex = -1;

    private void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].GetComponent<TouchEventHandler>().m_PressedEvent.AddListener(val =>
            {
                OnClickButton(index);
            });
        }
    }

    private void OnClickButton(int index)
    {
        if (selectedButtonIndex == index)
        {
            SetButtonImg(index, _selectImg[0]);            
            selectedButtonIndex = -1;
            popupTime.TimeDisable();
        }
        else
        {
            if (selectedButtonIndex != -1)
            {
                SetButtonImg(selectedButtonIndex, _selectImg[0]);
            }

            SetButtonImg(index, _selectImg[1]);            

            if (index == 3)
            {
                Managers.GameTime = Managers.SelGameSet.time4;
            }
            else
            {
                string buttonText = buttons[index].transform.GetComponentInChildren<TMP_Text>().text;
                int gameTime;
                if (int.TryParse(buttonText.Replace(Managers.Local.TextString("UiTextLanguage", "Minute"),""), out gameTime))
                {
                    Managers.GameTime = gameTime;
                }
                // 선택지가 한글일 때, 게임 타임(난이도)를 버튼 순서대로(1부터) 할당
                else
                {
                    Managers.GameTime = index + 1;
                }
            }
            selectedButtonIndex = index;
            popupTime.TimeSelect();
        }
    }

    private void SetButtonImg(int index, Sprite img)
    {
        Image buttonImage = buttons[index].transform.GetComponent<Image>();                
        buttonImage.sprite = img;
    }
}