using System.Collections.Generic;
using PCoreAdapters.Gameplay;
using PCoreAdapters.Utils;
using PEntities.Gameplay.Behaviour;
using PEntities.Gameplay.Combat;
using PEntities.Gameplay.Motion;
using PEntities.Meta.Data;
using PInfrastructure.Meta.Data;
using PUseCases.Gameplay;
using PUseCases.Gameplay.AI;
using PUseCases.Meta.Data;
using UnityEngine;
using Zenject;

namespace PInfrastructure.Objects
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class LightGunnerInstaller : MonoInstaller
    {
        [SerializeField] private MovementConfigHolder _movementConfig;
        [SerializeField] private MachineGunBulletProvider _bulletProvider;
        [SerializeField] private BaseWeaponConfigHolder _enemyWeaponConfig;
        [SerializeField] private BaseBulletConfigHolder _bulletConfigHolder;

        public override void InstallBindings()
        {
            Container.Bind<IShipMorph>()
                .To<DefaultMorph>()
                .FromMethod(GetEnemyMorph)
                .AsSingle();

            Container.Bind<RootNode>()
                .ToSelf()
                .FromMethod(GetRootNode)
                .AsSingle();
        }

        private RootNode GetRootNode()
        {
            var shipMorph = Container.Resolve<IShipMorph>();

            return new RootNode(
                new Selector(new List<INode>()
                {
                    new Chaser(shipMorph, new ChaserConfig(_movementConfig.Config, 10, 2), transform,
                        Container.Resolve<TempPlayer>().transform),
                    new Idler(shipMorph, transform)
                }));
        }

        private DefaultMorph GetEnemyMorph()
        {
            return new DefaultMorph(
                new DirectionMovement(_movementConfig.Config, GetComponent<Rigidbody2D>()),
                new DefaultHealth(15, 15),
                new List<IWeapon>() { new BaseGun(_bulletProvider, transform, 
                    _enemyWeaponConfig.Config, _bulletConfigHolder.Config) });
        }
    }
}