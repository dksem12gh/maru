using UnityEngine;
using TouchScript.Gestures;

namespace TouchScript.Examples.Tap
{
    public class InputTouch : MonoBehaviour
    {
        private PressGesture p_gesture;
        private ReleaseGesture r_gesture;
        private TapGesture t_gesture;
        public Camera activeCamera;

        private void OnEnable()
        {
            p_gesture = GetComponent<PressGesture>();
            r_gesture = GetComponent<ReleaseGesture>();
            t_gesture = GetComponent<TapGesture>();

            p_gesture.Pressed += pressedHandler;
            r_gesture.Released += releasedHandler;
            t_gesture.Tapped += tappedHandler;
        }

        private void OnDisable()
        {
            p_gesture.Pressed -= pressedHandler;
            r_gesture.Released -= releasedHandler;
            t_gesture.Tapped -= tappedHandler;
        }

        private void pressedHandler(object sender, System.EventArgs e)
        {
            if (p_gesture.ScreenPosition.magnitude >= 0)
            {
                var ray = activeCamera.ScreenPointToRay(p_gesture.ScreenPosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) && hit.transform == transform)
                {
                    Debug.Log("프레스");
                }
            }
        }
        private void releasedHandler(object sender, System.EventArgs e)
        {
            if (r_gesture.ScreenPosition.magnitude >= 0)
            {
                var ray = activeCamera.ScreenPointToRay(r_gesture.ScreenPosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) && hit.transform == transform)
                {
                    Debug.Log("릴리즈");
                }
            }
        }
        private void tappedHandler(object sender, System.EventArgs e)
        {
            if (t_gesture.ScreenPosition.magnitude >= 0)
            {
                var ray = activeCamera.ScreenPointToRay(t_gesture.ScreenPosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) && hit.transform == transform)
                {
                    Debug.Log("탭");
                }
            }
        }
    }
}