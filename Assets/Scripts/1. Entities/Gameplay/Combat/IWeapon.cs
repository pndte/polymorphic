using UnityEngine;

namespace PEntities.Gameplay.Combat
{
    public interface IWeapon : IReloadable
    {
        public IBullet Shoot(Vector2 direction);
    }
}