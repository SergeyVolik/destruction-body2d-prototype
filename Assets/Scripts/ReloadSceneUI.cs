using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Prototype
{
    public class ReloadSceneUI : MonoBehaviour
    {
        [SerializeField] private Button m_Button;

        private void OnEnable()
        {
            m_Button.onClick.AddListener(ReloadScene);
        }

        private void OnDisable()
        {
            m_Button.onClick.RemoveListener(ReloadScene);
        }

        void ReloadScene()
        {
            SceneManager.LoadScene(0);
        }
    }

}
