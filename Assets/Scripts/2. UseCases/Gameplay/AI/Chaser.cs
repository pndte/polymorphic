using PEntities.Gameplay.Behaviour;
using PUseCases.Meta.Data;
using UnityEngine;

namespace PUseCases.Gameplay.AI
{
    public class Chaser : INode
    {
        private readonly IShipMorph _shipMorph;
        private readonly Transform _origin;
        private readonly Transform _target;
        private readonly ChaserConfig _chaserConfig;
        
        private bool _isSeparating;

        public Chaser(IShipMorph shipMorph, ChaserConfig chaserConfig, Transform origin, Transform target)
        {
            _shipMorph = shipMorph;
            _chaserConfig = chaserConfig;
            _origin = origin;
            _target = target;
        }

        public NodeState Evaluate()
        {
            Vector2 toTarget = _target.position - _origin.position;
            float distance = toTarget.magnitude;

            if (distance > 35f)
            {
                return NodeState.Failure;
            }

            var direction = toTarget.normalized;

            LookAt(direction);

            if (distance < 25f)
            {
                direction = Arrive(distance, direction);
            }

            direction = Separation(direction);
            direction = Vector2.ClampMagnitude(direction, 1);

            _shipMorph.Move(direction);
            return NodeState.Success;
        }

        private void LookAt(Vector2 direction)
        {
            _origin.up = Vector2.Lerp(_origin.up, direction, 0.2f);
        }

        private Vector2 Separation(Vector2 direction)
        {
            Vector2 separationForce = Vector2.zero;
            int nearbyEnemies = 0;

            Vector2 checkPosition = (Vector2)_origin.position;

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(
                checkPosition,
                _chaserConfig.SeparationRadius
            );

            foreach (var collider in hitColliders)
            {
                if (collider.transform == _origin)
                    continue;

                Vector2 otherPos = collider.transform.position;
                Vector2 difference = checkPosition - otherPos;
                float dist = difference.magnitude;

                if (dist > 0 && dist < _chaserConfig.SeparationRadius)
                {
                    difference.Normalize();
                    separationForce += difference / dist;
                    nearbyEnemies++;
                }
            }

            if (nearbyEnemies > 0 && (separationForce.magnitude > 0.1f * _chaserConfig.SeparationRadius || _isSeparating))
            {
                _isSeparating = true;
                // separationForce /= nearbyEnemies;
                separationForce = separationForce.normalized * _chaserConfig.SeparationStrength;

                var maxMagnitude = direction.magnitude;
                direction += separationForce * 0.2f;

                direction = Vector2.ClampMagnitude(direction, maxMagnitude);
            }
            else
            {
                _isSeparating = false;
            }

            return direction;
        }

        private Vector2 Arrive(float distance, Vector2 direction)
        {
            float speed = _chaserConfig.MovementConfig.Speed * ((distance - 15f) / 5f);
            direction *= speed / _chaserConfig.MovementConfig.Speed;

            return direction;
        }
    }
}