using System;
using R3;

namespace Meta.Data
{
    [Serializable]
    public class MachineGunBulletConfig
    {
        public SerializableReactiveProperty<float> Damage;
        public SerializableReactiveProperty<float> Speed;
        public SerializableReactiveProperty<float> LiveTime;
    }
}