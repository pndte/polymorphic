using PCoreAdapters.Gameplay;
using PEntities.Meta.Data;
using UnityEngine;

namespace PInfrastructure.Meta.Data
{
    [CreateAssetMenu(menuName = "Create BaseWeaponConfigHolder", fileName = "BaseWeaponConfig", order = 0)]
    public class BaseWeaponConfigHolder : ScriptableObject
    {
        [SerializeField] private MonoBullet _bulletPrefab;
        [SerializeField] private float _cooldown;
        private BaseWeaponConfig _config;

        public BaseWeaponConfig Config
        {
            get
            {
                if (_config == null)
                    return _config = new BaseWeaponConfig() {BulletPrefab = _bulletPrefab, Cooldown = _cooldown};
                return _config;
            }
        }
    }
}