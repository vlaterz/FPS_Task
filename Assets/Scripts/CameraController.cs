using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Assets.Scripts
{
    public class CameraController : MonoBehaviour
    {
        public bool Smooth;
        public bool ClampVerticalRotation = true;
        public float XSensitivity = 2f;
        public float YSensitivity = 2f;
        public float MinimumX = -90F;
        public float MaximumX = 90F;
        public float SmoothTime = 5f;

        private Quaternion _characterTargetRot;
        private Quaternion _cameraTargetRot;

        private Camera _camera;

        void Awake()
        {
            _camera = Camera.main;
        }

        void Start()
        {
            _characterTargetRot = transform.localRotation;
            _cameraTargetRot = _camera.transform.localRotation;
        }

        void Update()
        {
            float yRot = CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity;
            float xRot = CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;

            _characterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            _cameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            if (ClampVerticalRotation)
            {
                _cameraTargetRot = ClampRotationAroundXAxis(_cameraTargetRot);
            }
            
            if (Smooth)
            {
                transform.localRotation = Quaternion.Slerp(transform.localRotation, _characterTargetRot,
                    SmoothTime * Time.deltaTime);
                _camera.transform.localRotation = Quaternion.Slerp(_camera.transform.localRotation, _cameraTargetRot,
                    SmoothTime * Time.deltaTime);
            }
            else
            {
                transform.localRotation = _characterTargetRot;
                _camera.transform.localRotation = _cameraTargetRot;
            }
        }

        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

            angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }
    }
}
