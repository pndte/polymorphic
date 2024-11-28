using System;
using Gameplay.Combat;

namespace Meta.Data
{
    [Serializable]
    public class PlayerShootingConfig
    {
        public float Cooldown;
        public Bullet BulletPrefab;
    }
}