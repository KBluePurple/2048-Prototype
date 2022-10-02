using UnityEngine;
using UnityEditor;

namespace AchromaticDev.Util.Pooling
{
    [CustomEditor(typeof(PoolManager))]
    [CanEditMultipleObjects]
    public class PoolManagerEditor : Editor
    {
        SerializedProperty poolList;
        
        public override void OnInspectorGUI()
        {
            PoolManager poolManager = (PoolManager)target;
            serializedObject.Update();
            
            EditorGUILayout.LabelField("Pool List", EditorStyles.boldLabel);

            if (poolManager is not null)
            {
                foreach (var pool in poolManager.PrefabDict)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(pool.Key.name);
                    EditorGUILayout.LabelField(pool.Value.PoolSize.ToString());
                    EditorGUILayout.EndHorizontal();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}