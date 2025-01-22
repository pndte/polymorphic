using PCoreAdapters.Gameplay;
using PCoreAdapters.Utils;
using PEntities.Gameplay.Combat;
using PEntities.Meta.Data;
using UnityEngine;
using Zenject;

namespace PInfrastructure.Scenes
{
    public class CombatInstaller : MonoInstaller
    {
        [SerializeField] private MonoBullet _machineGunBulletPrefab;
        [SerializeField] private MachineGunBulletProvider _machineGunBulletProvider;
        [SerializeField] private BaseBulletConfigHolder _playerMachineGunBulletConfigHolder;
        [SerializeField] private TempPlayer _player;

        public override void InstallBindings()
        {
            Container.BindFactory<MonoBullet, MonoBullet.Factory>()
                .FromSubContainerResolve()
                .ByNewContextPrefab(_machineGunBulletPrefab);

            Container.Bind<TempPlayer>()
                .FromInstance(_player)
                .AsSingle();

            Container.Bind<MachineGunBulletProvider>()
                .FromComponentInNewPrefab(_machineGunBulletProvider)
                .AsSingle();
        }
    }
}