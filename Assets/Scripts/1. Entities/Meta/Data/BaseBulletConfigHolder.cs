using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace PEntities.Meta.Data
{
    [Serializable,
     CreateAssetMenu(menuName = "Create BaseBulletConfigHolder", fileName = "BaseBulletConfigHolder", order = 0)]
    public class BaseBulletConfigHolder : ScriptableObject
    {
        [FormerlySerializedAs("BulletData")] public BaseBulletData Config;
    }
}