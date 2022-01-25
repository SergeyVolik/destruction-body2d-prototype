using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Prototype
{


    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour
    {
        private Rigidbody2D m_RB;
        private ShootingSettings m_Settings;
        private SlowmotionManager m_TimeManager;
        private Pool m_Pool;
        public float speed;

        private IEnumerator Despawn()
        {
            yield return null;
            yield return null;
            m_Pool.Despawn(this);
        }

        private void OnEnable()
        {
            StartCoroutine(Despawn());
        }
        public void Push(Vector2 vector)
        {
            //m_RB.AddForce(vector, ForceMode2D.Impulse);
        }

        [Inject]
        void Construct(ShootingSettings settings, Rigidbody2D rb, SlowmotionManager timeManager, Bullet.Pool pool)
        {
            m_RB = rb;
            m_Settings = settings;
            m_TimeManager = timeManager;

            m_Pool = pool;
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IDamageable>(out var damagable))
            {
                damagable.ApplyDamage((int)(speed * m_Settings.bulletDamageMult), transform.position);
                m_TimeManager.DoSlowmotion();
            }
        }

        void ResetBulletData()
        {
            
        }

        public class Pool : MemoryPool<Bullet> 
        {
            protected override void OnCreated(Bullet bullet)
            {
                bullet.gameObject.SetActive(false);
                // Called immediately after the item is first added to the pool
            }

            protected override void OnDestroyed(Bullet bullet)
            {
                // Called immediately after the item is removed from the pool without also being spawned
                // This occurs when the pool is shrunk either by using WithMaxSize or by explicitly shrinking the pool by calling the `ShrinkBy` / `Resize methods
            }

            protected override void OnSpawned(Bullet bullet)
            {
                bullet.gameObject.SetActive(true);
               
                // Called immediately after the item is removed from the pool
            }

            protected override void OnDespawned(Bullet bullet)
            {
                bullet.gameObject.SetActive(false);
                bullet.m_RB.velocity = Vector3.zero;
                bullet.m_RB.angularVelocity = 0;
                // Called immediately after the item is returned to the pool
            }

            protected override void Reinitialize(Bullet bullet)
            {
                bullet.ResetBulletData();
                // Similar to OnSpawned
                // Called immediately after the item is removed from the pool
                // This method will also contain any parameters that are passed along
                // to the memory pool from the spawning code
            }
        }
    }
}
