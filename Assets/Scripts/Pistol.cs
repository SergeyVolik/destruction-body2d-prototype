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
        private Bullet.Factory m_BulletFactory;

        [SerializeField] private Transform m_BulletSpawnPoint;


        [Inject]
        void Construct(ShootingSettings settings, InputReader input, Bullet.Factory factory)
        {
            m_Settings = settings;
            m_Input = input;
            m_BulletFactory = factory;

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
            var vector = m_Input.InputPos - transform.position;
            var speed = Random.Range(m_Settings.bulletSpeedMin, m_Settings.bulletSpeedMax);
            transform.rotation = MathfExtention.LookAt2D(vector);
            print("Shot");
            var bullet = m_BulletFactory.Create();
            bullet.speed = speed;
            bullet.transform.rotation = transform.rotation;
            bullet.transform.position = m_BulletSpawnPoint.position;
            bullet.Push(transform.right * speed);


        }



    }

}