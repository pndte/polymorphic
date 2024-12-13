using System;
using System.Collections.Generic;
using System.Linq;

namespace PEntities.Gameplay.Combat
{
    public class BaseWeaponHolder : IWeaponHolder
    {
        private readonly List<IWeapon> _weapons;
        private int _currentWeapon;
        public BaseWeaponHolder(params IWeapon[] weapons)
        {
            _weapons = weapons.ToList();
        }

        public IWeapon Current => _weapons[_currentWeapon];
        public void Change(int weaponIndex)
        {
            if (weaponIndex > _weapons.Count - 1)
                throw new IndexOutOfRangeException($"There's {_weapons.Count - 1} maximum index, while the given index is {weaponIndex}");
            if (weaponIndex < 0)
                throw new IndexOutOfRangeException($"Index should be greater or equal to zero, while the given index is {weaponIndex}"); 
            
            _currentWeapon = weaponIndex;
        }

        public IList<IWeapon> Weapons => _weapons;
    }
}