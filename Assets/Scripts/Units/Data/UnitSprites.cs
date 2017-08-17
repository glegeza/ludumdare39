namespace DLS.LD39.Units.Data
{
    using DLS.LD39.Graphics;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Game Data/Units/Unit Sprites")]
    public class UnitSprites : ScriptableObject
    {
        public Sprite Sprite;
        public RuntimeAnimatorController AnimationController;
        public Explosion DeathAnimation;
    }
}
