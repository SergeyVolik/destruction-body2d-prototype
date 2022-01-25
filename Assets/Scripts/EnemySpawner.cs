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

        private PositionAndEnemy[] m_Enemies;
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


            m_Enemies = new PositionAndEnemy[m_SpawnPoints.Count];

            for (int i = 0; i < m_SpawnPoints.Count; i++)
            {
                m_Enemies[i] = new PositionAndEnemy
                {
                    SpawnPoint = m_SpawnPoints[i]
                };

                //SpawnEnemy();
            }

        }
     
        public void SpawnEnemy()
        {
            if (m_PreloadedEnemies.Count > 0)
            {
                for (int i = 0; i < m_Enemies.Length; i++)
                {
                    if (!m_Enemies[i].Enemy)
                    {
                        var enemy = m_PreloadedEnemies[0];
                        m_PreloadedEnemies.Remove(enemy);
                        enemy.gameObject.SetActive(true);
                        enemy.transform.position = m_Enemies[i].SpawnPoint.position;
                        m_Enemies[i].Enemy = enemy;
                        return;
                    }
                }
            }          

        }

    }
}
