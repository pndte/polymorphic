using System;
using System.Collections.Generic;
using PEntities.Gameplay.Combat;
using PEntities.Gameplay.Motion;

namespace PUseCases.Gameplay
{
    public interface IShipMorph: IMovable, IDamageable
    {
        public IWeapon CurrentWeapon { get; }
        public void ChangeCurrentWeaponTo<TWeapon>() where TWeapon : IWeapon;
        public IReadOnlyDictionary<Type, IWeapon> Weapons { get; }
    }
}