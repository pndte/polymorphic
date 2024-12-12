using Entities.Gameplay.Combat;
using Entities.Meta.Data;
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
            

            Container.Bind<IBulletProvider>()
                .FromComponentInNewPrefab(_machineGunBulletProvider)
                .AsSingle()
                .When(ctx => ctx.ObjectType == typeof(KeyboardShooting));

            Container.Bind<BaseBulletData>()
                .FromInstance(_playerMachineGunBulletConfigHolder.BulletData)
                .AsSingle();
        }
    }
}