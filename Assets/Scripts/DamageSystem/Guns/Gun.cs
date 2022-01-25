using UnityEngine;

namespace Prototype
{
    public abstract class Gun : MonoBehaviour, IGun
    {
        [SerializeField] protected Transform m_BulletSpawnPoint;

        protected Transform m_Trasform;

        protected virtual void Awake()
        {
            m_Trasform = transform;
        }

        public abstract void Shot();
    }
}
