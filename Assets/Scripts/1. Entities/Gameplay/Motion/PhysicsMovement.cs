using PEntities.Meta.Data;
using UnityEngine;

namespace PEntities.Gameplay.Motion
{
    public class PhysicsMovement : IMovable
    {
        private readonly PlayerMovementConfig _config;
        private readonly Rigidbody2D _physics;
        
        public PhysicsMovement(PlayerMovementConfig config, Rigidbody2D physics)
        {
            _config = config;
            _physics = physics;
        }

        public void Move(Vector2 direction)
        {
            _physics.velocity = direction * _config.Speed;
        }
    }
}