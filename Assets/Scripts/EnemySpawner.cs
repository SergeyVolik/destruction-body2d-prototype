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
        private class PositionAndEnemy
        {
            public Transform SpawnPoint;
            public RagdollModel Enemy;
        }

        [SerializeField] private List<Transform> m_SpawnPoints;
        [SerializeField] private int m_PreloadEnemiesNumber = 10;

        RagdollModel.Factory m_RagdollFactory;

        List<RagdollModel> m_PreloadedEnemies = new List<RagdollModel>();

        [Inject]
        void Construct(RagdollModel.Factory ragdollFactory)
        {
            m_RagdollFactory = ragdollFactory;
        }

        private void Start()
        {
            for (int i = 0; i < m_PreloadEnemiesNumber; i++)
            {
                var enemy = m_RagdollFactory.Create();
                enemy.gameObject.SetActive(false);
                m_PreloadedEnemies.Add(enemy);
            }


            for (int i = 0; i < m_SpawnPoints.Count; i++)
            {
                SpawnEnemyInternal(m_SpawnPoints[i].position);
            }

        }

        public void SpawnEnemy()
        {
            var spawnPoint = m_SpawnPoints[UnityEngine.Random.Range(0, m_SpawnPoints.Count)];
            SpawnEnemyInternal(spawnPoint.position);

        }

        private void SpawnEnemyInternal(Vector3 spawnPos)
        {
            if (m_PreloadedEnemies.Count > 0)
            {
                var enemy = m_PreloadedEnemies[0];
                m_PreloadedEnemies.Remove(enemy);
                enemy.gameObject.SetActive(true);
                enemy.transform.position = spawnPos;
            }
        }

    }
}
