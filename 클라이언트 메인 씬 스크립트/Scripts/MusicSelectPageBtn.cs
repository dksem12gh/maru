using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSelectPageBtn : MonoBehaviour
{
    [SerializeField] RhythmSelected page;
    [SerializeField] GameObject[] btn;
    [SerializeField] GameObject[] pageObj;

    int count;

    private void Start()
    {
        count = 1;
    }

    public void LeftBtn()
    {
        count--;
        if (count <= 1) count = 1;        

        switch (count)
        {
            case 1:
                btn[0].SetActive(false);
                btn[1].SetActive(true);
                pageObj[0].SetActive(true);
                pageObj[1].SetActive(false);
                pageObj[2].SetActive(false);
                break;
            case 2:
                btn[0].SetActive(true);
                btn[1].SetActive(true);
                pageObj[0].SetActive(false);
                pageObj[1].SetActive(true);
                pageObj[2].SetActive(false);
                break;
            case 3:
                btn[0].SetActive(true);
                btn[1].SetActive(false);
                pageObj[0].SetActive(false);
                pageObj[1].SetActive(false);
                pageObj[2].SetActive(true);
                break;
        }
        page.NextPage();
    }

    public void RightBtn()
    {
        count++;
        if (count >= 3) count = 3;        

        switch (count)
        {
            case 1:
                btn[0].SetActive(false);
                btn[1].SetActive(true);
                pageObj[0].SetActive(true);
                pageObj[1].SetActive(false);
                pageObj[2].SetActive(false);
                break;
            case 2:
                btn[0].SetActive(true);
                btn[1].SetActive(true);
                pageObj[0].SetActive(false);
                pageObj[1].SetActive(true);
                pageObj[2].SetActive(false);
                break;
            case 3:
                btn[0].SetActive(true);
                btn[1].SetActive(false);
                pageObj[0].SetActive(false);
                pageObj[1].SetActive(false);
                pageObj[2].SetActive(true);
                break;
        }
        page.NextPage();
    }
}
