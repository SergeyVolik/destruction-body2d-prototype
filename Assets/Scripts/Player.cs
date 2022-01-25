using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Prototype
{
    public class Player : MonoBehaviour
    {
        private PlayerInventory m_Inventory;

        [SerializeField] private GameObject m_Pistol;
        [SerializeField] private GameObject m_Laser;
        [SerializeField] private GameObject m_RocketLauncher;
        [SerializeField] private GameObject m_Cannon;
        [Inject]
        void Construct(PlayerInventory inventory)
        {
            m_Inventory = inventory;
        }

        private void Start()
        {
            ActivateCurrentGun();
        }

        private void ActivateCurrentGun()
        {
            switch (m_Inventory.CurrentGun)
            {
                case Gun.Pistol:
                    m_Pistol.SetActive(true);
                    break;
                case Gun.Laser:
                    m_Laser.SetActive(true);
                    break;
                case Gun.RocketLauncher:
                    m_RocketLauncher.SetActive(true);
                    break;
                case Gun.Cannon:
                    m_Cannon.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }

}
