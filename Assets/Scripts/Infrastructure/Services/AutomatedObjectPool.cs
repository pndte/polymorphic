using System;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Infrastructure.Services
{
    /// <summary>
    /// Unity.API object pool decorator.
    /// T should override the GetHashCode() method.
    /// </summary>
    public class AutomatedObjectPool<T>: IObjectPool<T> where T : MonoBehaviour, IResettable<T>
    {
        private readonly MonoBehaviour _prefab;
        private readonly Transform _prefabsParent;
        private readonly IObjectPool<T> _objectPool;
        private readonly ISet<T> _objectsSubscribed = new HashSet<T>();
        private readonly IInstantiator _instantiator;

        public AutomatedObjectPool(MonoBehaviour prefab, Transform prefabsParent, int size, int maxSize, IInstantiator instantiator)
        {
            _prefab = prefab;
            _instantiator = instantiator;
            _prefabsParent = prefabsParent;
            _objectPool = new ObjectPool<T>(CreateObject, defaultCapacity: size, maxSize: maxSize);
        }

        private T CreateObject()
        {
            return _instantiator.InstantiatePrefabForComponent<T>(_prefab, _prefabsParent);
        }
        
        public T Get() => _objectPool.Get();

        public PooledObject<T> Get(out T v) => _objectPool.Get(out v);

        public void Release(T element) // TODO: профайлить, смотреть, что по затратам памяти.
        {
            if (!_objectsSubscribed.Contains(element))
            {
                element.OnReset += OnReset;
                _objectsSubscribed.Add(element);
            }

            _objectPool.Release(element);
        }

        private void OnReset(T obj)
        {
            Release(obj);
        }

        public void Clear() => _objectPool.Clear();

        public int CountInactive => _objectPool.CountInactive;
    }
}