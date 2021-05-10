using UnityEngine;

namespace Assets.Scripts
{
    public class CharacterSoundController : MonoBehaviour
    {
        [SerializeField] private AudioClip[] _footstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField] private AudioClip _jumpSound;           // the sound played when character leaves the ground.
        [SerializeField] private AudioClip _landSound;           // the sound played when character touches back on ground.
        private AudioSource _audioSource;

        void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            MovementController.StepEvent.AddListener(PlayFootStepAudio);
            MovementController.LandEvent.AddListener(PlayLandingSound);
            MovementController.JumpEvent.AddListener(PlayJumpSound);
        }

        private void PlayFootStepAudio()
        {
            int n = Random.Range(1, _footstepSounds.Length);
            _audioSource.clip = _footstepSounds[n];
            _audioSource.PlayOneShot(_audioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            _footstepSounds[n] = _footstepSounds[0];
            _footstepSounds[0] = _audioSource.clip;
        }

        private void PlayJumpSound()
        {
            _audioSource.clip = _jumpSound;
            _audioSource.Play();
        }

        private void PlayLandingSound()
        {
            _audioSource.clip = _landSound;
            _audioSource.Play();
        }
    }
}
