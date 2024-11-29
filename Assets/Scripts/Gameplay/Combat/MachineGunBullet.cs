using System;
using Meta.Data;
using UnityEngine;
using Zenject;

namespace Gameplay.Combat
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MachineGunBullet : Bullet
    {
        [field: SerializeField] public override MachineGunBulletConfig Config { get; protected set; }

        private Vector2 _cachedDirection;
        private bool _launched;
        private Rigidbody2D _physics;

        [Inject]
        private void Construct(MachineGunBulletConfig config)
        {
            Config = config;
        }

        private void Awake()
        {
            _physics = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (!_launched) return;

            Move(_cachedDirection);
        }

        public override void Move(Vector2 direction)
        {
            _physics.velocity = direction * Config.Speed.Value;
        }

        public override void Reset()
        {
            transform.position = new Vector3(0, 0, 0);
            _cachedDirection = Vector2.zero;
            _launched = false;
            _physics.velocity = Vector2.zero;
            
            OnReset?.Invoke(this);
        }

        public override event Action<Bullet> OnReset;


        public override void Launch(Vector2 direction)
        {
            _cachedDirection = direction;
            _launched = true;
            
            Invoke(nameof(Reset), Config.LiveTime.Value); // TODO: maybe UniTask.
        }

        public class Factory : PlaceholderFactory<MachineGunBullet> {}
    }
}