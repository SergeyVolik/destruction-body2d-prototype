using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Prototype
{
    public class LaserProjectile : Projectile2D
    {
        private LaserProjectile.Pool m_Pool;
        private LaserSettings m_Settings;

        public LaserSettings Settings => m_Settings;

        [Inject]
        void Construct(LaserProjectile.Pool pool, LaserSettings settings)
        {
            m_Pool = pool;
            m_Settings = settings;
        }

        protected override void Awake()
        {
            base.Awake();
            m_DespawnTime = 0.05f;
        }
        protected override void Despawn()
        {
            m_Pool.Despawn(this);
        }

        protected override void Accept(IProjectile2DVisitor visitor)
        {
            visitor.Visit(this);
        }


        public class Pool : MemoryPool<LaserProjectile>
        {
            protected override void OnCreated(LaserProjectile bullet)
            {
                bullet.gameObject.SetActive(false);
                // Called immediately after the item is first added to the pool
            }

            protected override void OnDestroyed(LaserProjectile bullet)
            {
                // Called immediately after the item is removed from the pool without also being spawned
                // This occurs when the pool is shrunk either by using WithMaxSize or by explicitly shrinking the pool by calling the `ShrinkBy` / `Resize methods
            }

            protected override void OnSpawned(LaserProjectile bullet)
            {
                bullet.gameObject.SetActive(true);

                // Called immediately after the item is removed from the pool
            }

            protected override void OnDespawned(LaserProjectile bullet)
            {
                bullet.gameObject.SetActive(false);
                bullet.m_RB.velocity = Vector3.zero;
                bullet.m_RB.angularVelocity = 0;
                // Called immediately after the item is returned to the pool
            }

            protected override void Reinitialize(LaserProjectile bullet)
            {

            }
        }
    }
}
