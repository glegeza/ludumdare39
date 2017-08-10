namespace DLS.LD39.Units
{
    using System;
    using UnityEngine;

    public class UnitAnimationController : GameUnitComponent
    {
        public const string IdleTag = "idle";
        public const string WalkTag = "walk";
        public const string MeleeTag = "melee";

        public const string WalkingTrigger = "walkingStart";
        public const string IdleTrigger = "idleStart";
        public const string MeleeTrigger = "attackMelee";

        private Animator _animationController;
        private bool _animating = false;
        private bool _transitioning = false;
        private string _targetAnimation;

        public event EventHandler<EventArgs> ReturnedToIdle;

        public void StartWalkAnimation()
        {
            _animationController?.SetTrigger(WalkingTrigger);
            _transitioning = true;
            _targetAnimation = WalkTag;
            if (_animationController == null)
            {
                OnAnimationComplete();
            }
        }

        public void StartIdleAnimation()
        {
            _animationController?.SetTrigger(IdleTrigger);
            if (_transitioning || _animating)
            {
                OnAnimationComplete();
            }
        }

        public void StartWaitAnimation()
        {
            _animationController?.SetTrigger(IdleTrigger);
            _transitioning = false;
            _animating = false;
        }

        public void StartMeleeAnimation()
        {
            _animationController?.SetTrigger(MeleeTrigger);
            _targetAnimation = MeleeTag;
            _transitioning = true;
            if (_animationController == null)
            {
                OnAnimationComplete();
            }
        }

        public void StartRangedAnimation()
        {
        }

        protected override void OnInitialized(GameUnit unit)
        {
            _animationController = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_animationController == null)
            {
                return;
            }

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
            ReturnedToIdle?.Invoke(this, EventArgs.Empty);
        }

        private void OnTransitionComplete()
        {
            _transitioning = false;
            _animating = true;
        }
    }
}
