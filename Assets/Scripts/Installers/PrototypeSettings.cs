using UnityEngine;
using Zenject;
using System;

namespace Prototype
{
    //[CreateAssetMenu(menuName = "Prototype/Settings")]
    public class PrototypeSettings : ScriptableObjectInstaller<PrototypeSettings>
    {

        [SerializeField] private CellsSettings m_CellsSettings;
        [SerializeField] private ShootingSettings m_ShoottingSettings;
        [SerializeField] private SlowmotionSettings m_SlowmotionSettings;

        [SerializeField] private RagdollSettings m_RagdollSettings;
        public override void InstallBindings()
        {
            Container.BindInstance(m_CellsSettings);
            Container.BindInstance(m_ShoottingSettings);
            Container.BindInstance(m_SlowmotionSettings);
            Container.BindInstance(m_RagdollSettings);
        }
    }


    [Serializable]
    public class CellsSettings
    {
        public Color maxHealthColor;
        public Color minHealthColor;

        public int maxHealth;
        public float cellSize;
        public float pushCellForce = 1000;

    }

    [Serializable]
    public class ShootingSettings
    {
       
        public float bulletDamageMult = 10;
        public float bulletSpeedMax = 1000f;

        public float bulletSpeedMin = 1000f;
        public float delayBetweenShots = 0.1f;
    }

    [Serializable]
    public class SlowmotionSettings
    {
        public float slowdownFactor = 0.05f;
        public float slowdownLength = 2f;
        public float slowdownTime = 1f;
    }

    [Serializable]
    public class RagdollSettings
    {
        public float bodyPushForce = 1000;
    }
}
