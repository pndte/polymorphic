using Gameplay.Combat;
using Meta.Data;
using UnityEngine;
using Zenject;

namespace Infrastructure.Scenes
{
    public class CombatInstaller : MonoInstaller
    {
        [SerializeField] private Bullet _machineGunBulletPrefab;
        [SerializeField] private MachineGunBulletProvider _machineGunBulletProvider;
        public override void InstallBindings()
        {
            Container.BindFactory<MachineGunBullet, MachineGunBullet.Factory>().
                FromComponentInNewPrefab(_machineGunBulletPrefab);
            
            Container.Bind<IBulletProvider>().
                WithId("MachineGun").
                FromComponentInNewPrefab(_machineGunBulletProvider).
                AsSingle();
        }
    }
}