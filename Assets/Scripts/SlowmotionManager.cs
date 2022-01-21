using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Prototype
{
    public class SlowmotionManager : MonoBehaviour
    {

        private float m_DefaultTimeScale = 1f;
        private float m_DefaultFixedDeltaTime = 1f;
        private float m_T;


       
        SlowmotionSettings m_Settings;

        [Inject]
        void Construct(SlowmotionSettings settings)
        {
            m_Settings = settings;
        }

        public void Enable()
        {
            enabled = true;
        }

        public void Disable()
        {
            enabled = false;
        }

        private void Awake()
        {
            m_DefaultFixedDeltaTime = Time.fixedDeltaTime;
            m_DefaultTimeScale = Time.timeScale;

        }

        private void Update()
        {
            if (m_T > m_Settings.slowdownTime)
            {
                Time.timeScale += (1f / m_Settings.slowdownLength) * Time.unscaledDeltaTime;
                Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
                Time.fixedDeltaTime = m_DefaultFixedDeltaTime * Time.timeScale;
            }

            m_T += Time.unscaledDeltaTime;

        }


        public void DoSlowmotion()
        {
            if (enabled)
            {                
                Time.timeScale = m_Settings.slowdownFactor;
                Time.fixedDeltaTime = m_DefaultFixedDeltaTime * Time.timeScale;
                m_T = 0;
            }
        }

        private void OnDisable()
        {
            Time.fixedDeltaTime = m_DefaultFixedDeltaTime;
            Time.timeScale = m_DefaultTimeScale;

        }
    }

}
