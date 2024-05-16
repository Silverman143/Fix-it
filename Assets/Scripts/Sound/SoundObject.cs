using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FixItGame
{
    public class SoundObject : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        private bool _isActive = false;


        private void FixedUpdate()
        {
            if (_isActive && !_audioSource.isPlaying)
            {
                gameObject.SetActive(false);
            }
        }

        public void PlaySound(Sound sound)
        {
            _audioSource.volume = sound._volume;
            _audioSource.pitch = sound._speed;
            _audioSource.PlayOneShot(sound._clip);
            _isActive = true;
        }
    }
}
