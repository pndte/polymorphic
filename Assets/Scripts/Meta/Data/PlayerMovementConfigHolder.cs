using UnityEngine;

namespace Meta.Data
{
    [CreateAssetMenu(menuName = "Create PlayerMovementConfigHolder", fileName = "Config", order = 0)]
    public class PlayerMovementConfigHolder : ScriptableObject
    {
        public PlayerMovementConfig Config;
    }
}