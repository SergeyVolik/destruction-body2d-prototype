
using UnityEngine;
using Zenject;

namespace Prototype
{

    public class ProjectInstaller : MonoInstaller
    {

        [SerializeField] private CannonBall m_CannonBallPrefab;
        [SerializeField] private GrenadeProjectile m_GrenadeProjectile;
        [SerializeField] private LaserProjectile m_LaserProjectile;
        [SerializeField] private PistolBullet m_PistolBullet;

        [SerializeField] private ExplosionVFX m_ExplosionVFX;

        public override void InstallBindings()
        {
            Container.Bind<PlayerInventory>().FromNew().AsSingle();

            InstallPoolsOfProjectiles();
            InstallVFX();
        }

        private void InstallPoolsOfProjectiles()
        {
            Container.BindMemoryPool<CannonBall, CannonBall.Pool>()
                .WithInitialSize(10)
                .FromComponentInNewPrefab(m_CannonBallPrefab)
                .WithGameObjectName("CannonBall")
                .UnderTransformGroup("CannonBalls");

            Container.BindMemoryPool<GrenadeProjectile, GrenadeProjectile.Pool>()
                .WithInitialSize(10)
                .FromComponentInNewPrefab(m_GrenadeProjectile)
                .WithGameObjectName("GrenadeProjectile")
                .UnderTransformGroup("GrenadeProjectiles");

            Container.BindMemoryPool<LaserProjectile, LaserProjectile.Pool>()
                .WithInitialSize(10)
                .FromComponentInNewPrefab(m_LaserProjectile)
                .WithGameObjectName("LaserProjectile")
                .UnderTransformGroup("LaserProjectiles");

            Container.BindMemoryPool<PistolBullet, PistolBullet.Pool>()
                .WithInitialSize(10)
                .FromComponentInNewPrefab(m_PistolBullet)
                .WithGameObjectName("PistolBullet")
                .UnderTransformGroup("PistolBullets");
            
        }

        private void InstallVFX()
        {
            Container.BindMemoryPool<ExplosionVFX, ExplosionVFX.Pool>()
               .WithInitialSize(10)
               .FromComponentInNewPrefab(m_ExplosionVFX)
               .WithGameObjectName("ExplosionVFX")
               .UnderTransformGroup("ExplosionVFXes");
        }
    }

}