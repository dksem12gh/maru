// ===================================================================
//
//  abstract GameManager
//
//  MonoBehaviour의 Start 메소드에서 MainLoop을 호출해주세요.
//
//
//  void Start()
//  {
//      _playMode = Managers.GameTime == -1 ? DigitalMaru.PlayMode.Infinity : DigitalMaru.PlayMode.Normal;
//      _gameTimeSec = Managers.GameTime > 0 ? Managers.GameTime * 60: -1;
//      StartCoroutine(DoMainLoop(_gameTimeSec));
//   }
// ===================================================================

using System.Collections;
using System.Collections.Generic;
using TouchScript.Examples.RawInput;
using UnityEngine;

namespace DigitalMaru
{
    public abstract class AGameManager : MonoBehaviour
    {
        protected virtual IEnumerator DoMainLoop()
        {
            yield return DoInit();
            yield return DoTitle();
            yield return DoRun(); 
            yield return DoResult();
        }

        protected abstract IEnumerator DoInit();
        protected abstract IEnumerator DoTitle();
        protected abstract IEnumerator DoRun();
        protected abstract IEnumerator DoResult();
    }
}