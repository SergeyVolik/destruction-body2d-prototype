using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Prototype
{
    public class ExplosionVFX : MonoBehaviour
    {
        private ParticleSystem m_ParticleSystem;
        private Pool m_Pool;

        [Inject]
        void Construct(ExplosionVFX.Pool pool)
        {
            m_Pool = pool;
        }
        private void Awake()
        {
            m_ParticleSystem = GetComponent<ParticleSystem>();
        }

        public void Play()
        {
            Debug.Log("Play Explosion Effect");
            m_ParticleSystem.Play();
            StartCoroutine(WaitEndOfParticleAndReturnToPool());
        }

        IEnumerator WaitEndOfParticleAndReturnToPool()
        {
            yield return new WaitForSeconds(m_ParticleSystem.main.duration);
            m_Pool.Despawn(this);
        }

        public class Pool : MemoryPool<ExplosionVFX> 
        {
            protected override void OnCreated(ExplosionVFX bullet)
            {
                // Called immediately after the item is first added to the pool
            }

            protected override void OnDestroyed(ExplosionVFX bullet)
            {
                // Called immediately after the item is removed from the pool without also being spawned
                // This occurs when the pool is shrunk either by using WithMaxSize or by explicitly shrinking the pool by calling the `ShrinkBy` / `Resize methods
            }

            protected override void OnSpawned(ExplosionVFX bullet)
            {

                // Called immediately after the item is removed from the pool
            }

            protected override void OnDespawned(ExplosionVFX bullet)
            {
                // Called immediately after the item is returned to the pool
            }

            protected override void Reinitialize(ExplosionVFX bullet)
            {

            }
        }
    }

}

