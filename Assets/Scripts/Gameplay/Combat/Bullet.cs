using System;
using Gameplay.Motion;
using Meta.Data;
using UnityEngine;

namespace Gameplay.Combat
{
    public abstract class Bullet : MonoBehaviour, IMovable, IResettable<Bullet>
    {
        public abstract BaseBulletConfig Config { get; set; }
        public abstract void Launch(Vector2 direction);
        public abstract void Launch(BaseBulletConfig bulletConfig, Vector2 direction);
        public abstract void Move(Vector2 direction);
        public abstract void Reset();
        public abstract event Action<Bullet> OnReset;
    }
}