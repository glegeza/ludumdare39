namespace DLS.LD39.Map
{
    using System.Collections.Generic;
    using UnityEngine;

    public class TileSetData : ScriptableObject
    {
        public string ID;
        public Shader TileShader;
        public Texture TileTexture;
        public int TileWidthPixels;
        public int TileHeightPixels;
        public List<string> TileNames = new List<string>();
    }
}
