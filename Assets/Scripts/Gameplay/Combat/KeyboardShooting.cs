using Meta.Data;
using UnityEngine;
using Zenject;

namespace Gameplay.Combat
{
    public class KeyboardShooting : MonoBehaviour
    {
        private PlayerShootingConfig _config;
        // Zenject Events
        
        [Inject]
        private void Construct(PlayerShootingConfig config)
        {
            _config = config;
        }
        
        private void Update()
        {
            if (!Input.GetMouseButton(0)) return;
            
            var bullet = Instantiate(_config.BulletPrefab, transform.position, transform.rotation);
            bullet.Launch(transform.up);
        }
    }
}