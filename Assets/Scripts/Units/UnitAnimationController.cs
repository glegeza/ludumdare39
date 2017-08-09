namespace DLS.LD39.Units
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine;

    public class UnitAnimationController : GameUnitComponent
    {
        private Animator _animationController;

        public void StartWalkAnimation()
        {
            _animationController?.SetTrigger("walkingStart");
        }

        public void StartIdleAnimation()
        {
            _animationController?.SetTrigger("idleStart");
        }

        public void StartWaitAnimation()
        {
            _animationController?.SetTrigger("idleStart");
        }

        public void StartMeleeAnimation()
        {
        }

        public void StartRangedAnimation()
        {
        }

        protected override void OnInitialized(GameUnit unit)
        {
            _animationController = GetComponent<Animator>();
        }
    }
}
