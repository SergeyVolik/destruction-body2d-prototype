using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTargetFrameRate : MonoBehaviour
{

    [SerializeField] private int m_Value = 60; 
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = m_Value;
    }

}
