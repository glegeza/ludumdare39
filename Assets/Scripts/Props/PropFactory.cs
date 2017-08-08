namespace DLS.LD39.Props
{
    using DLS.LD39.Graphics;
    using DLS.LD39.Map;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    class PropFactory : SingletonComponent<PropFactory>
    {
        public List<PropData> Props = new List<PropData>();
        public Vector3 Scaling = new Vector3(1, 1, 1);

        private Dictionary<string, PropData> _propDict = new Dictionary<string, PropData>();

        public Prop BuildPropAndAddToTile(string id, Tile tile)
        {
            var obj = BuildPropObject(id);
            tile.AddProp(obj);
            obj.Position.SetTile(tile);

            return obj;
        }

        public Prop BuildPropObject(string id)
        {
            var data = GetPropByID(id);
            if (data == null)
            {
                Debug.LogErrorFormat("Failed to build prop object for {0}", id);
            }
            var propObj = new GameObject(String.Format("Prop: {0}", id));

            var renderer = propObj.AddComponent<SpriteRenderer>();
            propObj.transform.localScale = Scaling;
            renderer.sprite = data.Sprite;
            switch (data.Layer)
            {
                case PropLayer.Wall:
                    renderer.sortingLayerName = "Wall Props";
                    break;
                case PropLayer.Floor:
                    renderer.sortingLayerName = "Floor Props";
                    break;
            }


            propObj.AddComponent<SortByY>();
            propObj.AddComponent<TilePosition>();
            var propComp = propObj.AddComponent<Prop>();
            propComp.Initialize(data);

            return propComp;
        }

        public PropData GetPropByID(string id)
        {
            var lowerID = id.ToLower();
            if (!_propDict.ContainsKey(lowerID))
            {
                Debug.LogErrorFormat("Unknown prop ID {0}", lowerID);
                return null;
            }
            return _propDict[id];
        }

        protected override void Awake()
        {
            base.Awake();

            foreach (var prop in Props)
            {
                var id = prop.ID.ToLower();
                if (_propDict.ContainsKey(id))
                {
                    Debug.LogErrorFormat("Duplicate prop ID {0} in prop {1}", prop.ID, prop.name);
                }
                _propDict.Add(id, prop);
            }
        }
    }
}
