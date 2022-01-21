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

        [SerializeField] private int preloadedEnemies = 10;

        RagdollModel enemy1;
        RagdollModel enemy2;
        RagdollModel enemy3;

        RagdollModel.Factory m_RagdollFactory;
        [Inject]
        void Construct(RagdollModel.Factory ragdollFactory)
        {
            m_RagdollFactory = ragdollFactory;

          
        }

        private void Start()
        {
            for (int i = 0; i < preloadedEnemies; i++)
            {
                var enemy = m_RagdollFactory.Create();
                enemy.gameObject.SetActive(false);
                preloaded.Add(enemy);
            }

            SpawnEnemy();
            SpawnEnemy();
            SpawnEnemy();
        }
        List<RagdollModel> preloaded = new List<RagdollModel>();
        public void SpawnEnemy()
        {
            if (preloaded.Count > 0)
            {
                var enemy = preloaded[0];
                if (!enemy1)
                {
                    enemy1 = enemy;
                    preloaded.Remove(enemy1);
                    enemy1.gameObject.SetActive(true);
                    enemy1.transform.position = m_SpawnPoint1.position;
                }
                else if (!enemy2)
                {
                    enemy2 = enemy;
                    preloaded.Remove(enemy2);
                    enemy2.gameObject.SetActive(true);
                    enemy2.transform.position = m_SpawnPoint2.position;
                }
                else if (!enemy3)
                {
                    enemy3 = enemy;
                    preloaded.Remove(enemy3);
                    enemy3.gameObject.SetActive(true);
                    enemy3.transform.position = m_SpawnPoint3.position;
                }
            }

        }

    }
}
