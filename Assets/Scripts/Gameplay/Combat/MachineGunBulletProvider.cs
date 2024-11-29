using Infrastructure.Services;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Gameplay.Combat
{
    public class MachineGunBulletProvider : MonoBehaviour, IBulletProvider
    {
        [SerializeField] private MachineGunBullet _bulletPrefab;
        [SerializeField] private Transform _bulletParent;
        [SerializeField] private int _poolSize;
        [SerializeField] private int _poolMaxSize;
        
        private IFactory<Bullet> _factory;
        private IObjectPool<Bullet> _objectPool;

        [Inject]
        private void Construct(MachineGunBullet.Factory factory)
        {
            _factory = factory;
        }

        private void Awake()
        {
            _objectPool =
                new AutomatedObjectPool<Bullet>(_factory, _bulletParent, _poolSize, _poolMaxSize,
                    bullet => bullet.gameObject.SetActive(true), bullet => bullet.gameObject.SetActive(false));
        }

        public Bullet Get() => _objectPool.Get();
    }
}