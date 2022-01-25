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
        private void Start()
        {
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

    }
}
