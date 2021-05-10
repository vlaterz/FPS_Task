using System;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    [Serializable]
    public class ObjectPicker
    {
        private GameObject _heldObject;
        [SerializeField] private float _dropDistance = 3f;
        [SerializeField] private float _pickDistance = 3f;
        private readonly Camera _camera;

        public ObjectPicker(Camera camera) => (_camera) = (camera);

        public void Pick(GameObject target)
        {
            if ((target.transform.position - _camera.transform.position).magnitude > _pickDistance)
            {
                return;
            }
                
            _heldObject = target;
            _heldObject.SetActive(false);
        }

        public void Drop()
        {
            _heldObject.transform.position = _camera.transform.position +
                                            _camera.transform.TransformDirection(Vector3.forward) * _dropDistance;
            _heldObject.SetActive(true);
        }
    }
}
