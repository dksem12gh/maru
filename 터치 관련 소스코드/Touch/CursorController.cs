using System;
using System.Collections;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    private bool m_IsActive;
    private Coroutine m_MouseCoroutine;
    private Coroutine m_TouchCoroutine;

    public Canvas m_MouseCursorCanvas;
    public RectTransform m_MouseCursor;

    public Canvas m_TouchCursorCanvas;
    public RectTransform m_TouchCursor;

    private void Awake()
    {
        m_IsActive = false;
        m_MouseCursor.gameObject.SetActive(false);
        m_TouchCursor.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F7))
        {
            m_IsActive = !m_IsActive;
            m_MouseCursor.gameObject.SetActive(false);
            m_MouseCursor.gameObject.SetActive(false);
        }
        if (m_IsActive)
        {
            if (Input.GetMouseButtonUp(0))
            {
                m_MouseCursor.gameObject.SetActive(false);
            }
        }
    }

    public void SetMouseCursor(int targetDisplay, Vector2 position)
    {
        if (m_IsActive)
        {
            if (m_MouseCoroutine != null)
            {
                StopCoroutine(m_MouseCoroutine);
            }
            m_MouseCursor.gameObject.SetActive(true);
            m_MouseCursorCanvas.targetDisplay = targetDisplay;
            m_MouseCursor.anchoredPosition = position;

            m_MouseCoroutine = StartCoroutine(WaitFrameAndAction(() => m_MouseCursor.gameObject.SetActive(false)));
        }
    }

    public void SetScreenTouchCursor(int targetDisplay, Vector2 position)
    {
        if (m_IsActive)
        {
            if (m_TouchCoroutine != null)
            {
                StopCoroutine(m_TouchCoroutine);
            }

            m_TouchCursor.gameObject.SetActive(true);
            m_TouchCursorCanvas.targetDisplay = targetDisplay;
            m_TouchCursor.anchoredPosition = position;

            m_TouchCoroutine = StartCoroutine(WaitFrameAndAction(() => m_TouchCursor.gameObject.SetActive(false)));
        }
    }

    private IEnumerator WaitFrameAndAction(Action callback)
    {
        yield return null;
        callback?.Invoke();
    }
}
