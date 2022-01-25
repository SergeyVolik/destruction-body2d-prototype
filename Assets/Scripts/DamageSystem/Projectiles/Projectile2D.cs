using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Prototype
{


    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class Projectile2D : MonoBehaviour
    {
        protected Rigidbody2D m_RB;
        protected Transform m_Transform;

        protected float m_DespawnTime = 0.1f;

        private IEnumerator Despawn_CO()
        {
            yield return new WaitForSeconds(m_DespawnTime);
            Despawn();
        }

        protected virtual void OnEnable()
        {
            StartCoroutine(Despawn_CO());
        }
   

        protected virtual void Start()
        {
            m_RB = GetComponent<Rigidbody2D>();
            m_Transform = transform;
        }

        public void Push(Vector2 vector)
        {
            m_RB.AddForce(vector, ForceMode2D.Impulse);
        }

      

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IProjectile2DVisitor>(out var visitor))
            {
                Accept(visitor);
            }
        }

        protected abstract void Despawn();
        protected abstract void Accept(IProjectile2DVisitor visitor);

    }
}
