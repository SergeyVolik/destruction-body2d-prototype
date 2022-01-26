using System;
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
        private GrenadeLauncherSettings m_Settings;
        private ExplosionVFX.Pool m_ExplosionPool;

        public GrenadeLauncherSettings Settings => m_Settings;

        [Inject]
        void Construct(GrenadeProjectile.Pool pool, GrenadeLauncherSettings settings, ExplosionVFX.Pool explosionPool)
        {
            m_Pool = pool;
            m_Settings = settings;
            m_ExplosionPool = explosionPool;
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

        private bool Exploded = false;
        protected override void OnEnable()
        {
            base.OnEnable();
            Exploded = false;
        }
        public void Explode()
        {
            if (!Exploded)
            {

                Exploded = true;

              
                var position = m_Transform.position;
                
                var colliders = Physics2D.OverlapCircleAll(position, m_Settings.explosionRange);

                for (int i = 0; i < colliders.Length; i++)
                {

                    if (colliders[i].TryGetComponent<IDamageable>(out var damageable))
                    {
                       
                        damageable.ApplyDamage(m_Settings.damage, position);
                    }
                }

                colliders = Physics2D.OverlapCircleAll(position, m_Settings.explosionRange);

                for (int i = 0; i < colliders.Length; i++)
                {

                    if (colliders[i].TryGetComponent<IDamageable>(out var damageable))
                    {

                        damageable.ApplyDamage(m_Settings.damage, position);
                    }
                }


                var particle = m_ExplosionPool.Spawn();
                particle.transform.position = position;
                particle.Play();

                Despawn();
                StopAllCoroutines();
            }
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
