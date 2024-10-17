using Fusion;
using Fusion.Editor;
using UnityEditor;
using UnityEngine;

namespace Shakhtarsk.Network.Editor
{
    [CustomEditor(typeof(NetworkManager))]
    public class NetworkManagerEditor : UnityEditor.Editor
    {
        private SerializedProperty isDebugProperty;
        private SerializedProperty playerCountProperty;
        private SerializedProperty networkPrefab;
        private SerializedProperty sceneReference;

        private NetworkProjectConfig _config;

        private void OnEnable()
        {
            // Инициализация SerializedObject и SerializedProperty
            isDebugProperty = serializedObject.FindProperty("isDebug");
            networkPrefab = serializedObject.FindProperty("runnerPrefab");
            sceneReference = serializedObject.FindProperty("startScene");
            playerCountProperty = serializedObject.FindProperty("additionalPlayerCount");

            var configPath = FusionGlobalScriptableObjectUtils.GetGlobalAssetPath<NetworkProjectConfigAsset>();
            _config = NetworkProjectConfigImporter.LoadConfigFromFile(configPath);
        }
        
        public override void OnInspectorGUI()
        {
            // Обновляем объект перед отображением
            serializedObject.Update();
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.PropertyField(networkPrefab, new GUIContent("Runner prefab", "Runner to instantiate"));
            EditorGUILayout.PropertyField(sceneReference, new GUIContent("Start scene"));
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
            
            isDebugProperty.boolValue = _config.PeerMode == NetworkProjectConfig.PeerModes.Multiple;
            
            // Если IsDebug включен, отображаем дополнительные настройки
            if (isDebugProperty.boolValue)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Debug Settings", EditorStyles.boldLabel);

                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.PropertyField(playerCountProperty, new GUIContent("Additional Player Count", "Number of players in the game"));
                EditorGUILayout.EndVertical();
            }

            // Применяем изменения к объекту
            serializedObject.ApplyModifiedProperties();
        }
    }
}