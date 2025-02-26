using System.Collections;
using UnityEngine;

[RequireComponent(typeof(TouchEventHandler))]
[RequireComponent(typeof(Collider))]
public class DoubleTouchDelay : MonoBehaviour
{
    private TouchEventHandler m_TouchEventHandler;
    private Collider m_Collider;

    public float m_Delay = 0.5f;

    private void Awake()
    {
        if (m_Collider == null)
        {
            m_Collider = GetComponent<Collider>();
        }

        if (m_TouchEventHandler == null)
        {
            m_TouchEventHandler = this.transform.GetComponent<TouchEventHandler>();
        }
        m_TouchEventHandler.m_PressedEvent.AddListener((Vector3 pos) => StartDelay());
    }

    private void OnEnable()
    {
        m_Collider.enabled = true;
    }

    private void StartDelay()
    {
        StopAllCoroutines();
        m_Collider.enabled = false;
        StartCoroutine(ButtonDelay());
    }

    private IEnumerator ButtonDelay()
    {
        yield return YieldInstructionCache.WaitForSeconds(m_Delay);
        m_Collider.enabled = true;
    }
}
