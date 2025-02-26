using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnableTouchDelay : MonoBehaviour
{
    private Collider m_Collider;

    public float m_Delay = 0.5f;

    private void Awake()
    {
        if (m_Collider == null)
        {
            m_Collider = GetComponent<Collider>();
        }
    }

    private void OnEnable()
    {
        m_Collider.enabled = false;
        StartDelay();
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
