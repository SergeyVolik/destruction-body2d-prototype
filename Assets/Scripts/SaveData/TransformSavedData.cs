using UnityEngine;

namespace Prototype
{
    public struct TransformSavedData
    {
        public Transform parent;
        public Vector3 localPosition;
        public Quaternion localRotation;

        public void ResetValues(Transform transform)
        {
            transform.parent = parent;
            transform.localPosition = localPosition;
            transform.localRotation = localRotation;
        }
    }
}
