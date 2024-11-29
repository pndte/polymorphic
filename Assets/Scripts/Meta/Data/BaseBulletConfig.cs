using System;
using R3;
using UnityEngine;

namespace Meta.Data
{
    [Serializable,
     CreateAssetMenu(menuName = "Create BaseBulletConfig", fileName = "BaseBulletConfig", order = 0)]
    public class BaseBulletConfig : ScriptableObject
    {
        [field: SerializeField] public virtual SerializableReactiveProperty<float> Speed { get; protected set; }
        [field: SerializeField] public virtual SerializableReactiveProperty<float> Damage { get; protected set; }
        [field: SerializeField] public virtual SerializableReactiveProperty<float> LiveTime { get; protected set; }
    }
}