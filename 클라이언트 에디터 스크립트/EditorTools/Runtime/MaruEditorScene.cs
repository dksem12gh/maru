#if UNITY_EDITOR

using System.Collections;
using TouchScript.InputSources;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DigitalMaru.Editor
{
    public class MaruEditorScene : MonoBehaviour
    {
        [SerializeField] private GameObject MultiDisplayTouchManager;
        [SerializeField] private GameObject Cursors;

        [SerializeField] int gameTime;
        [SerializeField] int gameLevel;
        [SerializeField] string contentScenePath;

        IEnumerator Start()
        {
            CreateAudioListener();
            CreateCursor();
            CreateMultiDisplayTouchManager();
            SetupManagersData();
            //yield return LoadMainScene();
            yield return new WaitForEndOfFrame();
            yield return LoadContentScene();
        }


        private void OnDestroy()
        {
            RecoveryScene();
        }

        public void Setup(string contentScenePath, int gameTime,int gameLevel)
        {            
            this.contentScenePath = contentScenePath;
            this.gameTime = gameTime;
            this.gameLevel = gameLevel;
        }
   

        void CreateAudioListener()
        {
            new GameObject("Audio Listener").AddComponent <AudioListener>();
        }

        void CreateCursor()
        {
            //var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/TouchScript/Prefabs/Cursors.prefab");
            Object.Instantiate(Cursors).GetComponent<Canvas>().targetDisplay = 1;
        }


        void CreateMultiDisplayTouchManager()
        {
            var go = Object.Instantiate(MultiDisplayTouchManager);
            GameObject.FindObjectOfType<TuioInput>(true).SupportedInputs =
            TuioInput.InputType.Cursors | TuioInput.InputType.Objects | TuioInput.InputType.Blobs;
        }

        void SetupManagersData()
        {
            Managers.GameTime = gameTime;
            Managers.GameLevel = gameLevel;
            Managers.SelGameSet = gameObject.AddComponent<SelectGameSetting>();
            Managers.SelGameSet.selectGameState = SelectGameState.Play;
            Managers.SelGameSet.rhythmMode = RhythmGameMode.FreeMode;           
        }
        
        IEnumerator LoadMainScene()
        {
            yield return EditorSceneManager.LoadSceneAsync("main", LoadSceneMode.Additive);                        
            EditorSceneManager.SetActiveScene(EditorSceneManager.GetSceneByName("main"));
            GameObject.FindObjectOfType<TuioInput>(true).SupportedInputs = 
                TuioInput.InputType.Cursors | TuioInput.InputType.Objects | TuioInput.InputType.Blobs;
        }

        IEnumerator LoadContentScene()
        {
            Managers.GameTime = gameTime;
            Managers.GameLevel = gameLevel;
            yield return EditorSceneManager.LoadSceneAsync(contentScenePath, LoadSceneMode.Additive);
            //EditorSceneManager.LoadScene(contentScenePath, LoadSceneMode.Additive);
        }

        void RecoveryScene()
        {
            EditorApplication.playModeStateChanged += Changed;
            void Changed(PlayModeStateChange state)
            {
                if (state == PlayModeStateChange.EnteredEditMode)
                {
                    EditorApplication.playModeStateChanged -= Changed;
                    EditorSceneManager.OpenScene(contentScenePath, OpenSceneMode.Single);                    
                }
            }
        }

    }
}
#endif