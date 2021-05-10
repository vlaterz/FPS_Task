using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    [Serializable]
    public class ObjectGrabber
    {
        private readonly Camera _camera;
        private Transform _previousParentOfHeldObject;
        private Transform _objectHolder;
        [HideInInspector] public Rigidbody HeldObject;
        [SerializeField] private float _moveForce = 250f;
        [SerializeField] private float _pickupRange = 5f;
        [SerializeField] private float _dragDistance = 3f;

        public ObjectGrabber(Camera camera, Transform holderParent) => (_camera, _objectHolder) = (camera, holderParent);

        public IEnumerator GrabObject(GameObject target)
        {
            HeldObject = target.GetComponent<Rigidbody>();
            if (HeldObject == null)
            {
                yield break;
            }

            if ((target.transform.position - _objectHolder.transform.position).magnitude > _pickupRange)
            {
                yield break;
            }

            _previousParentOfHeldObject = HeldObject.transform.parent;

            HeldObject.useGravity = false;
            HeldObject.drag = 10;
            HeldObject.transform.SetParent(_objectHolder);

            yield return MoveObject();
        }


        public void DropObject()
        {
            HeldObject.transform.SetParent(_previousParentOfHeldObject);
            HeldObject.useGravity = true;
            HeldObject.drag = 1f;
            HeldObject.transform.parent = _previousParentOfHeldObject;
            HeldObject = null;
        }


        private IEnumerator MoveObject()
        {
            while (HeldObject != null)
            {
                Vector3 magneticPoint = _camera.transform.position + _camera.transform.forward * _dragDistance;
                
                if (Vector3.Distance(HeldObject.transform.position, magneticPoint) > 0.1f)
                {
                    Vector3 moveDirection = (magneticPoint - HeldObject.transform.position);
                    HeldObject.AddForce(moveDirection * _moveForce);
                }
                else
                {
                    HeldObject.velocity = Vector3.zero;
                }
                yield return null;
            }
        }
    }
}
