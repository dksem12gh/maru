using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using TouchScript.InputSources;

#if UNITY_STANDALONE_WIN
using System.Runtime.InteropServices;
#endif

namespace TouchScript.Examples.RawInput
{
    public class MultiDisplayTouchManager : MonoBehaviour
    {
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        private static extern bool SetWindowPos(IntPtr hwnd, int hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

        [DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern IntPtr FindWindow(string className, string windowName);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        private static FieldInfo lastFocusedFieldInfo;
        private static FieldInfo targetDisplayFieldInfo;

        public class TouchPoint
        {
            public Vector3 TouchPos = Vector2.zero;
            public int TouchPosCam = 0;
            public bool check = false;
        }

        [Header("터치 시 해당 위치의 모든 오브젝트를 터치하려면 true, 가장 가까운 오브젝트만 터치하려면 false")]
        public bool m_IsOverlayTouch = false;
        [Header("Scene 화면에서 Ray 디버깅 시 true")]
        public bool m_UseDrawRay = false;
        [Header("터치될 오브젝트의 Tag")]
        public string TargetTag = "TouchObject";
        [Header("카메라를 기준으로 터치 가능한 거리")]
        public float m_RayDistance = 300f;

        public Camera[] MainCam;

        public CursorController m_CursorController;

        int targetCam_n;

        public bool exit = false;

        public void SetCameraAt(int index, Camera camera)
        {
            if(index <= (MainCam.Length-1))
                MainCam[index] = camera;
        }

        private void Awake()
        {
#if UNITY_EDITOR
            Assembly assembly = typeof(UnityEditor.EditorWindow).Assembly;

            Type playModeViewType = assembly.GetType("UnityEditor.PlayModeView");
            lastFocusedFieldInfo = playModeViewType.GetField("s_LastFocused", BindingFlags.NonPublic | BindingFlags.Static);
            targetDisplayFieldInfo = playModeViewType.GetField("m_TargetDisplay", BindingFlags.NonPublic | BindingFlags.Instance);
#endif

            StartCoroutine(SetWindowPosition(0, 0));

            for (int i = 0; i < MainCam.Length && i < Display.displays.Length; i++)
            {
                Display.displays[i].Activate();
            }

            Application.targetFrameRate = 60;
        }

        public static IEnumerator SetWindowPosition(int x, int y)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            SetWindowPos(FindWindow(null, Application.productName), 0, x, y, 0, 0, 5);
        }

        private void OnEnable()
        {
            if (TouchManager.Instance != null)
            {
                TouchManager.Instance.PointersPressed += PointersPressedHandler;
                TouchManager.Instance.PointersUpdated += PointersUpdatedHandler;
                TouchManager.Instance.PointersReleased += PointersReleasedHandler;                
            }
        }

        private void OnDisable()
        {
            if (TouchManager.Instance != null)
            {
                TouchManager.Instance.PointersPressed -= PointersPressedHandler;
                TouchManager.Instance.PointersUpdated -= PointersUpdatedHandler;
                TouchManager.Instance.PointersReleased -= PointersReleasedHandler;                
            }
        }

        private void Update()
        {
            Event m_Event = Event.current;

            if (Input.GetMouseButtonDown(0))
            {
                MouseTouchEvent((TouchEventHandler evt, Vector3 pos) => { evt.TouchPressed(pos); });
            }

            if (Input.GetMouseButton(0))
            {
                MouseTouchEvent((TouchEventHandler evt, Vector3 pos) => { evt.TouchUpdated(pos); }); 
            }

            if (Input.GetMouseButtonUp(0))
            {
                MouseTouchEvent((TouchEventHandler evt, Vector3 pos) => { evt.TouchReleased(pos); });
            }
        }

        private void PointersPressedHandler(object sender, PointerEventArgs e)
        {
            ScreenTouchEvent(e.Pointers, (TouchEventHandler evt, Vector3 pos) => { evt.TouchPressed(pos); });
        }
        private void PointersUpdatedHandler(object sender, PointerEventArgs e)
        {
            ScreenTouchEvent(e.Pointers, (TouchEventHandler evt, Vector3 pos) => { evt.TouchUpdated(pos); });
        }
        private void PointersReleasedHandler(object sender, PointerEventArgs e)
        {
            ScreenTouchEvent(e.Pointers, (TouchEventHandler evt, Vector3 pos) => { evt.TouchReleased(pos); });
        }

        // TUIO 터치 이벤트
        private void ScreenTouchEvent(IList<Pointers.Pointer> pointers, Action<TouchEventHandler, Vector3> touchEvent)
        {
            if (pointers == null) return;
            if (touchEvent == null) return;

            foreach (var pointer in pointers)
            {
                if (pointer.InputSource.GetType().Equals(typeof(TuioInput)))
                {
                    targetCam_n = 1;
                }
                else
                {
                    targetCam_n = 0;
                }

                if (pointer.Type != Pointers.Pointer.PointerType.Mouse)
                {
                    Vector3 touchPosition;

                    //if (MainCam.Length > 1)
                    //{
                    //    targetCam_n = (int)pointer.Position.x * MainCam.Length / Screen.width;
                    //    touchPosition = new Vector3((pointer.Position.x * MainCam.Length) - (Screen.width * targetCam_n), pointer.Position.y, 0);
                    //}
                    //else
                    //{
                    touchPosition = pointer.Position;
                    //}

                    if (MainCam.Length <= targetCam_n)
                    {
                        return;
                    }
                    if (MainCam[targetCam_n] == null) return;

                    m_CursorController.SetScreenTouchCursor(MainCam[targetCam_n].targetDisplay, touchPosition);
                    Touched(touchPosition, targetCam_n, (TouchEventHandler evt, Vector3 pos) => { touchEvent?.Invoke(evt, pos); });
                }
            }
        }

        // 마우스 터치 이벤트
        private void MouseTouchEvent(Action<TouchEventHandler, Vector3> touchEvent)
        {
            int targetCam_n;
            Vector3 point;
#if UNITY_EDITOR
            object playModeView = lastFocusedFieldInfo.GetValue(null);
            int displayIndex = 0;
            if (playModeView != null) displayIndex = (int)targetDisplayFieldInfo.GetValue(playModeView);
            targetCam_n = displayIndex;
            point = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
#else
            targetCam_n = (int)Input.mousePosition.x / Screen.width;
            point = new Vector3(Input.mousePosition.x % Screen.width, Input.mousePosition.y, 0);
#endif

            if (MainCam.Length <= targetCam_n)
            {
                return;
            }
            if (MainCam[targetCam_n] == null) return;

            m_CursorController.SetMouseCursor(MainCam[targetCam_n].targetDisplay, point);

            Touched(point, targetCam_n, (TouchEventHandler evt, Vector3 pos) => { touchEvent?.Invoke(evt, pos); });
        }

        // 터치 Ray 발사 이벤트
        private void Touched(Vector3 pos, int targetCam_n, Action<TouchEventHandler, Vector3> p)
        {

            if (pos.y < 0 || MainCam[targetCam_n].pixelHeight < pos.y || pos.x < 0 || MainCam[targetCam_n].pixelWidth < pos.x)
            {
                return;
            }

            Ray ray = MainCam[targetCam_n].ScreenPointToRay(pos);
            int layerMask = 1 << LayerMask.NameToLayer("Touch");

            RaycastHit[] hits = Physics.RaycastAll(ray, m_RayDistance, layerMask);


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
                {
                    TouchEventHandler touchEventHandler = hit.collider.GetComponent<TouchEventHandler>();
                    p.Invoke(touchEventHandler, hit.point);
                }
            }
        }
    }
}