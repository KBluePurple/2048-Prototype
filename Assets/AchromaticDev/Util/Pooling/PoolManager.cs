using System.Collections.Generic;
using UnityEngine;

namespace AchromaticDev.Util.Pooling
{
    public class PoolManager : MonoSingleton<PoolManager>
    {
        internal readonly Dictionary<GameObject, Pool> PrefabDict = new Dictionary<GameObject, Pool>();

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            if (_instance == null)
            {
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
        }
        
        public static GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (_instance.PrefabDict.ContainsKey(prefab))
            {
                return _instance.PrefabDict[prefab].GetObject(prefab, position, rotation, parent);
            }
            else
            {
                return Object.Instantiate(prefab, position, rotation, parent);
            }
        }
        
        public static GameObject Instantiate(GameObject prefab, Transform parent = null)
        {
            return Instantiate(prefab, Vector3.zero, Quaternion.identity, parent);
        }

        public static void Destroy(GameObject gameObject)
        {
            var prefab = gameObject.GetComponent<PoolObject>().pool.prefab;
            
            if (_instance.PrefabDict.ContainsKey(prefab))
            {
                _instance.PrefabDict[prefab].ReturnObject(gameObject);
            }
            else
            {
                Object.Destroy(gameObject);
            }
        }
    }
}