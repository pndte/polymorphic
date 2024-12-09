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
        private BaseBulletData _defaultData;

        private void Awake()
        {
            _physics = GetComponent<Rigidbody2D>(); 
            _defaultData = Resources.Load<BaseBulletConfigHolder>("Data/Combat/DefaultBulletConfig").BulletData; // TODO: remove absolute path
            SetDefaultConfig();
        }

        [field: SerializeField] public override BaseBulletData Data { get; protected set; }

        private void FixedUpdate()
        {
            if (!_launched) return;

            Move(_cachedDirection);
        }

        public override void Move(Vector2 direction)
        {
            _physics.velocity = direction * Data.Speed.Value;
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
        
        private void SetDefaultConfig() => Data = _defaultData;

        public override event Action<Bullet> OnReset;

        public override void Launch(Vector2 direction)
        {
            _cachedDirection = direction;
            _launched = true;

            Invoke(nameof(Reset), Data.LiveTime.Value); // TODO: maybe UniTask.
        }

        public override void Launch(BaseBulletData bulletData, Vector2 direction)
        {
            Data = new BaseBulletData(bulletData.Speed.Value, bulletData.Damage.Value, bulletData.LiveTime.Value); // TODO: optimize mb
            Launch(direction);
        }

        public class Factory : PlaceholderFactory<MachineGunBullet>
        {
        }
    }
}