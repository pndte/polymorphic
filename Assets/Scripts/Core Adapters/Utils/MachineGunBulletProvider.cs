using PCoreAdapters.Gameplay;
using PEntities.Gameplay.Combat;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace PCoreAdapters.Utils
{
    public class MachineGunBulletProvider : MonoBehaviour, IBulletProvider
    {
        [SerializeField] private MonoBullet _bulletPrefab;
        [SerializeField] private Transform _bulletParent;
        [SerializeField] private int _poolSize;
        [SerializeField] private int _poolMaxSize; // TODO: Zenject
        
        private IFactory<MonoBullet> _factory;
        private IObjectPool<MonoBullet> _objectPool;

        [Inject]
        private void Construct(MonoBullet.Factory factory)
        {
            _factory = factory;
        }

        private void Awake()
        {
            var automatedObjectPool =
                new AutomatedObjectPool<MonoBullet>(_factory, _bulletParent, _poolSize, _poolMaxSize,
                    bullet => bullet.gameObject.SetActive(true), bullet => bullet.gameObject.SetActive(false));
            automatedObjectPool.Fill(_poolSize); // TODO: передать конфигом данные и закинуть этот код в инфраструктуру.
            
            _objectPool = automatedObjectPool;
        }

        public IBullet Get() => _objectPool.Get();
    }
}