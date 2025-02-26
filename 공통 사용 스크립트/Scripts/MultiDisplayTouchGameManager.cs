using System.Collections;
using TouchScript.Examples.RawInput;
using UnityEngine;


namespace DigitalMaru
{

    public abstract class MultiDisplayTouchGameManager : AGameManager
    {
        [Header("Common")]
        [SerializeField] protected Camera SubGameCamera;

        protected int GameGoal => Managers.GameTime;

        protected SelectGameState CurrentGameState
        {
            get
            {
                if (Managers.SelGameSet != null) return Managers.SelGameSet.selectGameState;
                return SelectGameState.Play;
            }
        }


        protected bool IsGameFinish => CurrentGameState == SelectGameState.Finish;



        protected virtual void Awake()
        {
            MultiDisplayTouchManager multiDisplayTouchManager = FindObjectOfType<MultiDisplayTouchManager>();
            multiDisplayTouchManager.SetCameraAt(1, SubGameCamera);
        }

        void Start()
        {
            StartCoroutine(DoMainLoop());
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                switch (Managers.SelGameSet.selectGameState)
                {
                    case SelectGameState.Play:
                    case SelectGameState.UnPause:
                        Managers.SelGameSet.selectGameState = SelectGameState.Pause;
                        break;
                    case SelectGameState.Pause:
                        Managers.SelGameSet.selectGameState = SelectGameState.UnPause;
                        break;
                }
            }            
        }
#endif

        protected override IEnumerator DoMainLoop()
        {
            StartCoroutine(PauseRoutine());
            yield return DoInit();
            yield return DoTitle();
            yield return DoRun();
            yield return DoResult();
        }


        IEnumerator PauseRoutine()
        {
            var oldState = CurrentGameState;
            while (oldState != SelectGameState.Finish)
            {
                if (oldState != CurrentGameState)
                {
                    switch (CurrentGameState)
                    {
                        case SelectGameState.Pause:
                            OnPause(true);
                            break;
                        case SelectGameState.UnPause:
                            OnPause(false);
                            break;
                    }
                }                
                oldState = CurrentGameState;
                yield return YieldInstructionCache.WaitForEndOfFrame;
            }
        }

        protected abstract void OnPause(bool pause);
    }
}
