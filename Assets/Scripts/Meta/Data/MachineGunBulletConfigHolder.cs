using UnityEngine;

namespace Meta.Data
{
    [CreateAssetMenu(menuName = "Create MachineGunBulletConfigHolder", fileName = "MachineGunBulletConfig", order = 0)]
    public class MachineGunBulletConfigHolder : ScriptableObject
    {
        public MachineGunBulletConfig Config;
    }
}