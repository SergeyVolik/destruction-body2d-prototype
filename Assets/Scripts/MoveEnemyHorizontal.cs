using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
    [RequireComponent(typeof(RagdollModel))]
    public class MoveEnemyHorizontal : MonoBehaviour
    {
        [SerializeField] private float m_Speed = 10;
        [SerializeField] private bool m_MoveLeft = false;
        private bool IsMoving = true;

        private Transform m_Transform;

        RagdollModel m_RagdollModel;
        private void Awake()
        {
            m_Transform = transform;

            m_RagdollModel = GetComponent<RagdollModel>();
        }
        private void Update()
        {
            if (IsMoving)
            {
                Move();
            }
        }

        private void OnEnable()
        {
            m_RagdollModel.OnActivated += StopMove;
        }

        private void OnDisable()
        {
            m_RagdollModel.OnActivated -= StopMove;

        }

        void StopMove()
        {
            IsMoving = false;
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
