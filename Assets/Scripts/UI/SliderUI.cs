using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Prototype
{
    public class SliderUI : MonoBehaviour
    {
        [SerializeField] private Button m_NextSlide;
        [SerializeField] private Button m_PrevSlide;

        [SerializeField] private RectTransform[] m_Slides;

        private int m_CurrentSlideIndex = 0;

        private void Start()
        {
            for (int i = 0; i < m_Slides.Length; i++)
            {
                if (i == m_CurrentSlideIndex)
                {
                    m_Slides[i].gameObject.SetActive(true);
                }
                else
                {
                    m_Slides[i].gameObject.SetActive(false);
                }

            }    
        }

        private void OnEnable()
        {
            m_NextSlide.onClick.AddListener(NextSlide);
            m_PrevSlide.onClick.AddListener(PrevSlide);

        }

        private void OnDisable()
        {

            m_NextSlide.onClick.RemoveListener(NextSlide);
            m_PrevSlide.onClick.RemoveListener(PrevSlide);
        }


        private void NextSlide()
        {
            m_Slides[m_CurrentSlideIndex].gameObject.SetActive(false);
            m_CurrentSlideIndex++;

            if (m_CurrentSlideIndex == m_Slides.Length)
            {
                m_CurrentSlideIndex = 0;
            }

            m_Slides[m_CurrentSlideIndex].gameObject.SetActive(true);
        }

        private void PrevSlide()
        {
            m_Slides[m_CurrentSlideIndex].gameObject.SetActive(false);
            m_CurrentSlideIndex--;

            if (m_CurrentSlideIndex == -1)
            {
                m_CurrentSlideIndex = 0;
            }

            m_Slides[m_CurrentSlideIndex].gameObject.SetActive(true);
        }


    }
}
