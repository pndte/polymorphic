using Infrastructure.Services;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Gameplay.Combat
{
    public class MachineGunBulletProvider : MonoInstaller, IBulletProvider
    {
        [SerializeField] private MachineGunBullet _bulletPrefab;
        [SerializeField] private Transform _bulletParent;
        [SerializeField] private int _poolSize;
        [SerializeField] private int _poolMaxSize;

        private IObjectPool<Bullet> _objectPool;

        private void Awake()
        {
            _objectPool =
                new AutomatedObjectPool<Bullet>(_bulletPrefab, _bulletParent, _poolSize, _poolMaxSize, Container);
        }

        public override void InstallBindings()
        {
            Container.Bind<IBulletProvider>().WithId("MachineGun").FromInstance(this).AsSingle();
        }

        public Bullet Get() => _objectPool.Get();
    }
}