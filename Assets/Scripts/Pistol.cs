using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Prototype
{
    public class Pistol : MonoBehaviour
    {
        private ShootingSettings m_Settings;
        private InputReader m_Input;
        private Bullet.Pool m_BulletFactory;

        [SerializeField] private Transform m_BulletSpawnPoint;

        private Transform m_Trasform;

        [Inject]
        void Construct(ShootingSettings settings, InputReader input, Bullet.Pool factory)
        {
            m_Settings = settings;
            m_Input = input;
            m_BulletFactory = factory;

        }

        private void Awake()
        {
            m_Trasform = transform;
        }

        private void OnEnable()
        {
            m_Input.OnShoot += OnShoot;
        } 

        private void OnDisable()
        {
            m_Input.OnShoot -= OnShoot;
        }

        private void OnShoot()
        {
            var vector = m_Input.InputPos - m_Trasform.position;
            var speed = Random.Range(m_Settings.bulletSpeedMin, m_Settings.bulletSpeedMax);
            m_Trasform.rotation = MathfExtention.LookAt2D(vector);
            var bullet = m_BulletFactory.Spawn();
            bullet.speed = speed;
            var trans = bullet.transform;
            trans.rotation = m_Trasform.rotation;
            trans.position = m_BulletSpawnPoint.position;
            bullet.Push(m_Trasform.right * speed);


        }



    }

}