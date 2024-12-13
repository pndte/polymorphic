using UnityEngine;

namespace PEntities.Meta.Data
{
    [CreateAssetMenu(menuName = "Create PlayerMovementConfigHolder", fileName = "PlayerMovementConfig", order = 0)]
    public class PlayerMovementConfigHolder : ScriptableObject
    {
        public PlayerMovementConfig Config;
    }
}