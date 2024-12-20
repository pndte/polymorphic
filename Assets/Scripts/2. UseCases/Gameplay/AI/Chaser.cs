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
            var direction = (_origin.position - _target.position).normalized;
            _shipMorph.Move(direction);
            return NodeState.Success;
        }
    }
}