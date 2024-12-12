using System;
using Entities.Gameplay.Combat;

namespace Entities.Meta.Data 
{
    [Serializable]
    public class PlayerShootingConfig
    {
        public float Cooldown;
        public Bullet BulletPrefab;
    }
}