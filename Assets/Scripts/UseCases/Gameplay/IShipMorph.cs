using PEntities.Gameplay.Combat;
using PEntities.Gameplay.Motion;
using UnityEngine;

namespace PUseCases.Gameplay
{
    public interface IShipMorph: IMovable, IDamageable
    {
        public Bullet Shoot(int weaponIndex, Vector2 direction);
        public bool IsReadyToShoot { get; }
    }
}