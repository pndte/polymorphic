using UnityEngine;

namespace Entities.Meta.Data
{
    [CreateAssetMenu(menuName = "Create PlayerShootingConfigHolder", fileName = "BaseWeaponConfig", order = 0)]
    public class PlayerShootingConfigHolder : ScriptableObject
    {
        public BaseWeaponConfig Config;
    }
}