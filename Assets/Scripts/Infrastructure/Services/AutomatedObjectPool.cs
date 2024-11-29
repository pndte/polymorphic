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
    /// </summary>
    public class AutomatedObjectPool<T> : IObjectPool<T> where T : MonoBehaviour, IResettable<T>
    {
        private readonly Transform _prefabsParent;
        private readonly IObjectPool<T> _objectPool;
        private readonly IFactory<T> _factory;

        private readonly Action<T> _onGet;
        private readonly Action<T> _onRelease;

        public AutomatedObjectPool(IFactory<T> factory, Transform prefabsParent, int size, int maxSize)
        {
            _factory = factory;
            _prefabsParent = prefabsParent;
            _objectPool = new ObjectPool<T>(CreateObject, actionOnDestroy: OnDestroyObject, defaultCapacity: size,
                maxSize: maxSize);
            // FillPool(size);
        }

        public AutomatedObjectPool(IFactory<T> factory, Transform prefabsParent, int size, int maxSize,
            Action<T> onGet, Action<T> onRelease) :
            this(factory, prefabsParent, size, maxSize)
        {
            _onGet += onGet;
            _onRelease += onRelease;
        }

        private void FillPool(int objCount)
        {
            for (int i = 0; i < objCount; i++)
            {
                var obj = CreateObject();
                obj.Reset();
            }
        }

        private T CreateObject()
        {
            var obj = _factory.Create();
            obj.transform.parent = _prefabsParent;
            obj.OnReset += OnReset;

            return obj;
        }

        private void OnDestroyObject(T obj)
        {
            obj.OnReset -= OnReset;
        }

        public T Get()
        {
            var obj = _objectPool.Get();
            _onGet?.Invoke(obj);

            return obj;
        }

        public PooledObject<T> Get(out T v)
        {
            var pooledObj = _objectPool.Get(out v);
            _onGet?.Invoke(v); // TODO: it's ok?

            return pooledObj;
        }

        public void Release(T element) // TODO: профайлить, смотреть, что по затратам памяти.
        {
            _onRelease?.Invoke(element);
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