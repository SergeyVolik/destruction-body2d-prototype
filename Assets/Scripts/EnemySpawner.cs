using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Prototype
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Transform m_SpawnPoint1;
        [SerializeField] private Transform m_SpawnPoint2;
        [SerializeField] private Transform m_SpawnPoint3;


        RagdollModel enemy1;
        RagdollModel enemy2;
        RagdollModel enemy3;

        RagdollModel.Factory m_RagdollFactory;
        [Inject]
        void Construct(RagdollModel.Factory ragdollFactory)
        {
            m_RagdollFactory = ragdollFactory;
        }
        public void SpawnEnemy()
        {
            if (!enemy1)
            {
                enemy1 = m_RagdollFactory.Create();
                enemy1.transform.position = m_SpawnPoint1.position;
            }
            else if (!enemy2)
            {
                enemy2 = m_RagdollFactory.Create();
                enemy2.transform.position = m_SpawnPoint2.position;
            }
            else if (!enemy3)
            {
                enemy3 = m_RagdollFactory.Create();
                enemy3.transform.position = m_SpawnPoint3.position;
            }

        }

    }
}
