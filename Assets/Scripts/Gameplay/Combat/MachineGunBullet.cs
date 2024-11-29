using System;
using Meta.Data;
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
        private BaseBulletConfig _defaultConfig;

        private void Awake()
        {
            _physics = GetComponent<Rigidbody2D>();
            _defaultConfig = Resources.Load<BaseBulletConfig>("Data/Combat/DefaultBulletConfig");
            SetDefaultConfig();
        }

        [field: SerializeField] public override BaseBulletConfig Config { get; set; }

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
            SetDefaultConfig();

            OnReset?.Invoke(this);
        }
        
        private void SetDefaultConfig() => Config = _defaultConfig;

        public override event Action<Bullet> OnReset;

        public override void Launch(Vector2 direction)
        {
            _cachedDirection = direction;
            _launched = true;

            Invoke(nameof(Reset), Config.LiveTime.Value); // TODO: maybe UniTask.
        }

        public override void Launch(BaseBulletConfig bulletConfig, Vector2 direction)
        {
            Config = bulletConfig;
            Launch(direction);
        }

        public class Factory : PlaceholderFactory<MachineGunBullet>
        {
        }
    }
}