using System;
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

        public event Action OnActivated;

        [Inject]
        void Construct(RagdollSettings settings)
        {
            m_Settings = settings;
        }


        bool Activated;
        public void Activate()
        {
            if (Activated)
                return;

            OnActivated?.Invoke();

            Activated = true;

            if (m_Animator)
                m_Animator.enabled = false;

            for (int i = 0; i < m_BodyParts.Length; i++)
            {
                m_BodyParts[i].Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                if (m_BodyParts[i].Joint && m_BodyParts[i].Joint.connectedBody)
                    m_BodyParts[i].Joint.enabled = true;
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

