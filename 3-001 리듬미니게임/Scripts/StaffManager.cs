using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffManager : MonoBehaviour
{
    [Header("음표 프리팹")]
    [SerializeField] GameObject _quarterNote_prefeb;
    [Header("음표 표시 위치")]
    [SerializeField] Transform _note_Group;

    int _maxNote = 20;

    private void Start()
    {
        for(int i=0;i< _maxNote; i++)
        {
            GameObject obj = Instantiate(_quarterNote_prefeb, _note_Group);

            obj.GetComponent<QuarterNoteInfo>().SetActiveFalse();
        }
    }
    public void SetStaffNote(float num)
    {
        // 첫 자식 가져오기
        Transform firstChild = _note_Group.GetChild(0);

        // 마지막 위치로 이동
        firstChild.SetSiblingIndex(_note_Group.childCount - 1);

        firstChild.GetComponent<QuarterNoteInfo>().SetQuarterNote(num);
    }
}
