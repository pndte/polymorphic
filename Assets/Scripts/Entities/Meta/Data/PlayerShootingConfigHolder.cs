using UnityEngine;

namespace Entities.Meta.Data
{
    [CreateAssetMenu(menuName = "Create PlayerShootingConfigHolder", fileName = "PlayerShootingConfig", order = 0)]
    public class PlayerShootingConfigHolder : ScriptableObject
    {
        public PlayerShootingConfig Config;
    }
}