using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using UnityEngine.Pool;

namespace Infrastructure.Services
{
    /// <summary>
    /// Unity.API object pool decorator.
    /// T should override the GetHashCode() method.
    /// </summary>
    public class AutomatedObjectPool<T>: IObjectPool<T> where T : MonoBehaviour, IResettable<T>
    {
        private readonly IObjectPool<T> _objectPool;
        private readonly ISet<T> _objectsSubscribed = new HashSet<T>();
        
        public AutomatedObjectPool(IObjectPool<T> objectPool)
        {
            _objectPool = objectPool;
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