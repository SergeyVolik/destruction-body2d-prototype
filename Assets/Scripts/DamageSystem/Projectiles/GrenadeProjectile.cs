﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Prototype
{
    public class GrenadeProjectile : Projectile2D
    {
        private GrenadeProjectile.Pool m_Pool;
        [Inject]
        void Construct(GrenadeProjectile.Pool pool)
        {
            m_Pool = pool;
        }

        private void Awake()
        {
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

        public class Pool : MemoryPool<GrenadeProjectile>
        {
            protected override void OnCreated(GrenadeProjectile bullet)
            {
                bullet.gameObject.SetActive(false);
                // Called immediately after the item is first added to the pool
            }

            protected override void OnDestroyed(GrenadeProjectile bullet)
            {
                // Called immediately after the item is removed from the pool without also being spawned
                // This occurs when the pool is shrunk either by using WithMaxSize or by explicitly shrinking the pool by calling the `ShrinkBy` / `Resize methods
            }

            protected override void OnSpawned(GrenadeProjectile bullet)
            {
                bullet.gameObject.SetActive(true);

                // Called immediately after the item is removed from the pool
            }

            protected override void OnDespawned(GrenadeProjectile bullet)
            {
                bullet.gameObject.SetActive(false);
                bullet.m_RB.velocity = Vector3.zero;
                bullet.m_RB.angularVelocity = 0;
                // Called immediately after the item is returned to the pool
            }

            protected override void Reinitialize(GrenadeProjectile bullet)
            {

            }
        }
    }
}