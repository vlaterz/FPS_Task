using System;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class StepController
    {
        private float _stepCycle = 0f;
        public float StepInterval = 1f;

        public bool IsTimeToMakeAStep(float speed, Vector3 characterVelocity)
        {
            if (characterVelocity.sqrMagnitude > 0)
            {
                _stepCycle += (characterVelocity.magnitude + speed) * Time.fixedDeltaTime;
            }

            if (_stepCycle <= StepInterval)
            {
                return false;
            }

            _stepCycle = 0;

            return true;
        }
    }
}
