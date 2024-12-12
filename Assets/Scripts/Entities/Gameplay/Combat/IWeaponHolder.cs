using System.Collections.Generic;

namespace Entities.Gameplay.Combat
{
    public interface IWeaponHolder
    {
        public IWeapon Current();
        public void Change(int weaponIndex);
        public IList<IWeapon> Weapons { get; }
    }
}