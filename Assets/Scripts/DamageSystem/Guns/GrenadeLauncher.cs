using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Prototype
{
    public class GrenadeLauncher : Gun
    {
        private GrenadeLauncherSettings m_Settings;
        private GrenadeProjectile.Pool m_ProjectilePool;

        [Inject]
        void Construct(GrenadeLauncherSettings settings, GrenadeProjectile.Pool pool)
        {
            m_Settings = settings;
            m_ProjectilePool = pool;

        }

        public override void Shot()
        {
            var speed = m_Settings.projectileSpeed;
            var bullet = m_ProjectilePool.Spawn();
            //bullet.speed = speed;
            var trans = bullet.transform;
            trans.rotation = m_Trasform.rotation;
            trans.position = m_BulletSpawnPoint.position;
            bullet.Push(m_Trasform.right * speed);
        }





    }

}