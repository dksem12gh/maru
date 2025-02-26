using System.Collections;
using System.Collections.Generic;
using TouchScript.Examples.RawInput;
using UnityEngine;

public class SubCameraManager : MonoBehaviour
{
    [SerializeField] GameObject subPauseCamera;
    public GameObject subReadyCamera;
    [SerializeField] MultiDisplayTouchManager multiDisplayTouchManager;

    public void PauseSceneOn()
    {
        subReadyCamera.SetActive(false);
        subPauseCamera.SetActive(true);
    }

    public void PauseSceneOff()
    {
        subReadyCamera.SetActive(false);
        subPauseCamera.SetActive(false);
    }

    public void GameSceneOff()
    {
        subReadyCamera.SetActive(true);
        subPauseCamera.SetActive(false);
    }
    public void GameSceneOn()
    {
        subReadyCamera.SetActive(false);
        subPauseCamera.SetActive(false);
    }

    public void CameraChange()
    {        
        multiDisplayTouchManager.MainCam[1] = Managers.SubCamera.subReadyCamera.GetComponent<Camera>();
    }
}
