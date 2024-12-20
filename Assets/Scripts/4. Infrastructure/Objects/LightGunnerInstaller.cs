using System;
using System.Collections.Generic;
using PCoreAdapters.Gameplay;
using PEntities.Gameplay.Behaviour;
using PEntities.Gameplay.Combat;
using PEntities.Gameplay.Motion;
using PEntities.Meta.Data;
using PUseCases.Gameplay;
using PUseCases.Gameplay.AI;
using UnityEngine;
using Zenject;

namespace PInfrastructure.Objects
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class LightGunnerInstaller : MonoInstaller
    {
        [SerializeField] private MovementConfigHolder _movementConfig;

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
                    new Chaser(shipMorph, transform,
                        Container.Resolve<TempPlayer>().transform),
                    new Idler(shipMorph, transform)
                }));
        }

        private DefaultMorph GetEnemyMorph()
        {
            return new DefaultMorph(
                new PhysicsMovement(_movementConfig.Config, GetComponent<Rigidbody2D>()),
                new DefaultHealth(15, 15),
                new Dictionary<Type, IWeapon>() { { typeof(MachineGun), Container.Resolve<MachineGun>() } });
        }
    }
}