using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Prototype
{
    public class Cannon : Gun
    {
        private CannonSettings m_Settings;
        private CannonBall.Pool m_CannonBallsPool;

        [Inject]
        void Construct(CannonSettings settings, CannonBall.Pool pool)
        {
            m_Settings = settings;
            m_CannonBallsPool = pool;

        }

        public override void Shot()
        {
            var speed = m_Settings.projectileSpeed;
            var bullet = m_CannonBallsPool.Spawn();
            //bullet.speed = speed;
            var trans = bullet.transform;
            trans.rotation = m_Trasform.rotation;
            trans.position = m_BulletSpawnPoint.position;
            bullet.Push(m_Trasform.right * speed);
        }





    }

}