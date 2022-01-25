
using UnityEngine;
using Zenject;

namespace Prototype
{

    public class PrototypeInstaller : MonoInstaller
    {
       
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

            Container.BindFactory<RagdollModel, RagdollModel.Factory>()
                .FromComponentInNewPrefab(m_RagdollModelPrefab)
                .WithGameObjectName("RagdollModel")
                .UnderTransformGroup("RagdollModels");
        }
    }

}