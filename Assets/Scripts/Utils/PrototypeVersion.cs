using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeVersion : MonoBehaviour
{
    private string m_Version;
    [SerializeField] private string m_Description;

    [SerializeField] private GUISkin m_Skin;
    void Start()
    {
        m_Version =  $"version: {Application.version} ({m_Description})" ;

        DontDestroyOnLoad(gameObject);
    }

    private void OnGUI()
    {
        GUI.skin = m_Skin;
        GUI.Label(new Rect(20, 40, 1000, 1000), m_Version);
    }
}
