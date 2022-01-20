
using UnityEngine;
using Zenject;

namespace Prototype
{

    public class PrototypeInstaller : MonoInstaller
    {
       
        [SerializeField] private Bullet m_BulletPrefab;
        [SerializeField] private BodyCellNode m_BodyCellPrefab;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputReader>().AsSingle();

            InctallFactories();
        }

        private void InctallFactories()
        {
            Container.BindFactory<Bullet, Bullet.Factory>()

              .FromComponentInNewPrefab(m_BulletPrefab)

              .WithGameObjectName("Bullet")
              .UnderTransformGroup("Bullets");

            Container.BindFactory<BodyCellNode, BodyCellNode.Factory>()

            .FromComponentInNewPrefab(m_BodyCellPrefab)

            .WithGameObjectName("BodyCell")
            .UnderTransformGroup("BodyCells");
        }
    }

}