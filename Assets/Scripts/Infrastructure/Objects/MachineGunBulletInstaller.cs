using PCoreAdapters.Gameplay;
using PEntities.Gameplay.Combat;
using PEntities.Meta.Data;
using UnityEngine;
using Zenject;

namespace PInfrastructure.Objects
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MachineGunBulletInstaller : MonoInstaller
    {
        [SerializeField] private BaseBulletConfigHolder _baseBulletConfig;
        [SerializeField] private Rigidbody2D _playerRigidbody2D;

        public override void InstallBindings()
        {
            Container.Bind<BaseBulletData>()
                .FromInstance(_baseBulletConfig.BulletData)
                .AsSingle();

            Container.Bind<Rigidbody2D>()
                .FromInstance(_playerRigidbody2D)
                .AsSingle()
                .When(ctx => ctx.ObjectType == typeof(PhysicsBullet));
            
            Container.Bind<IBullet>()
                .To<PhysicsBullet>()
                .AsSingle();
            
            Container.Bind<MonoBullet>()
                .FromComponentOn(gameObject)
                .AsSingle();
        }
    }
}