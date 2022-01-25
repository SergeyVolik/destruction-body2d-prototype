using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Prototype
{
    public class GrenadeLauncher : Gun
    {
        private ShootingSettings m_Settings;
        private GrenadeProjectile.Pool m_ProjectilePool;

        [Inject]
        void Construct(ShootingSettings settings, GrenadeProjectile.Pool pool)
        {
            m_Settings = settings;
            m_ProjectilePool = pool;

        }

        public override void Shot()
        {
            var speed = Random.Range(m_Settings.bulletSpeedMin, m_Settings.bulletSpeedMax);
            var bullet = m_ProjectilePool.Spawn();
            //bullet.speed = speed;
            var trans = bullet.transform;
            trans.rotation = m_Trasform.rotation;
            trans.position = m_BulletSpawnPoint.position;
            bullet.Push(m_Trasform.right * speed);
        }





    }

}