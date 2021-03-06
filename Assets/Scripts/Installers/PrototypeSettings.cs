using UnityEngine;
using Zenject;
using System;

namespace Prototype
{
    //[CreateAssetMenu(menuName = "Prototype/Settings")]
    public class PrototypeSettings : ScriptableObjectInstaller<PrototypeSettings>
    {

        [SerializeField] private CellsSettings m_CellsSettings;
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
            Container.BindInstance(m_PistolSettings);
            Container.BindInstance(m_CannonSettings);
            Container.BindInstance(m_LaserSettings);
            Container.BindInstance(m_GrenadeLauncherSettings);
        }
    }


    [Serializable]
    public class CellsSettings
    {

        public Settings settings;

        [Serializable]
        public struct Settings
        {
            public Color maxHealthColor;
            public Color minHealthColor;

            public int maxHealth;
            public float cellSize;
            public float pushCellForce;
        }
    }


    [Serializable]
    public abstract class GunData
    {
        public int damage;
        public float projectileSpeed;
    }

    

    [Serializable]
    public class PistolSettings : GunData
    {

    }

    [Serializable]
    public class CannonSettings : GunData
    {

    }

    [Serializable]
    public class LaserSettings : GunData
    {

    }

    [Serializable]
    public class GrenadeLauncherSettings : GunData
    {

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
