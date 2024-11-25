using Meta.Data;
using R3;
using UnityEngine;
using Zenject;

namespace Gameplay.Combat
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MachineGunBullet : Bullet
    {
        private Vector2 _cachedDirection;
        private bool _launched;
        private Rigidbody2D _physics;
        private float _liveTime;
        
        [Inject]
        private void Construct(MachineGunBulletConfig config)
        {
            Damage.Value = 1;
            Speed.Value = 10; // TODO: чинить, не работает
            _liveTime = 5;
        }

        private void Awake()
        {
            _physics = GetComponent<Rigidbody2D>();
            Damage.Value = 1;
            Speed.Value = 10;
            _liveTime = 5;
        }

        private void FixedUpdate()
        {
            if (!_launched) return;
            
            Move(_cachedDirection);
        }

        public override void Move(Vector2 direction)
        {
            print("SPeed " + Speed.Value);
            print("Direction " + direction);
            _physics.velocity = direction * Speed.Value;
        }
        
        public override void Launch(Vector2 direction)
        {
            _cachedDirection = direction;
            _launched = true;
            Invoke(nameof(Die), _liveTime);
        }
        
        public void Die() => Destroy(gameObject);

        public override ReactiveProperty<float> Damage { get; } = new ReactiveProperty<float>();
        
        public override ReactiveProperty<float> Speed { get; } = new ReactiveProperty<float>();
    }
}