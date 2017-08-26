namespace DLS.LD39.Units
{
    using System;
    using JetBrains.Annotations;
    using UnityEngine;
    using Utility;

    public delegate void AnimationCallback();

    public class UnitAnimationController : GameUnitComponent
    {
        public const string IdleTag = "idle";
        public const string WalkTag = "walk";
        public const string MeleeTag = "melee";

        public const string WalkingTrigger = "walkingStart";
        public const string IdleTrigger = "idleStart";
        public const string MeleeTrigger = "attackMelee";

        private Animator _animationController;
        private bool _animating;
        private bool _transitioning;
        private string _targetAnimation;
        private AnimationCallback _actionCallback;

        public event EventHandler<EventArgs> ReturnedToIdle;

        public void StartWalkAnimation(AnimationCallback onAnimComplete=null)
        {
            SetTrigger(WalkingTrigger);
            _transitioning = true;
            _targetAnimation = WalkTag;
            _actionCallback = onAnimComplete;
            if (_animationController == null)
            {
                OnAnimationComplete();
            }
        }

        public void StartIdleAnimation()
        {
            SetTrigger(IdleTrigger);
            if (_transitioning || _animating)
            {
                OnAnimationComplete();
            }
        }

        public void StartWaitAnimation()
        {
            SetTrigger(IdleTrigger);
            _transitioning = false;
            _animating = false;
        }

        public void StartMeleeAnimation(AnimationCallback onAnimComplete = null)
        {
            SetTrigger(MeleeTrigger);
            _targetAnimation = MeleeTag;
            _transitioning = true;
            _actionCallback = onAnimComplete;
            if (_animationController == null)
            {
                OnAnimationComplete();
            }
        }

        public void StartRangedAnimation(AnimationCallback onAnimComplete = null)
        {
            SetTrigger(MeleeTrigger);
            _targetAnimation = MeleeTag;
            _transitioning = true;
            _actionCallback = onAnimComplete;
            if (_animationController == null)
            {
                OnAnimationComplete();
            }
        }

        protected override void OnInitialized(GameUnit unit)
        {
            _animationController = GetComponent<Animator>();
            if (_animationController != null)
            {
                _animationController.Play("Idle", 0, UnityEngine.Random.Range(0f, 1f));
            }
        }

        [UsedImplicitly]
        private void Update()
        {
            if (_animationController == null)
            {
                HandleStatic();
            }
            else
            {
                HandleAnimated();
            }
        }

        private void SetTrigger(string trigger)
        {
            if (_animationController != null)
            {
                _animationController.SetTrigger(trigger);
            }
        }

        private void HandleStatic()
        {
            if (_animating)
            {
                OnAnimationComplete();
            }
            else if (_transitioning)
            {
                OnTransitionComplete();
            }
        }

        private void HandleAnimated()
        {
            if (_animating && _animationController.GetCurrentAnimatorStateInfo(0).IsTag(IdleTag))
            {
                OnAnimationComplete();
            }

            if (_transitioning && _animationController.GetCurrentAnimatorStateInfo(0).IsTag(_targetAnimation))
            {
                OnTransitionComplete();
            }
        }

        private void OnAnimationComplete()
        {
            _transitioning = false;
            _animating = false;
            if (_actionCallback != null)
            {
                _actionCallback();
                _actionCallback = null;
            }
            ReturnedToIdle.SafeRaiseEvent(this);
        }

        private void OnTransitionComplete()
        {
            _transitioning = false;
            _animating = true;
        }
    }
}
