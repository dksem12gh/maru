using System.Collections;
using UnityEngine;

#if UNITY_STANDALONE_WIN

using System;
using System.Runtime.InteropServices;

#endif

namespace TouchScript.Examples.RawInput
{
    public class MultitouchManager : MonoBehaviour
    {
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string className, string windowName);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [Header("터치 시 해당 위치의 모든 오브젝트를 터치하려면 true, 가장 가까운 오브젝트만 터치하려면 false")]
        public bool m_IsOverlayTouch = false;
        [Header("Scene 화면에서 Ray 디버깅 시 true")]
        public bool m_UseDrawRay = false;
        [Header("터치될 오브젝트의 Tag")]
        public string TargetTag = "TouchObject";
        [Header("카메라를 기준으로 터치 가능한 거리")]
        public float m_RayDistance = 300f;
        public Camera[] MainCam;

        private void Awake()
        {
            for (int i = 0; i < MainCam.Length && i < Display.displays.Length; i++)
            {
                Display.displays[i].Activate();
            }

            StartCoroutine(SetWindowPosition(0, 0, 1920 * MainCam.Length, 1080));
        }

        public static IEnumerator SetWindowPosition(int x, int y, int width, int height)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            MoveWindow(FindWindow(null, Application.productName), x, y, width, height, true);
        }

        private void OnEnable()
        {
            if (TouchManager.Instance != null)
            {
                TouchManager.Instance.PointersPressed += PointersPressedHandler;
                TouchManager.Instance.PointersReleased += PointersReleasedHandler;
                TouchManager.Instance.PointersUpdated += PointersUpdatedHandler;
            }
        }

        private void OnDisable()
        {
            if (TouchManager.Instance != null)
            {
                TouchManager.Instance.PointersPressed -= PointersPressedHandler;
                TouchManager.Instance.PointersReleased -= PointersReleasedHandler;
                TouchManager.Instance.PointersUpdated -= PointersUpdatedHandler;
            }
        }

        private void PointersPressedHandler(object sender, PointerEventArgs e)
        {
            TouchEvent(e, (TouchEventHandler evt, Vector3 pos) => { evt.TouchPressed(pos); });
        }

        private void PointersReleasedHandler(object sender, PointerEventArgs e)
        {
            TouchEvent(e, (TouchEventHandler evt, Vector3 pos) => { evt.TouchReleased(pos); });
        }

        private void PointersUpdatedHandler(object sender, PointerEventArgs e)
        {
            TouchEvent(e, (TouchEventHandler evt, Vector3 pos) => { evt.TouchUpdated(pos); });
        }


        private void TouchEvent(PointerEventArgs e, Action<TouchEventHandler, Vector3> p)
        {
            if (Time.timeScale == 0)
                return;

            foreach (var pointer in e.Pointers)
            {
                Camera camera = GetCameraWithPointer(pointer);

                if (camera != null)
                {
                    Ray ray = camera.ScreenPointToRay(pointer.Position);
                    RaycastHit[] hits = Physics.RaycastAll(ray, m_RayDistance);

                    if (m_UseDrawRay)
                    {
                        Debug.DrawRay(ray.origin, ray.direction * m_RayDistance, Color.red, 1);
                    }

                    if (m_IsOverlayTouch)
                    {
                        foreach (RaycastHit hit in hits)
                            if (hit.collider.CompareTag(TargetTag))
                                p.Invoke(hit.collider.GetComponent<TouchEventHandler>(), hit.point);
                    }
                    else if (1 <= hits.Length)
                    {
                        Array.Sort<RaycastHit>(hits, (a, b) => a.distance.CompareTo(b.distance));
                        RaycastHit hit = hits[0];

                        if (hit.collider.CompareTag(TargetTag))
                            p.Invoke(hit.collider.GetComponent<TouchEventHandler>(), hit.point);
                    }
                }
            }
        }

        private Camera GetCameraWithPointer(Pointers.Pointer pointer)
        {
            if (0 < pointer.Position.y && pointer.Position.y < Screen.height)
            {
                for (int i = 0; i < MainCam.Length; i++)
                {
                    if (pointer.Position.x > Screen.width / MainCam.Length * (i) && pointer.Position.x < Screen.width / MainCam.Length * (i + 1))
                    {
                        return MainCam[i];
                    }
                }
            }

            return null;
        }
    }
}
