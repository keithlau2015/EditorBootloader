#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace EditorBootloader
{
    internal class BootloaderEditor : EditorWindow
    {
        private static SceneAsset bootloaderScene;

        [MenuItem("Bootloader/BootloaderEditor")]
        public static void ShowWindow()
        {
            if (EditorBuildSettings.scenes.Length == 0)
            {
                Debug.Log($"<color=cyan>Auto Load Bootloader:</color> <color=orange>building setting scene are empty, please go to [File] -> [Build Setting] -> Add your initialize scene</color>");
                return;
            }

            var pathOfFirstScene = EditorBuildSettings.scenes[0].path;
            bootloaderScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);
            EditorWindow.GetWindow(typeof(BootloaderEditor), true, "Bootloader Editor");
        }

        private Vector2 scrollPosition = Vector2.zero;

        private void OnGUI()
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(position.width), GUILayout.Height(position.height));
            GUILayout.Space(10);
            GUILayout.Label("Bootloader Scene");

            bootloaderScene = (SceneAsset)EditorGUILayout.ObjectField(bootloaderScene, typeof(SceneAsset), true);
            SetEditorBuildSettingsScenes();
            GUILayout.Space(10);
            
            GUILayout.Space(10);
            if (GUILayout.Button("Close"))
            {
                this.Close();
            }

            GUILayout.Space(10);
            GUILayout.EndScrollView();
        }

        public void SetEditorBuildSettingsScenes()
        {
            // Find valid Scene paths and make a list of EditorBuildSettingsScene
            List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();
            editorBuildSettingsScenes.AddRange(EditorBuildSettings.scenes);
            string scenePath = AssetDatabase.GetAssetPath(bootloaderScene);
            if (!string.IsNullOrEmpty(scenePath))
            {
                editorBuildSettingsScenes[0] = new EditorBuildSettingsScene(scenePath, true);
            }

            // Set the Build Settings window Scene list
            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
        }
    }

    [InitializeOnLoad]
    public class EditorBootloader
    {
        private static int BOOTLOADER_SCENE_INDEX = 0;

        static EditorBootloader()
        {
            if (EditorBuildSettings.scenes.Length == 0)
            {
                Debug.Log($"<color=cyan>Auto Load Bootloader:</color> <color=orange>building setting scene are empty, please go to [File] -> [Build Setting] -> Add your initialize scene</color>");
                return;
            }
            
            var enableAutoBootloader = EditorPrefs.GetBool("EnableAutoBootloader", true);
            if (enableAutoBootloader)
            {
                var pathOfFirstScene = EditorBuildSettings.scenes[BOOTLOADER_SCENE_INDEX].path;
                var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);
                EditorSceneManager.playModeStartScene = sceneAsset;
            }
            else
            {
                EditorSceneManager.playModeStartScene = null;
            }
        }

        [MenuItem("Bootloader/Enable Auto Load Bootloader")]
        public static void EnableAutoLoadBootloader()
        {
            EditorPrefs.SetBool("EnableAutoBootloader", true);
            DebugLogAutoLoadBootloader();
            CompilationPipeline.RequestScriptCompilation();
        }

        [MenuItem("Bootloader/Disable Auto Load Bootloader")]
        public static void DisableAutoLoadBootloader()
        {
            EditorPrefs.SetBool("EnableAutoBootloader", false);
            DebugLogAutoLoadBootloader();
            CompilationPipeline.RequestScriptCompilation();
        }

        [MenuItem("Bootloader/Debug Log Auto Load Bootloader")]
        public static void DebugLogAutoLoadBootloader()
        {
            if (EditorBuildSettings.scenes.Length == 0)
            {
                Debug.Log($"<color=cyan>Auto Load Bootloader:</color> <color=orange>building setting scene are empty, please go to [File] -> [Build Setting] -> Add your initialize scene</color>");
                return;
            }

            var enableAutoBootloader = EditorPrefs.GetBool("EnableAutoBootloader", true);
            if (enableAutoBootloader)
            {
                var pathOfFirstScene = EditorBuildSettings.scenes[0].path;
                var bootloaderScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);
                Debug.Log($"<color=cyan>Auto Load Bootloader:</color> <color=lime>{enableAutoBootloader}</color>, <color=cyan>Bootloader scene:</color> <color=orange>{bootloaderScene.name}</color>");
            }
            else
            {
                var pathOfFirstScene = EditorBuildSettings.scenes[0].path;
                var bootloaderScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);
                Debug.Log($"<color=cyan>Auto Load Bootloader:</color> <color=red>{enableAutoBootloader}</color> <color=cyan>Bootloader scene:</color> <color=orange>{bootloaderScene.name}</color>");
            }
        }
    }
}
#endif
