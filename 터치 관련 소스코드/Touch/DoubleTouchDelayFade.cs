using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TouchEventHandler))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(SpriteRenderer))]
public class DoubleTouchDelayFade : MonoBehaviour
{
    private Color m_OriginColor;
    private TouchEventHandler m_TouchEventHandler;
    private Collider m_Collider;
    private SpriteRenderer m_SpriteRenderer;

    public float m_Delay = 0.5f;

    private void Awake()
    {
        if (m_Collider == null)
        {
            m_Collider = GetComponent<Collider>();
        }

        if (m_SpriteRenderer == null)
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
        }
        m_OriginColor = m_SpriteRenderer.color;

        if (m_TouchEventHandler == null)
        {
            m_TouchEventHandler = this.transform.GetComponent<TouchEventHandler>();
        }
        m_TouchEventHandler.m_PressedEvent.AddListener((Vector3 pos) => StartDelay());
    }

    private void OnEnable()
    {
        m_SpriteRenderer.color = m_OriginColor;
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
        Color startColor = new Color(0.5f, 0.5f, 0.5f, 1);

        m_SpriteRenderer.color = startColor;

        for (float time = 0; time < m_Delay; time += Time.deltaTime)
        {
            m_SpriteRenderer.color = Color.Lerp(startColor, m_OriginColor, time / m_Delay);
            yield return null;
        }
        m_SpriteRenderer.color = m_OriginColor;

        m_Collider.enabled = true;
    }
}
