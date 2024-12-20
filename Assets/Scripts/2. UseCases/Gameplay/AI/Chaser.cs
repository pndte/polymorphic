using PEntities.Gameplay.Behaviour;
using UnityEngine;

namespace PUseCases.Gameplay.AI
{
    public class Chaser : INode
    {
        private readonly IShipMorph _shipMorph;
        private readonly Transform _origin;
        private readonly Transform _target;
        
        public Chaser(IShipMorph shipMorph, Transform origin, Transform target)
        {
            _shipMorph = shipMorph;
            _origin = origin;
            _target = target;
        }

        public NodeState Evaluate()
        {
            var distance = Vector2.Distance(_origin.position, _target.position);
            
            if (distance > 25f)
            {
                return NodeState.Failure;
            }
            
            var direction = (_target.position - _origin.position).normalized;
            
            _origin.up = Vector2.Lerp(_origin.up, direction, 0.3f);

            if (distance < 5f)
            {
                return NodeState.Success;
            }
            
            _shipMorph.Move(direction);
            
            return NodeState.Success;
        }
    }
}