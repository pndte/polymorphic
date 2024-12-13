using UnityEngine;

namespace PEntities.Gameplay.Combat
{
    public interface IWeapon
    {
        public Bullet Shoot(Vector2 direction);
        public bool IsReadyToShoot();
    }
}