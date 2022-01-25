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

        protected virtual void Start()
        {
            m_RB = GetComponent<Rigidbody2D>();
            m_Transform = transform;
        }

        public void Push(Vector2 vector)
        {
            m_RB.AddForce(vector, ForceMode2D.Impulse);
        }

        protected abstract void Accept(IProjectile2DVisitor visitor);

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IProjectile2DVisitor>(out var visitor))
            {
                Accept(visitor);
            }
        }
       
    }
}
