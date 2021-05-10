using Assets.Scripts.Classes;
using UnityEngine;

namespace Assets.Scripts
{
    public class ObjectsInterractionController : MonoBehaviour
    {
        [SerializeField] private ObjectGrabber _grabber;
        [SerializeField] private ObjectKicker _kicker;
        [SerializeField] private ObjectPicker _picker;

        void Awake()
        {
            _grabber = new ObjectGrabber(Camera.main, transform);
            _kicker = new ObjectKicker(Camera.main);
            _picker = new ObjectPicker(Camera.main);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _picker.Drop();
            }

            if (!Input.GetKeyDown(KeyCode.E))
            {
                return;
            }

            if (_grabber.HeldObject != null)
            {
                _grabber.DropObject();
                return;
            }

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit))
            {
                switch (hit.collider.tag)
                {
                    case "Grab":
                        //Т.к. я решил не наследовать обработчики от MonoBehaviour, корутина MoveObject() из граббера выполняется прямо здесь (+ избегаю this)
                        StartCoroutine(_grabber.GrabObject(hit.collider.gameObject));
                        break;
                    case "Kick":
                        _kicker.Kick(hit.collider.gameObject, hit.point);
                        break;
                    case "Pick":
                        _picker.Pick(hit.collider.gameObject);
                        break;
                    default:
                        return;
                }
            }
        }
    }
}
