using UnityEngine;

namespace Prototype
{
    public class HealthHandler : MonoBehaviour
    {
        private int m_Health;
        private int m_MaxHealth;
        private bool m_IsDead;
      
        public int Health => m_Health;
        public int MaxHealth => m_MaxHealth;
        public bool IsDead => m_IsDead;

        public void Init(int MaxHealth)
        {
            m_Health = MaxHealth;
            m_MaxHealth = MaxHealth;
        }

        public void ApplyDamage(int damage)
        {
            m_Health -= damage;

            if (m_Health <= 0)
            {
                m_IsDead = true;
                m_Health = 0;
            }
                
        }

    }
}
