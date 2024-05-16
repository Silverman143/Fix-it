using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FixItGame
{
    public class SoundsPool : MonoBehaviour
    {
        [SerializeField] private SoundObject _soundObjectPref;
        [SerializeField] private int _amount;
        [SerializeField] private bool _autoExpand;

        public PoolMono<SoundObject> _pool;

        private void Awake()
        {
            _pool = new PoolMono<SoundObject>(_soundObjectPref, _amount, transform, _autoExpand);
        }


    }
}
