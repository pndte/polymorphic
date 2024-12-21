using System;
using System.Collections.Generic;
using PCoreAdapters.Gameplay;
using PEntities.Gameplay.Combat;
using PEntities.Gameplay.Motion;
using PEntities.Meta.Data;
using PUseCases.Gameplay;
using UnityEngine;
using Zenject;

namespace PInfrastructure.Objects
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private BaseBulletConfigHolder _playerMachineGunBulletConfigHolder;
        [SerializeField] private MovementConfigHolder _playerMovementConfigHolder;

        private readonly DefaultHealth _sharedHealth = new(100, 100);
        
        public override void InstallBindings()
        {
            InstallPlayer();
        }
        
        private void InstallPlayer()
        {
            Container.Bind<IShipMorph[]>()
                .To<DefaultMorph[]>()
                .FromMethod(GetPlayerMorphs)
                .AsSingle();
            
            Container.Bind<IMortal>()
                .To<DefaultHealth>()
                .FromInstance(_sharedHealth)
                .AsSingle();
            
            Container.Bind<TempPlayer>()
                .FromComponentOn(gameObject)
                .AsSingle();
        }

        private DefaultMorph[] GetPlayerMorphs()
        {
            return new DefaultMorph[]
            {
                new(
                    new PhysicsMovement(_playerMovementConfigHolder.Config, GetComponent<Rigidbody2D>()),
                    _sharedHealth,
                    new Dictionary<Type, IWeapon>() { { typeof(MachineGun), Container.Resolve<MachineGun>() } })
            };
        }
    }
}