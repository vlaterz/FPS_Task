using System;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    [Serializable]
    public class ObjectKicker
    {
        private readonly Camera _camera;
        [SerializeField] private float _kickRange = 2f;
        [SerializeField] private float _kickPower = 20f;

        public ObjectKicker(Camera camera) => (_camera) = (camera);

        public void Kick(GameObject target, Vector3 point)
        {
            if ((target.transform.position - _camera.transform.position).magnitude > _kickRange)
            {
                return;
            }

            var rb = target.GetComponent<Rigidbody>();
            rb.AddForceAtPosition(_camera.transform.TransformDirection(Vector3.forward) * _kickPower, point, ForceMode.Impulse);
        }
    }
}
