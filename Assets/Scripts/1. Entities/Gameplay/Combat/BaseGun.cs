using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PEntities.Meta.Data;
using UnityEngine;

namespace PEntities.Gameplay.Combat
{
    public class BaseGun: IWeapon
    {
        private readonly BaseBulletData _bulletData;
        private readonly IBulletProvider _bulletProvider;
        private readonly BaseWeaponConfig _config;
        private readonly Transform _weaponUser;
        
        public BaseGun(IBulletProvider bulletProvider, Transform weaponUser,
            BaseWeaponConfig config, BaseBulletData bulletData, bool isReloaded = true)
        {
            _bulletData = bulletData;
            _bulletProvider = bulletProvider;
            _config = config;
            _weaponUser = weaponUser;
            Reloaded = isReloaded;
        }
        
        public IBullet Shoot(Vector2 direction)
        {
            var bullet = _bulletProvider.Get(_bulletData);
            
            bullet.Launch(_weaponUser.position, _weaponUser.up);

            Reloaded = false;

            return bullet;
        }

        public async UniTask ReloadAsync(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_config.Cooldown), cancellationToken: token);
            
            Reload();
        }

        public void Reload()
        {
            Reloaded = true;
        }

        public bool Reloaded { get; private set; }
    }
}