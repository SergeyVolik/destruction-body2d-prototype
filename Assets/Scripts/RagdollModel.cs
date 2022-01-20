using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Prototype
{
    public class RagdollModel : MonoBehaviour
    {
        [SerializeField] private Animator m_Animator;
        [SerializeField] private BodyPart[] m_BodyParts;


        private RagdollSettings m_Settings;
        public RagdollSettings Settings => m_Settings;
        [Inject]
        void Construct(RagdollSettings settings)
        {
            m_Settings = settings;
        }
        private void Start()
        {
            for (int i = 0; i < m_BodyParts.Length; i++)
            {
                m_BodyParts[i].Rigidbody2D.mass = 2;
                m_BodyParts[i].Rigidbody2D.gravityScale = 4;
            }
        }
        public void Activate()
        {
            if(m_Animator)
                m_Animator.enabled = false;

            for (int i = 0; i < m_BodyParts.Length; i++)
            {
                m_BodyParts[i].Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            }
            StartCoroutine(DestroyWithTime());
        }

        IEnumerator DestroyWithTime()
        {
            yield return new WaitForSeconds(5f);
            Destroy(gameObject);
        }

        public class Factory : PlaceholderFactory<RagdollModel> { }
    }

}

