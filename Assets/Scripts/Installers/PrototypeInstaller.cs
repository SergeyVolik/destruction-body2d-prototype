
using UnityEngine;
using Zenject;

namespace Prototype
{

    public class PrototypeInstaller : MonoInstaller
    {
       
        [SerializeField] private Bullet m_BulletPrefab;
        [SerializeField] private BodyCellNode m_BodyCellPrefab;
        [SerializeField] private RagdollModel m_RagdollModelPrefab;
        [SerializeField] private SlowmotionManager m_TimeManager;
        [SerializeField] private EnemySpawner m_EnemySpawner;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputReader>().AsSingle();
            Container.Bind<SlowmotionManager>().FromInstance(m_TimeManager).AsSingle();
            Container.Bind<EnemySpawner>().FromInstance(m_EnemySpawner).AsSingle();
            InstallFactories();
        }

        private void InstallFactories()
        {
            Container.BindFactory<Bullet, Bullet.Factory>()
              .FromComponentInNewPrefab(m_BulletPrefab)
              .WithGameObjectName("Bullet")
              .UnderTransformGroup("Bullets");

            Container.BindFactory<BodyCellNode, BodyCellNode.Factory>()
                .FromComponentInNewPrefab(m_BodyCellPrefab)
                .WithGameObjectName("BodyCell")
                .UnderTransformGroup("BodyCells");

            Container.BindFactory<RagdollModel, RagdollModel.Factory>()
                .FromComponentInNewPrefab(m_RagdollModelPrefab)
                .WithGameObjectName("RagdollModel")
                .UnderTransformGroup("RagdollModels");
        }
    }

}