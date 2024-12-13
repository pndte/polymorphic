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

        private void DefineDirection()
        {
            // if (Input.GetKey(KeyCode.A)) _cachedDirection.x = Mathf.Clamp(_cachedDirection.x - _config.DirectionIncreaseSpeed, -1, 0);
            // else if (Input.GetKey(KeyCode.D)) _cachedDirection.x = Mathf.Clamp(_cachedDirection.x + _config.DirectionIncreaseSpeed, 0, 1);
            // else _cachedDirection.x = 0;
            //
            // if (Input.GetKey(KeyCode.S)) _cachedDirection.y = Mathf.Clamp(_cachedDirection.y - _config.DirectionIncreaseSpeed, -1, 0);
            // else if (Input.GetKey(KeyCode.W)) _cachedDirection.y = Mathf.Clamp(_cachedDirection.y + _config.DirectionIncreaseSpeed, 0, 1);
            // else _cachedDirection.y = 0;
        }
    }
}