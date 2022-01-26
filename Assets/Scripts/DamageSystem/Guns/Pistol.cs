using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Prototype
{
    public class Pistol : Gun
    {
        private PistolSettings m_Settings;
        private PistolBullet.Pool m_BulletPool;

        [Inject]
        void Construct(PistolSettings settings, PistolBullet.Pool pool)
        {
            m_Settings = settings;
            m_BulletPool = pool;

        }

        public override void Shot()
        {
            var speed = m_Settings.projectileSpeed;
            var bullet = m_BulletPool.Spawn();
            var trans = bullet.transform;
            trans.rotation = m_Trasform.rotation;
            trans.position = m_BulletSpawnPoint.position;
            bullet.Push(m_Trasform.right * speed);
        }

    }

}