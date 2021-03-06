using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype
{
    public interface IProjectile2DVisitor
    {
        void Visit(CannonBall ball);
        void Visit(GrenadeProjectile grenadeProjectile);
        void Visit(LaserProjectile laserProjectile);
        void Visit(PistolBullet pistolBullet);

    }
}
