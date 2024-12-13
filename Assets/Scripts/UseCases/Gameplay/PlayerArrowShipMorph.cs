using System;
using System.Collections.Generic;
using System.Linq;
using PEntities.Gameplay.Combat;
using PEntities.Gameplay.Motion;
using UnityEngine;

namespace PUseCases.Gameplay
{
    public class PlayerArrowShipMorph : IShipMorph
    {
        private readonly IMovable _movable;
        private readonly IDamageable _damageable;

        private Type _currentWeaponType;

        public PlayerArrowShipMorph(IMovable movable, IDamageable damageable,
            IReadOnlyDictionary<Type, IWeapon> weapons)
        {
            _movable = movable;
            _damageable = damageable;
            Weapons = weapons;
            _currentWeaponType = weapons.Keys.First();
        }

        public IReadOnlyDictionary<Type, IWeapon> Weapons { get; }

        public IWeapon CurrentWeapon => Weapons[_currentWeaponType];

        public void ChangeCurrentWeaponTo<TWeapon>() where TWeapon : IWeapon
        {
            _currentWeaponType = typeof(TWeapon);
        }

        public void Move(Vector2 direction) => _movable.Move(direction);

        public void TakeDamage(float damage) => _damageable.TakeDamage(damage);
    }
}