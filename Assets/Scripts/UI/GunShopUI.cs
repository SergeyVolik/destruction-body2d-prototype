using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Prototype
{
    public class GunShopUI : MonoBehaviour
    {
        [SerializeField] private Button m_SelectPistolButton;
        [SerializeField] private Button m_SelectLaserButton;
        [SerializeField] private Button m_SelectRocketLauncherButton;
        [SerializeField] private Button m_SelectCannonButton;

        [SerializeField] private TMPro.TMP_Text m_CurrentGunText;

        private PlayerInventory m_Inventory;
        [Inject]
        void Construct(PlayerInventory inventory)
        {
            m_Inventory = inventory;
        }


        private void OnEnable()
        {
            m_SelectCannonButton.onClick.AddListener(SelectCannon_ClickEvent);

            m_SelectLaserButton.onClick.AddListener(SelectLaser_ClickEvent);
            m_SelectPistolButton .onClick.AddListener(SelectPistol_ClickEvent);
            m_SelectRocketLauncherButton.onClick.AddListener(SelectRocketLauncher_ClickEvent);

        }

        private void OnDisable()
        {
            m_SelectCannonButton.onClick.RemoveListener(SelectCannon_ClickEvent);
            m_SelectLaserButton.onClick.RemoveListener(SelectLaser_ClickEvent);
            m_SelectPistolButton.onClick.RemoveListener(SelectPistol_ClickEvent);
            m_SelectRocketLauncherButton.onClick.RemoveListener(SelectRocketLauncher_ClickEvent);

        }

        void SelectCannon_ClickEvent()
        {
            m_Inventory.CurrentGun = GunType.Cannon;
            UpdateText();
        }

        void SelectPistol_ClickEvent()
        {
            m_Inventory.CurrentGun = GunType.Pistol;
            UpdateText();
        }

        private void UpdateText()
        {
            m_CurrentGunText.text = m_Inventory.CurrentGun.ToString();
        }

        void SelectLaser_ClickEvent()
        {
            m_Inventory.CurrentGun = GunType.Laser;
            UpdateText();

        }
        void SelectRocketLauncher_ClickEvent()
        {
            m_Inventory.CurrentGun = GunType.RocketLauncher;
            UpdateText();

        }
    }

}
