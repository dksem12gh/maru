using UnityEngine;

public class CursorSwitcher : MonoBehaviour
{
    public Canvas m_Canvas;
    private int m_InitTargetDisplay;

    private void Awake()
    {
        m_InitTargetDisplay = m_Canvas.targetDisplay;
        m_Canvas.targetDisplay = -1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F7))
        {
            if (m_Canvas.targetDisplay == -1)
                m_Canvas.targetDisplay = m_InitTargetDisplay;
            else
                m_Canvas.targetDisplay = -1;
        }
    }
}
