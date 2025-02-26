using UnityEngine;

public class ButtonSignalHandler : MonoBehaviour
{
    [SerializeField] string _Category;
    [SerializeField] string _Name;

    public void SendSiginal()
    {
        Managers.UI.Signal.SendSignal(_Category,_Name);
    }
}
