using System.Collections.Generic;

namespace PEntities.Gameplay.Combat
{
    public interface IWeaponHolder
    {
        public IWeapon Current { get; }
        public void Change(int weaponIndex);
        public IList<IWeapon> Weapons { get; }
    }
}