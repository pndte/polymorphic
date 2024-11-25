using Gameplay.Motion;
using R3;
using UnityEngine;

namespace Gameplay.Combat
{
    public abstract class Bullet: MonoBehaviour, IMovable
    {
        public abstract ReactiveProperty<float> Damage { get; }
        
        public abstract ReactiveProperty<float> Speed { get; }
        
        public abstract void Launch(Vector2 direction);
        
        public abstract void Move(Vector2 direction);
    }
}