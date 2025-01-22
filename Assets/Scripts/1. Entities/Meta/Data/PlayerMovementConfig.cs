using System;

namespace PEntities.Meta.Data
{
    [Serializable]
    public class MovementConfig
    {
        public MovementConfig()
        { }

        public MovementConfig(float speed)
        {
            Speed = speed;
        }
        
        public float Speed;
    }
}