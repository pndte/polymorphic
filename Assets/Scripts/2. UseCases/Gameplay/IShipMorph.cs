using System.Collections.Generic;
using PEntities.Gameplay.Combat;
using PEntities.Gameplay.Motion;

namespace PUseCases.Gameplay
{
    public interface IShipMorph: IMovable, IMortal
    {
        public IWeapon CurrentWeapon { get; }
        public void ChangeCurrentWeapon(int weaponIndex);
        public IReadOnlyList<IWeapon> Weapons { get; }
    }
}