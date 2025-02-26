using UnityEngine;
using UnityEngine.Events;

public class TouchEventHandler : MonoBehaviour
{
    public UnityEvent<Vector3> m_PressedEvent;
    public UnityEvent<Vector3> m_UpdatedEvent;    
    public UnityEvent<Vector3> m_ReleasedEvent;


    Collider touchCollider;

    Collider GetCollider()
    {
        if (null == touchCollider)
            touchCollider = GetComponent<Collider>();
        return touchCollider;
    }

    public bool IsLock
    {
        get
        {
            if (null != GetCollider()) return GetCollider().enabled is false;
            return false;
        }
    }

    public virtual void TouchPressed(Vector3 pos)
    {
        m_PressedEvent?.Invoke(pos);
    }

    public virtual void TouchUpdated(Vector3 pos)
    {
        m_UpdatedEvent?.Invoke(pos);
    }

    public virtual void TouchReleased(Vector3 pos)
    {
        m_ReleasedEvent?.Invoke(pos);
    }

    public virtual void Stop()
    {

    }

    public void SetLock(bool value)
    {        
        if(GetCollider() != null)
        {
            GetCollider().enabled = value is false; 
        }
    }

    public void SetLock(bool value,int index)
    {
        if (GetCollider() != null)
        {
            GetCollider().enabled = value is false;
        }
    }

#if UNITY_EDITOR
    [SerializeField] private KeyCode keyCode;
    
    private void Update()
    {                    
        if (Input.GetKeyDown(keyCode) || Input.GetKey(keyCode) || Input.GetKeyUp(keyCode))
        {
            HandleInput();
        }
    }
    private void HandleInput()
    {
        if (IsLock) return;

        Vector3 touchPosition = Vector3.zero;
        switch (true)
        {
            case var _ when Input.GetKeyDown(keyCode):
                TouchPressed(touchPosition);
                break;
            case var _ when Input.GetKey(keyCode):
                TouchUpdated(touchPosition);
                break;
            case var _ when Input.GetKeyUp(keyCode):
                TouchReleased(touchPosition);
                break;
        }
    }
#endif
}
