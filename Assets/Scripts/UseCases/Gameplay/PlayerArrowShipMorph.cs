using PEntities.Gameplay.Combat;
using PEntities.Gameplay.Motion;
using UnityEngine;

namespace PUseCases.Gameplay
{
    public class PlayerArrowShipMorph: IShipMorph
    {
        private readonly IMovable _movable;
        private readonly IDamageable _damageable;
        private readonly IWeaponHolder _weaponHolder;

        public PlayerArrowShipMorph(IMovable movable, IDamageable damageable, IWeaponHolder weaponHolder)
        {
            _movable = movable;
            _damageable = damageable;
            _weaponHolder = weaponHolder;
        }
        
        public void Move(Vector2 direction) => _movable.Move(direction);

        public void TakeDamage(float damage) => _damageable.TakeDamage(damage);

        public Bullet Shoot(int weaponIndex, Vector2 direction)
        {
            _weaponHolder.Change(weaponIndex);
            return _weaponHolder.Current.Shoot(direction);
        }

        public bool IsReadyToShoot => _weaponHolder.Current.IsReadyToShoot;
    }
}