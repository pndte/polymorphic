using PEntities.Gameplay.Combat;
using PEntities.Meta.Data;
using UnityEngine;
using Zenject;

namespace Infrastructure.Scenes
{
    public class CombatInstaller : MonoInstaller
    {
        [SerializeField] private Bullet _machineGunBulletPrefab;
        [SerializeField] private MachineGunBulletProvider _machineGunBulletProvider;
        [SerializeField] private BaseBulletConfigHolder _playerMachineGunBulletConfigHolder;
        public override void InstallBindings()
        {
            Container.BindFactory<MachineGunBullet, MachineGunBullet.Factory>().
                FromComponentInNewPrefab(_machineGunBulletPrefab);

            Container.Bind<BaseBulletData>()
                .FromInstance(_playerMachineGunBulletConfigHolder.BulletData)
                .AsSingle();
        }

        public void InstallMachineGun()
        {
            Container.Bind<IBulletProvider>()
                .FromComponentInNewPrefab(_machineGunBulletProvider)
                .AsSingle()
                .When(ctx => ctx.ObjectType == typeof(MachineGun));
            
            //TODO: передать transform игрока
        }
    }
}