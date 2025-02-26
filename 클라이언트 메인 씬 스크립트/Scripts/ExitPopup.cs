using Doozy.Runtime.UIManager.Components;
using System.Collections;
using System.Collections.Generic;
using TouchScript.Examples.RawInput;
using UnityEngine;

public class ExitPopup : MonoBehaviour
{    
    public void exit()
    {
        Managers.PopupCol.popupColliderList.Remove(Managers.PopupCol.popupColliderList.GetLastItem());
        Managers.SubCamera.CameraChange();
    }
}
