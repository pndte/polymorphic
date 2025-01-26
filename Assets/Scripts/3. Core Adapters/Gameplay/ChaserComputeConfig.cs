using UnityEngine;

namespace PCoreAdapters.Gameplay
{
    [CreateAssetMenu(fileName = "ChaserComputeConfig", menuName = "ChaserComputeConfig", order = 1)]
    public class ChaserComputeConfig : ScriptableObject
    {
        public Vector2 DomainMin = new Vector2(-100, -100);
        public Vector2 DomainMax = new Vector2(100, 100);
        public float CellSize = 5f;

        public float SeparationRadius = 8f;
        public float SeparationStrength = 1.5f;
        public float BaseSpeed = 10f;
        public float MinArriveDistance = 15f;
        public float ArriveRange = 5f;
        public float MaxChaseDistance = 70f;
        public float MinDistanceToSlow = 25f;
    }
}