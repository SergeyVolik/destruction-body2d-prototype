using UnityEngine;
using Zenject;

namespace Prototype
{
    public class Player : MonoBehaviour
    {
        private PlayerInventory m_Inventory;
        private InputReader m_InputReader;
        private Transform m_HandTrasform;

        [SerializeField] private Pistol m_Pistol;
        [SerializeField] private Laser m_Laser;
        [SerializeField] private GrenadeLauncher m_GrenadeLauncher;
        [SerializeField] private Cannon m_Cannon;
        [SerializeField] private Transform m_Hand;

        private IGun m_CurrentGun;
        
        [Inject]
        void Construct(PlayerInventory inventory, InputReader inputReader)
        {
            m_Inventory = inventory;
            m_InputReader = inputReader;
        }


        private void Start()
        {
            ActivateCurrentGun();
            m_HandTrasform = m_Hand.transform;
        }

        private void OnEnable()
        {
            m_InputReader.OnShoot += OnShoot;
        }

        private void OnDisable()
        {
            m_InputReader.OnShoot -= OnShoot;
        }

        private void OnShoot()
        {
            var vector = m_InputReader.InputPos - m_HandTrasform.position;
            m_HandTrasform.rotation = MathfExtention.LookAt2D(vector);

            m_CurrentGun.Shot();


        }

        private void ActivateCurrentGun()
        {
            switch (m_Inventory.CurrentGun)
            {
                case GunType.Pistol:
                    m_CurrentGun = m_Pistol;
                    m_Pistol.gameObject.SetActive(true);
                    break;
                case GunType.Laser:
                    m_CurrentGun = m_Laser;
                    m_Laser.gameObject.SetActive(true);
                    break;
                case GunType.RocketLauncher:
                    m_CurrentGun = m_GrenadeLauncher;
                    m_GrenadeLauncher.gameObject.SetActive(true);
                    break;
                case GunType.Cannon:
                    m_CurrentGun = m_Cannon;
                    m_Cannon.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }

}
