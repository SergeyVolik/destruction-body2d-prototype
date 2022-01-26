using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
    public class MoveEnemyHorizontal : MonoBehaviour
    {
        [SerializeField] private float m_Speed = 10;
        [SerializeField] private bool m_MoveLeft = false;

        private Transform m_Transform;

        private void Awake()
        {
            m_Transform = transform;
        }
        private void Update()
        {
            Move();
        }

        private void Move()
        {
            var position = m_Transform.position;

            float offset = Time.deltaTime * m_Speed;

            if (m_MoveLeft)
            {
                offset *= -1;
            }

            position = new Vector3(position.x + offset, position.y, position.z);

            m_Transform.position = position;
        }
    }
}
