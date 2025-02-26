using UnityEngine;
public class PopupSettingCanvas : MonoBehaviour
{
    private void Start()
    {
        //Managers.Data.JsonLoad();

        this.transform.parent.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        this.transform.parent.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    public void backBtn()
    {
        Managers.UI.Popup.BackBtn(Vector3.zero);
    }
}
