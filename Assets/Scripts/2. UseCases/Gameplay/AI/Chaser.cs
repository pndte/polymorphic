using PEntities;
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
                return NodeState.Failure; // TODO: переместить в условную ноду.
            }

            var direction = (Vector2)(_target.position - _origin.position).normalized;

            _origin.up = Vector2.Lerp(_origin.up, direction, 0.2f);
            
            direction = Separation(direction);

            if (distance < 25)
            {
                direction = Arrive(distance, direction);
            }

            _shipMorph.Move(direction);

            return NodeState.Success;
        }

        private Vector2 Separation(Vector2 direction)
        {
            Vector2 separationForce = Vector2.zero;
            int nearbyEnemies = 0;

            // Определяем позицию для проверки соседей
            Vector2 checkPosition = (Vector2)_origin.position + direction;

            // Получаем всех врагов в радиусе разделения
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(checkPosition, _chaserConfig.SeparationRadius);

            foreach (var collider in hitColliders)
            {
                // Игнорируем самого себя и объекты, не являющиеся врагами
                if (collider.transform != _origin)
                {
                    Vector2 difference = ( (Vector2)_origin.position - (Vector2)collider.transform.position ).normalized;
                    float distance = Vector2.Distance(_origin.position, collider.transform.position);

                    if (distance > 0 && distance < _chaserConfig.SeparationRadius)
                    {
                        // Добавляем вклад отталкивания, обратно пропорционально расстоянию
                        separationForce += difference / distance;
                        nearbyEnemies++;
                    }
                }
            }

            if (nearbyEnemies > 0)
            {
                // Усредняем силу отталкивания
                separationForce /= nearbyEnemies;

                // Нормализуем и умножаем на коэффициент силы отталкивания
                separationForce = separationForce.normalized * _chaserConfig.SeparationStrength;

                // Корректируем направление движения
                direction += Vector2.Lerp(direction, direction + separationForce, 0.2f);
            }

            return direction;
        }


        private Vector2 Arrive(float distance, Vector2 direction)
        {
            var maxMagnitude = direction.magnitude;
            float speed = _chaserConfig.MovementConfig.Speed * ((distance - 15) / 5);
            
            direction *= speed / _chaserConfig.MovementConfig.Speed;
            direction = direction.Truncate(maxMagnitude);
            
            return direction;
        }
    }
}