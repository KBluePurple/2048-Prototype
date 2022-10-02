using System.Collections.Generic;
using UnityEngine;

namespace AchromaticDev.Util.Pooling
{
    public class PoolManager : MonoSingleton<PoolManager>
    {
        internal readonly Dictionary<GameObject, Pool> PrefabDict = new Dictionary<GameObject, Pool>();
        internal readonly Dictionary<GameObject, PoolObject> PoolObjectCache = new Dictionary<GameObject, PoolObject>();

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            if (_instance != null) return;
            
            var go = new GameObject("PoolManager");
            _instance = go.AddComponent<PoolManager>();
            DontDestroyOnLoad(go);

            var pools = Resources.LoadAll<Pool>("Pools");
                
            foreach (var poolSetting in pools)
            {
                poolSetting.Initialize(_instance.transform);
                _instance.PrefabDict.Add(poolSetting.prefab, poolSetting);
            }
        }
        
        public static GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (_instance.PrefabDict.ContainsKey(prefab))
            {
                return _instance.PrefabDict[prefab].GetObject(prefab, position, rotation, parent);
            }

            _instance.PrefabDict.Add(prefab, ScriptableObject.CreateInstance<Pool>());
            _instance.PrefabDict[prefab].prefab = prefab;
            _instance.PrefabDict[prefab].Initialize(_instance.transform);
            return _instance.PrefabDict[prefab].GetObject(prefab, position, rotation, parent);
        }
        
        public static GameObject Instantiate(GameObject prefab, Transform parent = null)
        {
            return Instantiate(prefab, Vector3.zero, Quaternion.identity, parent);
        }

        public static void Destroy(GameObject gameObject)
        {
            var prefab = _instance.PoolObjectCache[gameObject].pool.prefab;
            
            if (_instance.PrefabDict.ContainsKey(prefab))
            {
                _instance.PrefabDict[prefab].ReturnObject(gameObject);
            }
            else
            {
                Debug.LogWarning($"PoolManager: {prefab.name} is not in the pool dictionary.");
                Object.Destroy(gameObject);
            }
        }
    }
}