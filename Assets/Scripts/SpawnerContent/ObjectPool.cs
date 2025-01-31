using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpawnerContent
{
    public class ObjectPool<T> where T : MonoBehaviour
    {
        private Transform _container;
        private List<T> _poolGeneric;

        public ObjectPool(T prefab, int count, Transform container)
        {
            _container = container;
            Initialize(count, prefab);
        }

        public bool AutoExpand { get; private set; }

        public bool TryGetObject(out T spawned, T prefabs)
        {
            var filter = _poolGeneric.Where(p => p.gameObject.activeSelf == false);
            int index = Random.Range(0, filter.Count());

            if (filter.Count() == 0)
            {
                if (AutoExpand)
                {
                    spawned = CreateObject(prefabs);
                    return spawned != null;
                }
                else
                {
                    spawned = default;
                    return false;
                }
            }

            spawned = filter.ElementAt(index);
            return spawned != null;
        }

        public void AddObject(T obj)
        {
            _poolGeneric.Add(obj);
        }

        public T GetFirstObject()
        {
            T filter = _poolGeneric.FirstOrDefault(p => p.gameObject.activeSelf == false);
            return filter;
        }

        public void EnableAutoExpand()
        {
            AutoExpand = true;
        }

        public void DisableAutoExpand()
        {
            AutoExpand = false;
        }

        public void Reset()
        {
            foreach (var item in _poolGeneric)
                item.gameObject.SetActive(false);
        }

        private void Initialize(int count, T prefabs)
        {
            _poolGeneric = new List<T>();

            for (int i = 0; i < count; i++)
            {
                var spawned = Object.Instantiate(prefabs, _container.transform);
                spawned.gameObject.SetActive(false);
                _poolGeneric.Add(spawned);
            }
        }

        private T CreateObject(T prefabs)
        {
            var spawned = Object.Instantiate(prefabs, _container.transform);
            spawned.gameObject.SetActive(true);
            _poolGeneric.Add(spawned);
            return spawned;
        }
    }
}