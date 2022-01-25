using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Prototype
{
    public class Pistol : Gun
    {
        private ShootingSettings m_Settings;
        private PistolBullet.Pool m_BulletPool;

        [Inject]
        void Construct(ShootingSettings settings, PistolBullet.Pool pool)
        {
            m_Settings = settings;
            m_BulletPool = pool;

        }

        public override void Shot()
        {
            var speed = Random.Range(m_Settings.bulletSpeedMin, m_Settings.bulletSpeedMax);
            var bullet = m_BulletPool.Spawn();
            bullet.speed = speed;
            var trans = bullet.transform;
            trans.rotation = m_Trasform.rotation;
            trans.position = m_BulletSpawnPoint.position;
            bullet.Push(m_Trasform.right * speed);
        }

    }

}