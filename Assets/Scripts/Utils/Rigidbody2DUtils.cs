using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Prototype
{
    public static class Rigidbody2DUtils
    {
        public static void ResetValues(this Rigidbody2D rb)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}
