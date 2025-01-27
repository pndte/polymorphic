using PEntities.Meta.Data;
using UnityEngine;

namespace PUseCases.Meta.Data
{
    [CreateAssetMenu(fileName = "ChaserConfig", menuName = "ChaserConfig", order = 1)]
    public class ChaserConfig : ScriptableObject
    {
        public Vector2 DomainMin = new Vector2(-100, -100);
        public Vector2 DomainMax = new Vector2(100, 100);
        public float CellSize = 5f;
        
        public MovementConfig MovementConfig;
        public float SeparationRadius = 8f;
        public float SeparationStrength = 1.5f;
        public float MinArriveDistance = 15f;
        public float ArriveRange = 5f;
        public float MaxChaseDistance = 70f;
        public float MinDistanceToSlow = 25f;
    }
}