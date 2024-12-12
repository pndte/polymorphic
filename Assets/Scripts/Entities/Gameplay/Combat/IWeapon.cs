using UnityEngine;

namespace Entities.Gameplay.Combat
{
    public interface IWeapon
    {
        public Bullet Shoot(Vector2 direction);
        public bool IsReadyToShoot();
    }
}