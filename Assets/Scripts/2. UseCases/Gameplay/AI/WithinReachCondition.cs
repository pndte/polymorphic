using PEntities.Gameplay.Behaviour;
using PUseCases.Meta.Data;
using UnityEngine;

namespace PUseCases.Gameplay.AI
{
    public class WithinReachCondition : INode
    {
        private readonly Transform _origin;
        private readonly Transform _target;
        private readonly ShooterConfig _shooterConfig;

        public WithinReachCondition(Transform origin, Transform target, ShooterConfig shooterConfig)
        {
            _origin = origin;
            _target = target;
            _shooterConfig = shooterConfig;
        }
        public NodeState Evaluate()
        {
            var distance = (_target.position - _origin.position).magnitude;
            if (_shooterConfig.WeaponRange >= distance)
            {
                return NodeState.Success;
            }
            
            return NodeState.Failure;
        }
    }
}