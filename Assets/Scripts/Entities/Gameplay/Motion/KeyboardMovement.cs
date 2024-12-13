using PEntities.Meta.Data;
using UnityEngine;
using Zenject;

namespace PEntities.Gameplay.Motion
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class KeyboardMovement : MonoBehaviour, IMovable
    {
        private PlayerMovementConfig _config;
        private Rigidbody2D _physics;
        private Vector2 _cachedDirection = Vector2.zero;
        
        [Inject]
        private void Construct(PlayerMovementConfig config)
        {
            _config = config;
            _physics = GetComponent<Rigidbody2D>();
        }

        public void Move(Vector2 direction)
        {
            _physics.velocity = direction * _config.Speed;
        }

        void Update()
        {
            DefineDirection();
        }

        private void FixedUpdate()
        {
            Move(_cachedDirection);
        }

        private void DefineDirection()
        {
            if (Input.GetKey(KeyCode.A)) _cachedDirection.x = Mathf.Clamp(_cachedDirection.x - _config.DirectionIncreaseSpeed, -1, 0);
            else if (Input.GetKey(KeyCode.D)) _cachedDirection.x = Mathf.Clamp(_cachedDirection.x + _config.DirectionIncreaseSpeed, 0, 1);
            else _cachedDirection.x = 0;

            if (Input.GetKey(KeyCode.S)) _cachedDirection.y = Mathf.Clamp(_cachedDirection.y - _config.DirectionIncreaseSpeed, -1, 0);
            else if (Input.GetKey(KeyCode.W)) _cachedDirection.y = Mathf.Clamp(_cachedDirection.y + _config.DirectionIncreaseSpeed, 0, 1);
            else _cachedDirection.y = 0;
        }
    }
}