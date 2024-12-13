using UnityEngine;

namespace PEntities.Gameplay.Combat
{
    public interface IWeapon : IReloadable
    {
        public Bullet Shoot(Vector2 direction);
    }
}