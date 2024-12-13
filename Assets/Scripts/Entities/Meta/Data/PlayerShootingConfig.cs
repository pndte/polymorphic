using System;
using PEntities.Gameplay.Combat;

namespace PEntities.Meta.Data 
{
    [Serializable]
    public class BaseWeaponConfig
    {
        public float Cooldown;
        public Bullet BulletPrefab;
    }
}