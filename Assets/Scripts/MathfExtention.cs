using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Prototype
{
    public static class MathfExtention
    {

        public static Quaternion LookAt2D(Vector3 vector)
        {
            vector.Normalize();
            float rot_z = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
            return Quaternion.Euler(0f, 0f, rot_z);
        }
    }
}
