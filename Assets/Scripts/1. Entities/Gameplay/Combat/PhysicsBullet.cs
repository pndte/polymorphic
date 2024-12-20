using PEntities.Meta.Data;
using R3;
using UnityEngine;

namespace PEntities.Gameplay.Combat
{
    public class PhysicsBullet : IBullet
    {
        private readonly Rigidbody2D _physics;

        public PhysicsBullet(BaseBulletData data, Rigidbody2D physics)
        {
            Data = data;
            _physics = physics;
            IsLaunched = new ReactiveProperty<bool>(false);
            LaunchedDirection = new ReactiveProperty<Vector2>();
        }

        public BaseBulletData Data { get; set; }
        public ReactiveProperty<bool> IsLaunched { get; }
        public ReactiveProperty<Vector2> LaunchedDirection { get; }
        
        public void Launch(Vector2 launchOrigin, Vector2 direction)
        {
            _physics.position = launchOrigin;
            LaunchedDirection.Value = direction;
            IsLaunched.Value = true;
        }

        public void Move(Vector2 direction)
        {
            _physics.velocity = direction * Data.Speed.Value;
        }
    }
}