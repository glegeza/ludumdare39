namespace DLS.LD39.Props
{
    using DLS.LD39.Graphics;
    using DLS.LD39.Map;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    class PropFactory : SingletonComponent<PropFactory>
    {
        private Dictionary<string, PropData> _propDict = new Dictionary<string, PropData>();

        public IEnumerable<string> PropIDs
        {
            get
            {
                return _propDict.Keys;
            }
        }

        public Prop BuildPropAndAddToTile(string id, Tile tile)
        {
            var obj = BuildPropObject(id);
            tile.AddProp(obj);
            obj.Position.SetTile(tile);
            ActiveUnits.Instance.UpdateVisibility();

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
            renderer.sprite = data.Sprite;
            switch (data.Layer)
            {
                case PropLayer.Wall:
                    renderer.sortingLayerName = "Wall Props";
                    break;
                case PropLayer.Floor:
                    renderer.sortingLayerName = "Floor Props";
                    break;
                case PropLayer.WallDeco:
                    renderer.sortingLayerName = "Wall Decorations";
                    break;
            }

            if (data.BlocksLineOfSight)
            {
                var collider = propObj.AddComponent<BoxCollider2D>();
                collider.offset = new Vector2(0.0f, 0.0f);
                collider.size = new Vector2(0.95f, 0.95f);
                propObj.AddComponent<LOSBlocker>();
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

            var props = Resources.LoadAll<PropData>("Props");
            foreach (var prop in props)
            {
                var hasErrors = false;
                if (String.IsNullOrEmpty(prop.ID))
                {
                    Debug.LogErrorFormat("Prop file {0} missing ID", prop.name);
                    hasErrors = true;
                }
                if (_propDict.ContainsKey(prop.ID))
                {
                    Debug.LogErrorFormat("Prop dictionary already contains prop with ID {0}", prop.ID);
                    hasErrors = true;
                }
                if (prop.Sprite == null)
                {
                    Debug.LogErrorFormat("Prop file {0} missing sprite", prop.name);
                    hasErrors = true;
                }

                if (!hasErrors)
                {
                    _propDict.Add(prop.ID, prop);
                }
                else
                {
                    Debug.LogErrorFormat("Prop file {0} had errors and was not added to prop list.", prop.name);
                }
            }
        }
    }
}
