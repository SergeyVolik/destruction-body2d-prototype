
using UnityEngine;
using Zenject;

namespace Prototype
{

    public class ProjectInstaller : MonoInstaller
    {
      
        public override void InstallBindings()
        {
            Container.Bind<PlayerInventory>().FromNew().AsSingle();
        }

    }

}