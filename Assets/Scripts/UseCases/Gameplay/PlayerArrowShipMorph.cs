using System;
using System.Collections.Generic;
using System.Linq;
using PEntities.Gameplay.Combat;
using PEntities.Gameplay.Motion;
using R3;
using UnityEngine;

namespace PUseCases.Gameplay
{
    public class PlayerArrowShipMorph : IShipMorph
    {
        private readonly IMovable _movable;
        private readonly IMortal _mortal;

        private Type _currentWeaponType;

        public PlayerArrowShipMorph(IMovable movable, IMortal mortal,
            IReadOnlyDictionary<Type, IWeapon> weapons)
        {
            _movable = movable;
            _mortal = mortal;
            Weapons = weapons;
            _currentWeaponType = weapons.Keys.First();
        }

        public IReadOnlyDictionary<Type, IWeapon> Weapons { get; }
        public IWeapon CurrentWeapon => Weapons[_currentWeaponType];
        public ReactiveProperty<float> CurrentHealth => _mortal.CurrentHealth;
        public ReactiveProperty<float> MaximumHealth => _mortal.MaximumHealth;
        public ReactiveProperty<bool> IsDead => _mortal.IsDead;
        public void ChangeCurrentWeaponTo<TWeapon>() where TWeapon : IWeapon
        {
            _currentWeaponType = typeof(TWeapon);
        }

        public void Move(Vector2 direction) => _movable.Move(direction);
        public void ApplyDamage(float damage) => _mortal.ApplyDamage(damage);
    }
}