using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGamePageBtn : MonoBehaviour
{
    [SerializeField] GameObject[] btn;
    [SerializeField] GameObject[] pageObj;
    [SerializeField] Sprite[] btnImg;

    int count;

    private void Start()
    {
        count = 1;
        SetButton(btn[0], btnImg[0], false);
        SetButton(btn[1], btnImg[1], true);
        /*pageObj[0].SetActive(true);
        pageObj[1].SetActive(false);*/
    }

    public void LeftBtn()
    {
        count--;
        if (count <= 1) count = 1;

        switch (count)
        {
            case 1:
                SetButton(btn[0], btnImg[0], false);
                SetButton(btn[1], btnImg[1], true);                
                pageObj[0].SetActive(true);
                pageObj[1].SetActive(false);
                if (pageObj.Length == 3)
                    pageObj[2].SetActive(false);
                break;
            case 2:
                SetButton(btn[0], btnImg[1], true, -1);
                SetButton(btn[1], btnImg[0], false, -1);
                pageObj[0].SetActive(false);
                pageObj[1].SetActive(true);
                if (pageObj.Length == 3)
                {
                    SetButton(btn[1], btnImg[1], true, 1);
                    pageObj[2].SetActive(false);
                }
                break;            
        }
    }

    public void RightBtn()
    {
        count++;
        if (count >= pageObj.Length) count = pageObj.Length;

        switch (count)
        {
            case 1:
                SetButton(btn[0], btnImg[0], false);
                SetButton(btn[1], btnImg[1], true);                
                pageObj[0].SetActive(true);
                pageObj[1].SetActive(false);
                if (pageObj.Length == 3)
                    pageObj[2].SetActive(false);
                break;
            case 2:
                SetButton(btn[0], btnImg[1], true,-1);
                SetButton(btn[1], btnImg[0], false,-1);                
                pageObj[0].SetActive(false);
                pageObj[1].SetActive(true);
                if (pageObj.Length == 3)
                {
                    SetButton(btn[1], btnImg[1], true, 1);
                    pageObj[2].SetActive(false);
                }
                break;
            case 3:
                SetButton(btn[0], btnImg[1], true, -1);
                SetButton(btn[1], btnImg[0], false, -1);
                pageObj[0].SetActive(false);
                pageObj[1].SetActive(false);
                pageObj[2].SetActive(true);
                break;
        }
    }

    private void SetButton(GameObject index, Sprite spr, bool enable,int flip = 1)
    {
        if(flip == -1)
        { 
            index.transform.rotation = Quaternion.Euler(0,180,0);
        }
        else
        {
            index.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        Image buttonImage = index.transform.GetComponent<Image>();
        BoxCollider col = index.transform.GetComponent<BoxCollider>();

        buttonImage.sprite = spr;
        col.enabled = enable;
    }
}
