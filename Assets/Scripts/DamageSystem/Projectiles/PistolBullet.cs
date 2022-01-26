using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Prototype
{
    public class PistolBullet : Projectile2D
    {
        private PistolBullet.Pool m_Pool;
        private PistolSettings m_Settings;

        public PistolSettings Settings => m_Settings;
        [Inject]
        void Construct(PistolBullet.Pool pool, PistolSettings settings)
        {
            m_Pool = pool;
            m_Settings = settings;
        }

        protected override void Awake()
        {
            base.Awake();
            m_DespawnTime = 8f;
        }
        protected override void Despawn()
        {
            m_Pool.Despawn(this);
        }
        protected override void Accept(IProjectile2DVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Stoped = false;
            currentSlow = 0;
        }
        bool Stoped = false;

        private int CellsToStop = 5;
        private int currentSlow = 0;
        public void SlowBullet()
        {
            currentSlow++;
            if (currentSlow >= CellsToStop && !Stoped)
            {
                Stoped = true;
                StopAllCoroutines();
                Despawn();
            }
        }
        public class Pool : MemoryPool<PistolBullet>
        {
            protected override void OnCreated(PistolBullet bullet)
            {
                bullet.gameObject.SetActive(false);
                // Called immediately after the item is first added to the pool
            }

            protected override void OnDestroyed(PistolBullet bullet)
            {
                // Called immediately after the item is removed from the pool without also being spawned
                // This occurs when the pool is shrunk either by using WithMaxSize or by explicitly shrinking the pool by calling the `ShrinkBy` / `Resize methods
            }

            protected override void OnSpawned(PistolBullet bullet)
            {
                bullet.gameObject.SetActive(true);

                // Called immediately after the item is removed from the pool
            }

            protected override void OnDespawned(PistolBullet bullet)
            {
                bullet.gameObject.SetActive(false);
                bullet.m_RB.velocity = Vector3.zero;
                bullet.m_RB.angularVelocity = 0;
                // Called immediately after the item is returned to the pool
            }

            protected override void Reinitialize(PistolBullet bullet)
            {

            }
        }
    }
}
