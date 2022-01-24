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
        public float speed;
        public void Push(Vector2 vector)
        {
            m_RB.AddForce(vector, ForceMode2D.Impulse);
        }

        [Inject]
        void Construct(ShootingSettings settings, Rigidbody2D rb, SlowmotionManager timeManager)
        {
            m_RB = rb;
            m_Settings = settings;
            m_TimeManager = timeManager;
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IDamageable>(out var damagable))
            {
                damagable.ApplyDamage((int)(speed * m_Settings.bulletDamageMult), transform.position);
                m_TimeManager.DoSlowmotion();
            }
        }

        public class Factory : PlaceholderFactory<Bullet> { }
    }
}
