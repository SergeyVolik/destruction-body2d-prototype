using UnityEngine;
using Zenject;
using System;

namespace Prototype
{
    //[CreateAssetMenu(menuName = "Prototype/Settings")]
    public class PrototypeSettings : ScriptableObjectInstaller<PrototypeSettings>
    {

        [SerializeField]
        private CellsSettings m_CellsSettings;
        [SerializeField]
        private ShootingSettings m_ShoottingSettings;
        public override void InstallBindings()
        {
            Container.BindInstance(m_CellsSettings);
            Container.BindInstance(m_ShoottingSettings);

        }
    }


    [Serializable]
    public class CellsSettings
    {
        public Color maxHealthColor;
        public Color minHealthColor;

        public int maxHealth;
        public float cellSize;

    }

    [Serializable]
    public class ShootingSettings
    {
       
        public float bulletDamageMult = 10;
        public float bulletSpeedMax = 1000f;

        public float bulletSpeedMin = 1000f;
        public float delayBetweenShots = 0.1f;
    }

}
