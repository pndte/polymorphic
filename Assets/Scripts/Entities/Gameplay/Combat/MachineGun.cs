using System;
using Cysharp.Threading.Tasks;
using PEntities.Meta.Data;
using UnityEngine;

namespace PEntities.Gameplay.Combat
{
    public class MachineGun: IWeapon
    {
        private readonly BaseBulletData _bulletData;
        private readonly IBulletProvider _bulletProvider;
        private readonly BaseWeaponConfig _config;
        private readonly Transform _bulletSpawn;
        
        public MachineGun(IBulletProvider bulletProvider, Transform bulletSpawn, BaseWeaponConfig config, BaseBulletData bulletData)
        {
            _bulletData = bulletData;
            _bulletProvider = bulletProvider;
            _config = config;
            _bulletSpawn = bulletSpawn;
            Reloaded = true;
        }
        
        public Bullet Shoot(Vector2 direction)
        {
            var bullet = _bulletProvider.Get();
            var position = _bulletSpawn.position;
            
            bullet.transform.position = new Vector3(position.x, position.y, position.z);
            bullet.transform.rotation = _bulletSpawn.rotation;
            
            bullet.Launch(_bulletData, _bulletSpawn.up);

            Reloaded = false;

            return bullet;
        }

        public async UniTask ReloadAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_config.Cooldown));
            
            Reload();
        }

        public void Reload()
        {
            Reloaded = true;
        }

        public bool Reloaded { get; private set; }
    }
}