using UnityEngine;

namespace PEntities.Meta.Data
{
    [CreateAssetMenu(menuName = "Create GameCameraConfigHolder", fileName = "GameCameraConfig", order = 0)]
    public class GameCameraConfigHolder : ScriptableObject
    {
        public GameCameraConfig Config;
    }
}