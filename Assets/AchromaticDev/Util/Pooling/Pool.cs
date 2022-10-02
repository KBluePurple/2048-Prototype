using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AchromaticDev.Util.Pooling
{
    [UnityEngine.CreateAssetMenu(fileName = "New Pool Settings", menuName = "AchromaticDev/Pooling", order = 0)]
    public class Pool : ScriptableObject
    {
        public GameObject prefab = null;
        public int initialPoolSize = 0;
        
        private readonly Queue<PoolObject> _pool = new Queue<PoolObject>();
        private GameObject _poolParent = null;
        
        public int PoolSize => _pool.Count;
        
        public void Initialize(Transform parent)
        {
            _poolParent = new GameObject(prefab.name + " Pool");
            _poolParent.transform.parent = parent;
            
            for (int i = 0; i < initialPoolSize; i++)
            {
                GameObject obj = Instantiate(prefab, _poolParent.transform);
                var poolObject = obj.AddComponent<PoolObject>();
                poolObject.pool = this;
                obj.SetActive(false);
                _pool.Enqueue(poolObject);
            }

            SceneManager.sceneUnloaded += SceneUnloaded;
        }
        
        public GameObject GetObject(GameObject poolPrefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            GameObject obj;
            if (_pool.Count > 0)
            {
                obj = _pool.Dequeue().gameObject;
            }
            else
            {
                obj = Instantiate(poolPrefab, position, rotation, parent);
                obj.name = $"PoolObject ({poolPrefab.name})";
                var poolObject = obj.GetComponent<PoolObject>();
                
                if (poolObject == null)
                    poolObject = obj.AddComponent<PoolObject>();
                
                PoolManager.Instance.PoolObjectCache[obj] = poolObject;
                poolObject.pool = this;
            }
            obj.SetActive(true);
            PoolManager.Instance.PoolObjectCache[obj].onSpawn?.Invoke();
            
            return obj;
        }
        
        public void ReturnObject(GameObject obj)
        {
            obj.SetActive(false);
            if (PoolManager.Instance.PoolObjectCache.TryGetValue(obj, out var poolObject))
            {
                _pool.Enqueue(poolObject);
                poolObject.onDespawn?.Invoke();
            }
        }
        
        private void SceneUnloaded(Scene scene)
        {
            foreach (var poolObject in _pool)
            {
                PoolManager.Instance.PoolObjectCache.Remove(poolObject.gameObject);
                Destroy(poolObject.gameObject);
            }
            _pool.Clear();
        }
    }
}