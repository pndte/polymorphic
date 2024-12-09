using System;
using R3;
using UnityEngine;

namespace Meta.Data
{
    [Serializable]
    public class BaseBulletData
    {
        public BaseBulletData(float speed, float damage, float liveTime)
        {
            if (liveTime < 0) throw new ArgumentOutOfRangeException(nameof(liveTime), "Live time must be greater than or equal to zero.");
            
            Speed = new SerializableReactiveProperty<float>(speed);
            Damage = new SerializableReactiveProperty<float>(damage);
            LiveTime = new SerializableReactiveProperty<float>(liveTime);
        }
        
        [field: SerializeField] public SerializableReactiveProperty<float> Speed { get; private set; }
        [field: SerializeField] public SerializableReactiveProperty<float> Damage { get; private set; }
        [field: SerializeField] public SerializableReactiveProperty<float> LiveTime { get; private set; }
    }
}