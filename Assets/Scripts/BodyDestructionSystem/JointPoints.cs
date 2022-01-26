using System;
using UnityEngine;

namespace Prototype
{

    [Serializable]
    public class JointPoints
    {
        
        [SerializeField] private BodyCellNode[] m_Points;
        [SerializeField] private HingeJoint2D m_ConnectedJoint;

        Rigidbody2D m_SavedConnectedBody;

        void Awake()
        {
            m_SavedConnectedBody = m_ConnectedJoint.connectedBody;
        }

        public bool CheckJointConnection()
        {
            int numberOfDeadPoints = 0;

            for (int i = 0; i < m_Points.Length; i++)
            {
                if (m_Points[i].Health.IsDead)
                    numberOfDeadPoints++;
            }

            var ration = (float)numberOfDeadPoints / m_Points.Length;

            if (ration > 0.5)
            {              
                return true;
            }

            return false;
        }

        public void DisconnectJoint()
        {
            m_ConnectedJoint.connectedBody = null;
            m_ConnectedJoint.enabled = false;
        }


    }
}
