﻿namespace DLS.LD39.Interface
{
    using UnityEngine;
    using UnityEngine.UI;

    public class FloatingCombatText : MonoBehaviour
    {
        private Animator _animator;
        private Text _text;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _text = GetComponentInChildren<Text>();
        }

        private void Start()
        {
            var clipInfo = _animator.GetCurrentAnimatorClipInfo(0)[0];
            Destroy(gameObject, clipInfo.clip.length);
        }

        public void SetText(string text)
        {
            if (_text == null)
            {
                throw new System.Exception("what");
            }
            _text.text = text;
        }
    }
}
