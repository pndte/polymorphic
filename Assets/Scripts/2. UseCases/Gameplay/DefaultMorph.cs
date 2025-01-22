using System.Collections.Generic;
using PEntities.Gameplay.Combat;
using PEntities.Gameplay.Motion;
using R3;
using UnityEngine;

namespace PUseCases.Gameplay
{
    public class DefaultMorph : IShipMorph
    {
        private readonly IMovable _movable;
        private readonly IMortal _mortal;

        private int _currentWeaponIndex;

        public DefaultMorph(IMovable movable, IMortal mortal,
            IReadOnlyList<IWeapon> weapons)
        {
            _movable = movable;
            _mortal = mortal;
            Weapons = weapons;
            _currentWeaponIndex = 0;
        }

        public IReadOnlyList<IWeapon> Weapons { get; }
        public IWeapon CurrentWeapon => Weapons[_currentWeaponIndex];
        public ReactiveProperty<float> CurrentHealth => _mortal.CurrentHealth;
        public ReactiveProperty<float> MaximumHealth => _mortal.MaximumHealth;
        public ReactiveProperty<bool> IsDead => _mortal.IsDead;
        public void ChangeCurrentWeapon(int weaponIndex)
        {
            _currentWeaponIndex = weaponIndex;
        }
        public void Move(Vector2 direction) => _movable.Move(direction);
        public void ApplyDamage(float damage) => _mortal.ApplyDamage(damage);
    }
}