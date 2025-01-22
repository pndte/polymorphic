using System.Collections.Generic;
using PCoreAdapters.Gameplay;
using PCoreAdapters.Utils;
using PEntities.Gameplay.Combat;
using PEntities.Gameplay.Motion;
using PEntities.Meta.Data;
using PInfrastructure.Meta.Data;
using PUseCases.Gameplay;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace PInfrastructure.Objects
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private MovementConfigHolder _movementConfigHolder;
        private MachineGunBulletProvider _machineGunBulletProvider;

        [Header("Machine Gun Config")]
        [SerializeField] private BaseWeaponConfigHolder _machineGunConfigHolder;
        [SerializeField] private BaseBulletConfigHolder _machineGunBulletConfigHolder;

        [Header("Cannon Gun Config")]
        [SerializeField] private BaseWeaponConfigHolder _cannonGunConfigHolder;
        [SerializeField] private BaseBulletConfigHolder _cannonGunConfigBulletConfigHolder;

        private readonly DefaultHealth _sharedHealth = new(100, 100);

        public override void InstallBindings()
        {
            _machineGunBulletProvider = Container.Resolve<MachineGunBulletProvider>();
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
                    new DirectionMovement(_movementConfigHolder.Config, GetComponent<Rigidbody2D>()),
                    _sharedHealth,
                    new List<IWeapon>() { CreateMachineGun() }),
                new(
                    new DirectionMovement(new MovementConfig(_movementConfigHolder.Config.Speed / 2),
                        GetComponent<Rigidbody2D>()),
                    _sharedHealth,
                    new[] { CreateCannonGun() })
            };
        }

        private IWeapon CreateMachineGun()
        {
            return new BaseGun(_machineGunBulletProvider, transform,
                _machineGunConfigHolder.Config, _machineGunBulletConfigHolder.Config);
        }

        private IWeapon CreateCannonGun()
        {
            var baseGun = new BaseGun(_machineGunBulletProvider, transform,
                _cannonGunConfigHolder.Config, _cannonGunConfigBulletConfigHolder.Config);
            return baseGun;
        }
    }
}