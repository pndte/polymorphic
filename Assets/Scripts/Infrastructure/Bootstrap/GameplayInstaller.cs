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
                .FromInstance(Resources.Load<PlayerMovementConfigHolder>("Data/PlayerMovementConfig").PlayerMovementConfig)
                .AsSingle();
        }
    }
}