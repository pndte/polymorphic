using Meta.Data;
using UnityEngine;
using Zenject;

namespace Gameplay.Combat
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class KeyboardShooting : MonoBehaviour
    {
        [SerializeField] private BaseBulletConfig _bulletConfig;
        
        [Inject(Id = "MachineGun")] private IBulletProvider _bulletProvider;
        private PlayerShootingConfig _config;
        private float _timeSinceLastShot;
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
            
            var bullet = _bulletProvider.Get();
            var position = transform.position;
            bullet.transform.position = new Vector3(position.x, position.y, position.z);
            bullet.transform.rotation = transform.rotation;
            
            bullet.Launch(_bulletConfig, transform.up);
            
            _timeSinceLastShot = 0;
        }
    }
}