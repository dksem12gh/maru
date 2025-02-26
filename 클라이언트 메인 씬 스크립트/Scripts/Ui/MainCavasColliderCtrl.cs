using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainCavasColliderCtrl : MonoBehaviour
{
    [SerializeField] GameObject _select;
    public GameObject _exercise;
    public GameObject _stretching;
    public GameObject _leisure;
    [SerializeField] BoxCollider _exit;
    public string _name = null;

    //[SerializeField] GameObject[] _gamePageBtn;
    [SerializeField]List<BoxCollider> _selectCols = new List<BoxCollider>();

    [SerializeField] Sprite _nomalImg;
    [SerializeField] Sprite _selectImg;
    [SerializeField] Color32 _nomalColor;
    [SerializeField] Color32 _selectColor;

    [SerializeField] Sprite[] _ExerciseIcon;
    [SerializeField] Sprite[] _StretchingIcon;
    [SerializeField] Sprite[] _MiniGameIcon;

    List<BoxCollider> _exerciseCols = new List<BoxCollider>();
    List<BoxCollider> _stretchingCols = new List<BoxCollider>();
    List<BoxCollider> _leisureCols = new List<BoxCollider>();
    

    private void Start()
    {
        BoxCollider[] _col = _select.GetComponentsInChildren<BoxCollider>();

        foreach(BoxCollider col in _col)
        {
            _selectCols.Add(col);            
        }

        _col = _exercise.GetComponentsInChildren<BoxCollider>();

        foreach (BoxCollider col in _col)
        {
            _exerciseCols.Add(col);
        }
        _exerciseCols[8].transform.parent.gameObject.SetActive(false);
        _exerciseCols[16].transform.parent.gameObject.SetActive(false);

        _col = _stretching.GetComponentsInChildren<BoxCollider>();

        foreach (BoxCollider col in _col)
        {
            _stretchingCols.Add(col);
        }
        _stretchingCols[8].transform.parent.gameObject.SetActive(false);
        _stretchingCols[16].transform.parent.gameObject.SetActive(false);


        _col = _leisure.GetComponentsInChildren<BoxCollider>();

        foreach (BoxCollider col in _col)
        {
            _leisureCols.Add(col);
        }
        _leisureCols[8].transform.parent.gameObject.SetActive(false);

        SelectColOn();
        ExerciseColOff();
        LeisureColOff();
        StretchingColOff();
    }
    
    public void ExitColOff()
    {
        _exit.enabled = false;
    }
    public void ExitColOn()
    {
        _exit.enabled = true;
    }
    public void ShowMain()
    {
        //Managers.UI.Signal.SendSignal("MainFlow", "Training");
        ExerciseColOn();
        LeisureColOff();
        StretchingColOff();
        SelectNameExercise();
    }
    public void SelectColOn()
    {
        for(int i = 0; i<_selectCols.Count; i++)
        {
            _selectCols[i].enabled = true;
        }
    }

    public void SelectColOff()
    {
        for (int i = 0; i < _selectCols.Count; i++)
        {
            _selectCols[i].enabled = false;
        }
    }

    public void ExerciseColOn()
    {
        for (int i = 0; i < _exerciseCols.Count; i++)
        {
            _exerciseCols[i].enabled = true;
        }
    }

    public void ExerciseColOff()
    {
        for (int i = 0; i < _exerciseCols.Count; i++)
        {
            _exerciseCols[i].enabled = false;
        }
    }

    public void StretchingColOn()
    {
        for (int i = 0; i < _stretchingCols.Count; i++)
        {
            _stretchingCols[i].enabled = true;
        }
    }

    public void StretchingColOff()
    {
        for (int i = 0; i < _stretchingCols.Count; i++)
        {
            _stretchingCols[i].enabled = false;
        }
    }

    public void LeisureColOn()
    {
        for (int i = 0; i < _leisureCols.Count; i++)
        {
            _leisureCols[i].enabled = true;
        }
    }

    public void LeisureColOff()
    {
        for (int i = 0; i < _leisureCols.Count; i++)
        {
            _leisureCols[i].enabled = false;
        }
    }

    /*public void PageBtnOff()
    {
        foreach(GameObject obj in _gamePageBtn)
        {
            obj.SetActive(false);
        }
    }*/
    /*public void PageBtnOn()
    {
        foreach (GameObject obj in _gamePageBtn)
        {
            obj.SetActive(true);
        }
    }*/

    public void SelectNameExercise()
    {
        _name = _selectCols[0].transform.parent.name;        

        _selectCols[0].gameObject.GetComponent<Image>().sprite = _selectImg;
        _selectCols[0].gameObject.GetComponentInChildren<TMP_Text>().color = _selectColor;
        _selectCols[0].gameObject.transform.GetChild(1).GetComponent<Image>().sprite = _ExerciseIcon[1];

        _selectCols[1].gameObject.GetComponent<Image>().sprite = _nomalImg;
        _selectCols[1].gameObject.GetComponentInChildren<TMP_Text>().color = _nomalColor;
        _selectCols[1].gameObject.transform.GetChild(1).GetComponent<Image>().sprite = _StretchingIcon[0];

        _selectCols[2].gameObject.GetComponent<Image>().sprite = _nomalImg;
        _selectCols[2].gameObject.GetComponentInChildren<TMP_Text>().color = _nomalColor;
        _selectCols[2].gameObject.transform.GetChild(1).GetComponent<Image>().sprite = _MiniGameIcon[0];
    }

    public void SelectNameStretching()
    {
        _name = _selectCols[1].transform.parent.name;        

        _selectCols[0].gameObject.GetComponent<Image>().sprite = _nomalImg;
        _selectCols[0].gameObject.GetComponentInChildren<TMP_Text>().color = _nomalColor;
        _selectCols[0].gameObject.transform.GetChild(1).GetComponent<Image>().sprite = _ExerciseIcon[0];

        _selectCols[1].gameObject.GetComponent<Image>().sprite = _selectImg;
        _selectCols[1].gameObject.GetComponentInChildren<TMP_Text>().color = _selectColor;
        _selectCols[1].gameObject.transform.GetChild(1).GetComponent<Image>().sprite = _StretchingIcon[1];

        _selectCols[2].gameObject.GetComponent<Image>().sprite = _nomalImg;
        _selectCols[2].gameObject.GetComponentInChildren<TMP_Text>().color = _nomalColor;
        _selectCols[2].gameObject.transform.GetChild(1).GetComponent<Image>().sprite = _MiniGameIcon[0];
    }
    public void SelectNameLeisure()
    {
        _name = _selectCols[2].transform.parent.name;        

        _selectCols[0].gameObject.GetComponent<Image>().sprite = _nomalImg;
        _selectCols[0].gameObject.GetComponentInChildren<TMP_Text>().color = _nomalColor;
        _selectCols[0].gameObject.transform.GetChild(1).GetComponent<Image>().sprite = _ExerciseIcon[0];

        _selectCols[1].gameObject.GetComponent<Image>().sprite = _nomalImg;
        _selectCols[1].gameObject.GetComponentInChildren<TMP_Text>().color = _nomalColor;
        _selectCols[1].gameObject.transform.GetChild(1).GetComponent<Image>().sprite = _StretchingIcon[0];

        _selectCols[2].gameObject.GetComponent<Image>().sprite = _selectImg;
        _selectCols[2].gameObject.GetComponentInChildren<TMP_Text>().color = _selectColor;
        _selectCols[2].gameObject.transform.GetChild(1).GetComponent<Image>().sprite = _MiniGameIcon[1];
    }
}
