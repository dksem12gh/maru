using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DigitalMaru
{    
    /// <summary>
    /// Editor에서 키보드로 터치 테스트 하기 위한 스크립트.
    /// 빌드시에는 포함 안 시킴.
    /// </summary>
    public class TouchEventKeyboardHandler : MonoBehaviour
    {
        [SerializeField] private TouchEventHandler target;
        [SerializeField] private KeyCode keyCode;

        private void Reset()
        {
            target = GetComponent<TouchEventHandler>();
        }
         
        private void Update()
        {                    
            if (Input.GetKeyDown(keyCode) || Input.GetKey(keyCode) || Input.GetKeyUp(keyCode))
            {
                HandleInput();
            }
        }
        private void HandleInput()
        {
            Vector3 touchPosition = Vector3.zero;

            switch (true)
            {
                case var _ when Input.GetKeyDown(keyCode):
                    target.TouchPressed(touchPosition);
                    break;
                case var _ when Input.GetKey(keyCode):
                    target.TouchUpdated(touchPosition);
                    break;
                case var _ when Input.GetKeyUp(keyCode):
                    target.TouchReleased(touchPosition);
                    break;
            }
        }

    }
}
