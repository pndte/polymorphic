using System;
using UnityEngine;

namespace PEntities.Meta.Data
{
    [Serializable,
     CreateAssetMenu(menuName = "Create BaseBulletConfigHolder", fileName = "BaseBulletConfigHolder", order = 0)]
    public class BaseBulletConfigHolder : ScriptableObject
    {
        public BaseBulletData BulletData;
    }
}