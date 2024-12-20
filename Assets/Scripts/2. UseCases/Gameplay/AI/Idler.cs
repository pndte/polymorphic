using System;
using Cysharp.Threading.Tasks;
using PEntities.Gameplay.Behaviour;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PUseCases.Gameplay.AI
{
    public class Idler : INode
    {
        private readonly IShipMorph _shipMorph;
        private readonly Transform _originTransform;
        private Vector2 _randomDirection;
        private bool _isReadyToChangeDirection;

        public Idler(IShipMorph shipMorph, Transform originTransform)
        {
            _shipMorph = shipMorph;
            _originTransform = originTransform;
            _isReadyToChangeDirection = true;
        }
        
        public NodeState Evaluate()
        {
            if (_isReadyToChangeDirection) ChangeDirectionAsync();
            
            _originTransform.up = Vector2.Lerp(_originTransform.up, _randomDirection, 0.125f);
            
            _shipMorph.Move(_randomDirection / 2);
            
            return NodeState.Success;
        }

        private async UniTaskVoid ChangeDirectionAsync()
        {
            _randomDirection = new Vector2(Random.Range(-100f, 100f), Random.Range(-100f, 100f)).normalized;
            
            _isReadyToChangeDirection = false;
            
            await UniTask.Delay(TimeSpan.FromSeconds(3));
            _isReadyToChangeDirection = true;
        }
    }
}