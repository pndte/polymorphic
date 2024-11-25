using Meta.Data;
using UnityEngine;
using Zenject;

namespace Infrastructure.Bootstrap
{
    public class GameplayInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallConfigs();
        }

        private void InstallConfigs()
        {
            Container.Bind<PlayerMovementConfig>()
                .FromInstance(Resources.Load<PlayerMovementConfigHolder>("Data/PlayerMovementConfig").Config)
                .AsSingle();
            
            Container.Bind<GameCameraConfig>()
                .FromInstance(Resources.Load<GameCameraConfigHolder>("Data/GameCameraConfig").Config)
                .AsSingle();
            
            Container.Bind<PlayerShootingConfig>()
                .FromInstance(Resources.Load<PlayerShootingConfigHolder>("Data/PlayerShootingConfig").Config)
                .AsSingle();
            
            Container.Bind<MachineGunBulletConfig>()
                .FromInstance(Resources.Load<MachineGunBulletConfigHolder>("Data/MachineGunBulletConfig").Config)
                .AsSingle();
        }
    }
}