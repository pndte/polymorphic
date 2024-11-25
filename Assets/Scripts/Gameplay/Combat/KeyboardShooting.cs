using Meta.Data;
using UnityEngine;
using Zenject;

namespace Gameplay.Combat
{
    public class KeyboardShooting : MonoBehaviour
    {
        private PlayerShootingConfig _config;
        private float _timeSinceLastShot;
        // Zenject Events
        
        [Inject]
        private void Construct(PlayerShootingConfig config)
        {
            _config = config;
        }
        
        private void Update()
        {
            _timeSinceLastShot += Time.deltaTime;
            if (!Input.GetMouseButton(0)) return;
            if (_timeSinceLastShot < _config.Cooldown) return;
            
            var bullet = Instantiate(_config.BulletPrefab, transform.position, transform.rotation);
            bullet.Launch(transform.up);
            _timeSinceLastShot = 0;
        }
    }
}