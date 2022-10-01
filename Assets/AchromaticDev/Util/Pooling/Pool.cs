using System.Collections.Generic;
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
        
        private void SceneUnloaded(Scene scene)
        {
            _pool.Clear();
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
                var poolObject = obj.AddComponent<PoolObject>();
                poolObject.pool = this;
            }
            obj.SetActive(true);
            
            return obj;
        }
        
        public void ReturnObject(GameObject obj)
        {
            obj.SetActive(false);
            _pool.Enqueue(obj.GetComponent<PoolObject>());
        }
    }
}