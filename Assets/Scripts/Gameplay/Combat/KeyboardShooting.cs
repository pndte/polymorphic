using Meta.Data;
using UnityEngine;
using Zenject;

namespace Gameplay.Combat
{
    public class KeyboardShooting : MonoBehaviour
    {
        private PlayerShootingConfig _config;
        private float _timeSinceLastShot;
        private IBulletProvider _bulletProvider;
        // event Shooted;  Zenject Events
        
        [Inject(Id = "MachineGun")]
        private void Construct(PlayerShootingConfig config, IBulletProvider bulletProvider)
        {
            _config = config;
            _bulletProvider = bulletProvider;
        }
        
        private void Update()
        {
            _timeSinceLastShot += Time.deltaTime;
            
            if (!Input.GetMouseButton(0)) return; // TODO: абстракции для input-сервисов
            
            if (_timeSinceLastShot < _config.Cooldown) return;
            
            var bullet = _bulletProvider.Get();
            bullet.Launch(transform.up);
            
            _timeSinceLastShot = 0;
        }
    }
}