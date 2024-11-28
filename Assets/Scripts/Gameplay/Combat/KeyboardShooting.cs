using Meta.Data;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Gameplay.Combat
{
    public class KeyboardShooting : MonoBehaviour
    {
        private PlayerShootingConfig _config;
        private float _timeSinceLastShot;
        private IObjectPool<Bullet> _bulletPool;
        // event Shooted;  Zenject Events
        
        [Inject]
        private void Construct(PlayerShootingConfig config)
        {
            _config = config;
        }
        
        private void Update()
        {
            _timeSinceLastShot += Time.deltaTime;
            if (!Input.GetMouseButton(0)) return; // TODO: абстракции для input-сервисов
            if (_timeSinceLastShot < _config.Cooldown) return;
            // _bulletPool.
            var bullet = Instantiate(_config.BulletPrefab, transform.position, transform.rotation);
            bullet.Launch(transform.up);
            _timeSinceLastShot = 0;
        }
    }
}