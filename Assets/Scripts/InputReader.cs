using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Prototype
{
    public class InputReader : ITickable, IInitializable
    {
        private Vector3 m_InputPos;

        public Vector3 InputPos => m_InputPos;

        public event Action OnShoot;

        Camera m_Camera;
        public void Initialize()
        {
            m_Camera = Camera.main;
        }

        public void Tick()
        {
#if UNITY_EDITOR
           

            if (Input.GetMouseButtonDown(0))
            {
                m_InputPos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
                OnShoot?.Invoke();
                
            }

          
            

#elif UNITY_ANDROID || UNITY_IOS
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    m_InputPos = Camera.main.ScreenToWorldPoint(touch.position);
                    OnShoot?.Invoke();

                }
            }
#endif
        }
    }
}
