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
                .FromInstance(Resources.Load<PlayerMovementConfigHolder>("Data/PlayerMovementConfig").Config) // TODO: remove absolute path
                .AsSingle();
            
            Container.Bind<GameCameraConfig>()
                .FromInstance(Resources.Load<GameCameraConfigHolder>("Data/GameCameraConfig").Config)  // TODO: remove absolute path
                .AsSingle();
            
            Container.Bind<PlayerShootingConfig>()
                .FromInstance(Resources.Load<PlayerShootingConfigHolder>("Data/PlayerShootingConfig").Config)  // TODO: remove absolute path
                .AsSingle();
        }
    }
}