using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Prototype
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BodySubPart : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D m_RB;
        [SerializeField] private HingeJoint2D[] m_ConnectTo;

        public Rigidbody2D Rigidbody2D => m_RB;

        Transform m_Transform;
        private TransformSavedData m_TransformSavedData;

        private void Awake()
        {
            m_Transform = transform;
            m_TransformSavedData = new TransformSavedData()
            {
                parent = m_Transform.parent,
                localPosition = m_Transform.localPosition,
                localRotation = m_Transform.localRotation,
                localScale = m_Transform.localScale

            };

            gameObject.SetActive(false);
        }
        public void ActivateAndConnectJoints()
        {
            gameObject.SetActive(true);
            transform.parent = null;
            m_RB.bodyType = RigidbodyType2D.Dynamic;

            for (int i = 0; i < m_ConnectTo.Length; i++)
            {
              
                m_ConnectTo[i].connectedBody = m_RB;
                var rb = m_ConnectTo[i].GetComponent<Rigidbody2D>();
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0;
            }

        }

        public void ResetValues()
        {
            m_TransformSavedData.ResetValues(m_Transform);
            Rigidbody2DUtils.ResetValues(m_RB);

        }

    }
}
