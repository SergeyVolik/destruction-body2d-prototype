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
        [SerializeField] private PistolSettings m_PistolSettings;
        [SerializeField] private CannonSettings m_CannonSettings;
        [SerializeField] private LaserSettings m_LaserSettings;
        [SerializeField] private GrenadeLauncherSettings m_GrenadeLauncherSettings;

        public override void InstallBindings()
        {
            Container.BindInstance(m_CellsSettings);
        
            Container.BindInstance(m_SlowmotionSettings);
            Container.BindInstance(m_RagdollSettings);

            InstallGunSettings();
        }

        private void InstallGunSettings()
        {
            Container.BindInstance(m_ShoottingSettings);
            Container.BindInstance(m_PistolSettings);
            Container.BindInstance(m_CannonSettings);
            Container.BindInstance(m_LaserSettings);
            Container.BindInstance(m_GrenadeLauncherSettings);
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
    public class PistolSettings
    {
        public float damage;
        public float projectileSpeed;
    }

    [Serializable]
    public class CannonSettings
    {
        public float damage;
        public float projectileSpeed;
    }

    [Serializable]
    public class LaserSettings
    {
        public float damage;
        public float projectileSpeed;
    }

    [Serializable]
    public class GrenadeLauncherSettings
    {
        public float damage;
        public float projectileSpeed;
        public float explosionRange;
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
