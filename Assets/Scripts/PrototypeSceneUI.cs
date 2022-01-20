using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Prototype
{
    public class PrototypeSceneUI : MonoBehaviour
    {
        [SerializeField] private Button m_ReloadSceneButton;
        [SerializeField] private Button m_SpawnEnemyButton;
        [SerializeField] private Toggle m_ToggleSlowmotion;

        private SlowmotionManager m_TimeManager;
        private EnemySpawner m_Spawner;
       [Inject]
        private void Construct(SlowmotionManager timeManager, EnemySpawner spawner)
        {
            m_TimeManager = timeManager;
            m_Spawner = spawner;
        }

        private void OnEnable()
        {
            m_ReloadSceneButton.onClick.AddListener(ReloadScene);
            m_SpawnEnemyButton.onClick.AddListener(SpawnEnemy);
            m_ToggleSlowmotion.onValueChanged.AddListener(OnToggleSlowmotion);

            OnToggleSlowmotion(m_ToggleSlowmotion.isOn);
        }

        private void OnDisable()
        {         
            m_ReloadSceneButton.onClick.RemoveListener(ReloadScene);
            m_SpawnEnemyButton.onClick.RemoveListener(SpawnEnemy);
            m_ToggleSlowmotion.onValueChanged.RemoveListener(OnToggleSlowmotion);
        }

        void ReloadScene()
        {
            SceneManager.LoadScene(0);
        }

        void SpawnEnemy()
        {
            m_Spawner.SpawnEnemy();
        }

        void OnToggleSlowmotion(bool value)
        {
            if (value)
                m_TimeManager.Enable();
            else m_TimeManager.Disable();
        }

    }

}
