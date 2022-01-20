
using UnityEngine;

namespace Prototype
{
    public interface IDamageable
    {
        void ApplyDamage(int damage, Vector3 pos);
    }

}
