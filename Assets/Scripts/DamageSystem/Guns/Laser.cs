using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Prototype
{
    public class Laser : Gun
    {
        private ShootingSettings m_Settings;
        private LaserProjectile.Pool m_LaserProjectilesPool;

        [Inject]
        void Construct(ShootingSettings settings, LaserProjectile.Pool pool)
        {
            m_Settings = settings;
            m_LaserProjectilesPool = pool;

        }

        public override void Shot()
        {
            var bullet = m_LaserProjectilesPool.Spawn();
            var trans = bullet.transform;
            trans.rotation = m_Trasform.rotation;
            trans.position = m_BulletSpawnPoint.position;
        }





    }

}