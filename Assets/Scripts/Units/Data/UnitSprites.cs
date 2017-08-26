namespace DLS.LD39.Units.Data
{
    using Graphics;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Game Data/Units/Unit Sprites")]
    public class UnitSprites : ScriptableObject
    {
        public Sprite Sprite;
        public Sprite IconSprite;
        public RuntimeAnimatorController AnimationController;
        public Explosion DeathAnimation;
    }
}
