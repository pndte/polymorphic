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
        private readonly Transform _weaponUser;
        
        public MachineGun(IBulletProvider bulletProvider, Transform weaponUser, BaseWeaponConfig config, BaseBulletData bulletData)
        {
            _bulletData = bulletData;
            _bulletProvider = bulletProvider;
            _config = config;
            _weaponUser = weaponUser;
            Reloaded = true;
        }
        
        public IBullet Shoot(Vector2 direction)
        {
            var bullet = _bulletProvider.Get(_bulletData);
            
            bullet.Launch(_weaponUser.position, _weaponUser.up);

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