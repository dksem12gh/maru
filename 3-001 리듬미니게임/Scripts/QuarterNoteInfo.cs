using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class QuarterNoteInfo : MonoBehaviour
{
    [SerializeField] GameObject _postion;
    [SerializeField] GameObject _head;
    [SerializeField] GameObject _sharp;
    [SerializeField] GameObject _do;

    public void SetQuarterNote(float num)
    {
        _postion.SetActive(true);

        //도 표시용
        bool isDo = (int)num == 0 ? true : false;
        _do.SetActive(isDo);

        //샤프 표시용
        bool isSharp = num % 1 == 0.5f ? true : false;
        _sharp.SetActive(isSharp);

        //소숫점 아래 버리기
        num = (int)num;

        //Y축 위치 이동
        Vector3 pos = _postion.GetComponent<RectTransform>().localPosition;        

        pos.y = num * 17.5f; //17.5가 한음의 차이
        _postion.GetComponent<RectTransform>().localPosition = pos;

        //라부터 뒤집기
        int zRotation = num >= 5 ? 180 : 0;
        _head.transform.eulerAngles = new Vector3(0, 0, zRotation);

        StartCoroutine(Move());
    }   
    public void SetActiveFalse()
    {
        _postion.SetActive(false);
    }
    private IEnumerator Move() //방향 반대로 감
    {
        float duration = 1f; // 이동 시간 (초)
        float elapsedTime = 0f;

        Vector3 startPosition = new Vector3(450, 0, 0); // 초기 위치
        Vector3 targetPosition = new Vector3(-450, 0, 0); // 목표 위치
        Debug.Log(startPosition);
        Debug.Log(transform.localPosition);
        
        while (elapsedTime < duration)
        {
            // 시간 경과 비율
            elapsedTime += Time.deltaTime;

            // 선형 보간 (Lerp)
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);

            yield return null; // 다음 프레임까지 대기
        }

        SetActiveFalse();
    }
}
