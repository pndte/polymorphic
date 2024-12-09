using System;
using Meta.Data;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Gameplay.Combat
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class KeyboardShooting : MonoBehaviour
    {
        private BaseBulletData _bulletData;
        private IBulletProvider _bulletProvider;
        private PlayerShootingConfig _config;
        private float _timeSinceLastShot;
        public UnityEvent<Vector2> Shooted;
        
        [Inject]
        private void Construct(IBulletProvider bulletProvider, PlayerShootingConfig config, BaseBulletData bulletData)
        {
            _config = config;
            _bulletProvider = bulletProvider;
            _bulletData = bulletData;
        }

        private void Update()
        {
            _timeSinceLastShot += Time.deltaTime;
            
            if (!Input.GetMouseButton(0)) return; // TODO: абстракции для input-сервисов
            
            if (_timeSinceLastShot < _config.Cooldown) return;
            
            var bullet = _bulletProvider.Get();
            var position = transform.position;
            bullet.transform.position = new Vector3(position.x, position.y, position.z);
            bullet.transform.rotation = transform.rotation;
            
            bullet.Launch(_bulletData, transform.up);
            Shooted?.Invoke(transform.up);
            
            _timeSinceLastShot = 0;
        }
    }
}