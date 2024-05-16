using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FixItGame
{
    public class SoundController : MonoBehaviour
    {
        private SoundsPool _soundsPool;

        //SOunds
        [SerializeField] private Sound _hitSound;

        private void Awake()
        {
            _soundsPool = GetComponent<SoundsPool>();
        }
        private void OnEnable()
        {
            GlobalEvents.OnPuzzleHits += PlayHit;
        }

        private void OnDisable()
        {
            GlobalEvents.OnPuzzleHits -= PlayHit;
        }

        private void PlayHit()
        {
            _soundsPool._pool.GetFreeElement().PlaySound(_hitSound);
        }
    }
}
