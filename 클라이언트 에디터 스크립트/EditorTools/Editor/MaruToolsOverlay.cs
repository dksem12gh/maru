using DigitalMaru.Common;
using System;
using System.Collections.Generic;
using System.IO;
using TouchScript.Behaviors.Cursors;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


namespace DigitalMaru.Editor
{

    [Overlay(typeof(SceneView), "Maru Editor Tools")]
    public class MaruToolsOverlay : Overlay
    {
        VisualElement m_root;
        VisualElement m_playingRoot;
        VisualElement m_editorRoot;
        bool IsEnable => !EditorApplication.isPlaying && CheckContentScene;

        bool RuntimeEnable => EditorApplication.isPlaying;

        bool CheckContentScene
        {
            get
            {
                var scene = GetCurrentSceneName();
                return GetContent().Contains(scene);
            }
        }
        public override void OnCreated()
        {
            base.OnCreated();
            EditorApplication.playModeStateChanged += OnPlayStateChanged;
            EditorSceneManager.sceneOpened += OnSceneOpened;
            m_root = new VisualElement();
            m_editorRoot = new VisualElement();
            m_playingRoot = new VisualElement();
            m_root.Add(m_playingRoot);
            m_root.Add(m_editorRoot);
            m_editorRoot.SetEnabled(IsEnable);
            m_playingRoot.SetEnabled(RuntimeEnable);

        }

        public override void OnWillBeDestroyed()
        {
            base.OnWillBeDestroyed();
            EditorApplication.playModeStateChanged -= OnPlayStateChanged;
            EditorSceneManager.sceneOpened -= OnSceneOpened;
        }


        void OnPlayStateChanged(PlayModeStateChange mode)
        {
            m_editorRoot.SetEnabled(IsEnable);
            m_playingRoot.SetEnabled(RuntimeEnable);
         //   m_root.SetEnabled(IsEnable);
        }

        void OnSceneOpened(Scene scene, OpenSceneMode mode)
        {
            m_editorRoot.SetEnabled(IsEnable);
            m_playingRoot.SetEnabled(RuntimeEnable);
        //    m_root.SetEnabled(IsEnable);
        }

        public override VisualElement CreatePanelContent()
        {         
            var root = new VisualElement();
            m_editorRoot = CreateEditorElement();
            m_playingRoot = CreatePlayingElement();

            root.Add(m_playingRoot);
            root.Add(m_editorRoot);

            m_root = root;
            return m_root;
        }

        VisualElement CreatePlayingElement()
        {
            var root = new VisualElement();
            root.Add(PauseButton());
            return root;
        }

        VisualElement CreateEditorElement()
        {
            var root = new VisualElement();
            root.Add(AddCursorButton());
       

            root.Add(AddGameCountInputField());
            root.Add(AddGameLevelInputField());

            var runtimeRoot = new VisualElement();
            runtimeRoot.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            runtimeRoot.Add(MainButton());
            runtimeRoot.Add(PlayButton());

            root.Add(runtimeRoot);
            return root;
        }

        Button PauseButton()
        {
            var btn = new Button();
            btn.text = "Pause";
            btn.clicked += () =>
            {
                if (Application.isPlaying is false) return;
                var mgr = GameObject.FindObjectOfType<MultiDisplayTouchGameManager>();
                if (mgr == null) return;

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
            };
            return btn;
        }

        Button MainButton()
        {
            var btn = new Button();
            btn.style.flexGrow = 1;
            btn.text = "Main";
            btn.clicked += () =>
            {
                if (EditorSceneManager.GetActiveScene().isDirty)
                {
                    if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                        return;
                }                
                EditorSceneManager.OpenScene(EditorBuildSettings.scenes[0].path);
                EditorApplication.EnterPlaymode();
                AddCursors();
            };
            return btn;
        }

        Button PlayButton()
        {
            var btn = new Button();
            btn.style.flexGrow = 1;
            btn.text = "Content";
            btn.clicked += OnPlayBtn;
            return btn;
        }

        Button AddCursorButton()
        {
            var btn = new Button();
            btn.text = "Cursors 추가";
            btn.clicked += AddCursors;

            return btn;
        }

        VisualElement AddGameCountInputField()
        {
            var element = new IntegerField();
            element.label = "Goal";
            element.value = GoalValue;
            element.RegisterValueChangedCallback(evt => {
                GoalValue = Math.Clamp(evt.newValue, -1, 100);                
            });
            return element;
        }

        int GoalValue
        {
            get => EditorPrefs.GetInt("edit_goal_value", 1);
            set => EditorPrefs.SetInt("edit_goal_value", value);
        }

        VisualElement AddGameLevelInputField()
        {
            var element = new IntegerField();
            element.label = "Level";
            element.value = LevelValue;
            element.RegisterValueChangedCallback(evt => {
                LevelValue = Math.Clamp(evt.newValue, -1, 100);
            });
            return element;
        }

        int LevelValue
        {
            get => EditorPrefs.GetInt("edit_level_value", 1);
            set => EditorPrefs.SetInt("edit_level_value", value);
        }

        string GetCurrentSceneName() 
        {
            return EditorSceneManager.GetActiveScene().name;
        }
        string GetCurrentScenePath()
        {
            return EditorSceneManager.GetActiveScene().path;
        }

        void OnPlayBtn()
        {
            if (EditorSceneManager.GetActiveScene().isDirty)
            {
                if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    return;
            }
            LoadEditorScene();            
        }



        void LoadEditorScene()
        {
            var scenePath = GetCurrentScenePath();
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            scene.name = "Test Root";
            var editorScene = new GameObject().AddComponent<MaruEditorScene>();
            editorScene.name = "Maru Editor Scene - For Editor. Don't Save In Build";
            editorScene.hideFlags = HideFlags.DontSaveInBuild;
            editorScene.Setup(scenePath, GoalValue,LevelValue);

            EditorApplication.EnterPlaymode();
        }

        List<string> GetContent()
        {
            List<string> content = new List<string>();
            foreach(var scene in EditorBuildSettings.scenes)
            {
                var sceneName = Path.GetFileNameWithoutExtension(scene.path);
                content.Add(sceneName);
            }
            content.Remove("main");
            return content;
        }


        void AddCursors()
        {
            var cursor = GameObject.FindObjectOfType<CursorManager>();
            if (cursor == null)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/TouchScript/Prefabs/Cursors.prefab");
                var go = GameObject.Instantiate(prefab);
                go.GetComponent<Canvas>().targetDisplay = 1;
                cursor = go.GetComponent<CursorManager>();
                go.hideFlags = HideFlags.DontSave;
                
            }
            Selection.activeGameObject = cursor.gameObject;
        }
    }
}
