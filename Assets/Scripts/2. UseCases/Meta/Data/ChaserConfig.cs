using PEntities.Meta.Data;

namespace PUseCases.Meta.Data
{
    public class ChaserConfig
    {
        public ChaserConfig(MovementConfig movementConfig, float separationRadius, float separationStrength)
        {
            MovementConfig = movementConfig;
            SeparationRadius = separationRadius;
            SeparationStrength = separationStrength;
        }

        public MovementConfig MovementConfig { get; set; }
        public float SeparationRadius { get; set; }
        public float SeparationStrength { get; set; }
    }
}