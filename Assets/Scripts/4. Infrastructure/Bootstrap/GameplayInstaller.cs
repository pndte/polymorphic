using PEntities.Meta.Data;
using UnityEngine;
using Zenject;

namespace PInfrastructure.Bootstrap
{
    public class GameplayInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallConfigs();
        }

        private void InstallConfigs()
        {
            Container.Bind<GameCameraConfig>()
                .FromInstance(Resources.Load<GameCameraConfigHolder>("Data/GameCameraConfig").Config)  // TODO: remove absolute path
                .AsSingle();
            
            Container.Bind<BaseWeaponConfig>()
                .FromInstance(Resources.Load<PlayerShootingConfigHolder>("Data/PlayerShootingConfig").Config)  // TODO: remove absolute path
                .AsSingle();
        }
    }
}