using System;
using PEntities.Gameplay;
using R3;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace PCoreAdapters.Utils
{
    /// <summary>
    /// Unity.API object pool decorator.
    /// </summary>
    public class AutomatedObjectPool<T> : IDisposable, IObjectPool<T> where T : MonoBehaviour, IResettable<T>
    {
        private readonly Transform _prefabsParent;
        private readonly IObjectPool<T> _objectPool;
        private readonly IFactory<T> _factory;

        private readonly Action<T> _onGet;
        private readonly Action<T> _onRelease;
        private readonly Action<T> _onCreate;
        private readonly Action<T> _onDestroy;
        private readonly Action<T> _onReset;

        private readonly CompositeDisposable _disposables;

        public AutomatedObjectPool(IFactory<T> factory, Transform prefabsParent, int size, int maxSize,
            Action<T> onGet = null, Action<T> onRelease = null, Action<T> onCreate = null, Action<T> onDestroy = null, Action<T> onReset = null)
        {
            _onGet = onGet;
            _onRelease = onRelease;
            _onCreate = onCreate;
            _onDestroy = onDestroy;
            _onReset = onReset;
            _factory = factory;
            _prefabsParent = prefabsParent;
            _disposables = new CompositeDisposable();
            _objectPool = new ObjectPool<T>(CreateObject, actionOnDestroy: OnDestroyObject, defaultCapacity: size,
                maxSize: maxSize);
        }

        public void Fill(int objCount)
        {
            for (int i = 0; i < objCount; i++)
            {
                var obj = CreateObject();
                obj.Reset.Execute(obj);
            }
        }

        private T CreateObject()
        {
            var obj = _factory.Create();
            obj.transform.parent = _prefabsParent;
            obj.Reset
                .Subscribe(OnReset)
                .AddTo(_disposables);
            
            _onCreate?.Invoke(obj);

            return obj;
        }

        private void OnDestroyObject(T obj)
        {
            _onDestroy?.Invoke(obj);
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
            _onReset?.Invoke(obj);
            Release(obj);
        }

        public void Clear() => _objectPool.Clear();

        public int CountInactive => _objectPool.CountInactive;
        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}