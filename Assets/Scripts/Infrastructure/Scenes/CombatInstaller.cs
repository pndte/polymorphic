using System;
using System.Collections.Generic;
using PEntities.Gameplay.Combat;
using PEntities.Gameplay.Motion;
using PEntities.Meta.Data;
using PUseCases.Gameplay;
using UnityEngine;
using Zenject;
using Bullet = PEntities.Gameplay.Combat.Bullet;
using MachineGun = PEntities.Gameplay.Combat.MachineGun;

namespace Infrastructure.Scenes
{
    public class CombatInstaller : MonoInstaller
    {
        [SerializeField] private Bullet _machineGunBulletPrefab;
        [SerializeField] private MachineGunBulletProvider _machineGunBulletProvider;
        [SerializeField] private BaseBulletConfigHolder _playerMachineGunBulletConfigHolder;
        [SerializeField] private Rigidbody2D _playerRigidbody2D;
        public override void InstallBindings()
        {
            Container.BindFactory<MachineGunBullet, MachineGunBullet.Factory>().
                FromComponentInNewPrefab(_machineGunBulletPrefab);

            Container.Bind<BaseBulletData>()
                .FromInstance(_playerMachineGunBulletConfigHolder.BulletData)
                .AsSingle();
            
            InstallMachineGun();
            InstallPlayer();
        }

        private void InstallMachineGun()
        {
            Container.Bind<IBulletProvider>()
                .FromComponentInNewPrefab(_machineGunBulletProvider)
                .AsSingle()
                .When(ctx => ctx.ObjectType == typeof(MachineGun));
            
            Container.Bind<Transform>()
                .FromInstance(_playerRigidbody2D.transform)
                .AsSingle()
                .When(ctx => ctx.ObjectType == typeof(MachineGun));
            
            Container.Bind(typeof(MachineGun))
                .To<MachineGun>()
                .AsSingle();
        }
        
        private void InstallPlayer()
        {
            Container.Bind<IShipMorph>()
                .To<PlayerArrowShipMorph>()
                .FromMethod(GetPlayerMorph)
                .AsSingle();
        }

        private PlayerArrowShipMorph GetPlayerMorph()
        {
            return new PlayerArrowShipMorph(
                new PhysicsMovement(Container.Resolve<PlayerMovementConfig>(), _playerRigidbody2D),
                new DefaultHealth(100, 100),
                new Dictionary<Type, IWeapon>() {{typeof(MachineGun), Container.Resolve<MachineGun>()}});
        }
    }
}