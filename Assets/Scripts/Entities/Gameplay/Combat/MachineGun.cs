using PEntities.Meta.Data;
using UnityEngine;
using Zenject;

namespace PEntities.Gameplay.Combat
{
    public class MachineGun: IWeapon, ITickable
    {
        private readonly BaseBulletData _bulletData;
        private readonly IBulletProvider _bulletProvider;
        private readonly BaseWeaponConfig _config;
        private readonly Transform _bulletSpawn;
        
        private float _timeSinceLastShot;
        
        public MachineGun(IBulletProvider bulletProvider, Transform bulletSpawn, BaseWeaponConfig config, BaseBulletData bulletData)
        {
            _bulletData = bulletData;
            _bulletProvider = bulletProvider;
            _config = config;
            _bulletSpawn = bulletSpawn;
        }
        
        public Bullet Shoot(Vector2 direction)
        {
            var bullet = _bulletProvider.Get();
            var position = _bulletSpawn.position;
            
            bullet.transform.position = new Vector3(position.x, position.y, position.z);
            bullet.transform.rotation = _bulletSpawn.rotation;
            
            bullet.Launch(_bulletData, _bulletSpawn.up);
            
            _timeSinceLastShot = 0;

            return bullet;
        }

        public bool IsReadyToShoot 
        {
            get
            {
                if (_timeSinceLastShot < _config.Cooldown) return false;

                return true;
            }
        }

        public void Tick()
        {
            _timeSinceLastShot += Time.deltaTime;
        }
    }
}