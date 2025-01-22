using PEntities.Meta.Data;
using UnityEngine;

namespace PEntities.Gameplay.Motion
{
    public class VelocityMovement : IMovable
    {
        private readonly MovementConfig _config;
        private readonly Rigidbody2D _physics;
        
        public VelocityMovement(MovementConfig config, Rigidbody2D physics)
        {
            _config = config;
            _physics = physics;
        }

        public void Move(Vector2 velocity)
        {
            _physics.velocity = velocity.Truncate(_config.Speed);
        }
    }
}