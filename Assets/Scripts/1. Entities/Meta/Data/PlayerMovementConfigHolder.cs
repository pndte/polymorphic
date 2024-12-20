using UnityEngine;

namespace PEntities.Meta.Data
{
    [CreateAssetMenu(menuName = "Create PlayerMovementConfigHolder", fileName = "PlayerMovementConfig", order = 0)]
    public class MovementConfigHolder : ScriptableObject
    {
        public MovementConfig Config;
    }
}