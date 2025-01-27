using UnityEngine;

namespace PUseCases.Meta.Data
{
    [CreateAssetMenu(fileName = "ShooterConfig", menuName = "ShooterConfig", order = 1)]
    public class ShooterConfig : ScriptableObject
    {
        public float WeaponRange;
    }
}