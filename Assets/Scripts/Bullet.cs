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

        public float speed;
        public void Push(Vector2 vector)
        {
            m_RB.AddForce(vector);
        }

        [Inject]
        void Construct(ShootingSettings settings, Rigidbody2D rb)
        {
            m_RB = rb;
            m_Settings = settings;
        }

        bool dead = false;
        private void OnCollisionEnter2D(Collision2D collision)
        {
            
            if (!dead && collision.collider.TryGetComponent<IDamageable>(out var damagable))
            {
                dead = true;
                Debug.Log(m_RB.velocity.magnitude);
                damagable.ApplyDamage((int)(speed * m_Settings.bulletDamageMult));
                Destroy(gameObject);

            }

           
        }

        public class Factory : PlaceholderFactory<Bullet> { }
    }
}
