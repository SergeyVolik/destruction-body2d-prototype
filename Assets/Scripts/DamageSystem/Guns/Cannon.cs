using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Prototype
{
    public class Cannon : Gun
    {
        private ShootingSettings m_Settings;
        private CannonBall.Pool m_CannonBallsPool;

        [Inject]
        void Construct(ShootingSettings settings, CannonBall.Pool pool)
        {
            m_Settings = settings;
            m_CannonBallsPool = pool;

        }

        public override void Shot()
        {
            var speed = Random.Range(m_Settings.bulletSpeedMin, m_Settings.bulletSpeedMax);
            var bullet = m_CannonBallsPool.Spawn();
            //bullet.speed = speed;
            var trans = bullet.transform;
            trans.rotation = m_Trasform.rotation;
            trans.position = m_BulletSpawnPoint.position;
            bullet.Push(m_Trasform.right * speed);
        }





    }

}