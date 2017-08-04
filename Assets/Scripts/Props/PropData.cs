namespace DLS.LD39.Props
{
    using UnityEngine;
    
    public class PropData : ScriptableObject
    {
        public string ID;
        public PropLayer Layer;
        public bool Passable;
        public bool BlocksLineOfSight;
        public Sprite Sprite;
    }
}