using Meta.Data;
using UnityEngine;
using Zenject;

namespace Infrastructure.Scenes
{
    public class ProjectileInstaller: MonoInstaller
    {
        // private void Awake()
        // {
        //     Container.InstantiatePrefabForComponent<>()
        // }

        public override void InstallBindings()
        {
            InstallConfigs();
        }

        public void InstallConfigs()
        {
            Container.Bind<MachineGunBulletConfig>()
                .FromInstance(Resources.Load<MachineGunBulletConfigHolder>("Data/MachineGunBulletConfig").Config)
                .AsTransient();
        }
    }
}