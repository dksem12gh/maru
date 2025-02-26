using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DigitalMaru.Exercise.Walk
{
    public class GameStagePlayer : MonoBehaviour
    {
        [Header("플레이어 인덱스")]
        [SerializeField] int playerIndext;
        [Header("패드들")]
        [SerializeField] List<GamePad> _padList;
        [Header("랩 이벤트")]
        [SerializeField] UnityEvent<int> _repEvent;

        private GamePad _selectedPad;




        public void SetLock(bool value)
        {
            foreach (var p in _padList)
            {
                p.SetLock(value);
            }
        }

        public void Clear()
        {
            foreach (var p in _padList)
            {
                p.Clear();
            }
        }

        public void Init()
        {
            foreach (var p in _padList)
            {
                p.Init();
            }
        }

        public void Play()
        {
            _selectedPad = _padList[0];
            _selectedPad.Ready();
        }

        public void Touched(GamePad target)
        {
            if (_selectedPad != target)
            {
                Debug.LogWarning("Touched is Not equal. _selectedPad != target");
            }

            if (IsLast(target))
            {
                _repEvent?.Invoke(playerIndext);
            }

            _selectedPad = GetNext();
            _selectedPad.Ready();
        }

        public void OnPause(bool pause)
        {
            foreach (var p in _padList)
            {
                p.SetLock(true);
            }

            if (pause is false)
            {
                if (_selectedPad != null)
                {
                    _selectedPad.Pause(false);
                }
            }
        }


        private bool IsLast(GamePad target)
        {
            return _padList[_padList.Count - 1] == target;
        }


        private GamePad GetNext()
        {
            int index = 0;
            for (; index < _padList.Count; index++)
            {
                if (_selectedPad == _padList[index])
                {
                    break;
                }
            }
            index = (index + 1) % _padList.Count;
            return _padList[index];
        }
    }
}
