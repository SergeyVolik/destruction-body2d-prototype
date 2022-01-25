using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Prototype
{

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public class OpenSceneButtonEvent : MonoBehaviour
    {
       
        [SerializeField] private Scenes m_Scene;

        private Button m_Button;

        private void Awake()
        {
            m_Button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            m_Button.onClick.AddListener(ChangeScene);
        }

        private void OnDisable()
        {
            m_Button.onClick.RemoveListener(ChangeScene);
        }

        private void ChangeScene()
        {
            SceneManager.LoadScene((int)m_Scene);
        }
    }

}
