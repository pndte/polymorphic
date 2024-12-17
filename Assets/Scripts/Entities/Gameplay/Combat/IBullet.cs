using PEntities.Gameplay.Motion;
using PEntities.Meta.Data;
using R3;
using UnityEngine;

namespace PEntities.Gameplay.Combat
{
    public interface IBullet : IMovable
    {
        public BaseBulletData Data { get; set; }
        public ReactiveProperty<bool> IsLaunched { get; }
        public ReactiveProperty<Vector2> LaunchedDirection { get; }
        public void Launch(Vector2 launchOrigin, Vector2 direction);
    }
}