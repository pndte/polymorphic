using System;
using Gameplay.Motion;
using Meta.Data;
using UnityEngine;

namespace Gameplay.Combat
{
    public abstract class Bullet : MonoBehaviour, IMovable, IResettable<Bullet>
    {
        public abstract BaseBulletData Data { get; protected set; }
        public abstract void Launch(Vector2 direction);
        public abstract void Launch(BaseBulletData bulletData, Vector2 direction);
        public abstract void Move(Vector2 direction);
        public abstract void Reset();
        public abstract event Action<Bullet> OnReset;
    }
}