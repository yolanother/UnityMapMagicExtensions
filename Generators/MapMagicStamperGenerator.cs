using System.Collections.Generic;
using MapMagic;
using UnityEditor;
using UnityEngine;
using static MapMagic.Generator;

namespace com.doubtech.mapmagic.generators {
	[System.Serializable]
	[GeneratorMenu(menu = "Map", name = "Stamper", disengageable = true)]
	public class MapMagicStamperGenerator : TextureInput {
        private MapMagicStamper stamper;

        public override void OnGUI(GeneratorsAsset gens) {
            Transform mm = MapMagic.MapMagic.instance.gameObject.transform;
            stamper = mm.GetComponentInChildren<MapMagicStamper>();
            

            base.OnGUI(gens);
            if ((null == stamper || null != stamper && stamper.Generator != this) && layout.Button("Show Stamper")) {
                if (null == stamper || stamper.Generator != this) {
                    if (null != stamper) {
                        GameObject.DestroyImmediate(stamper.gameObject);
                    }
                    GameObject go = new GameObject("Stamper");
                    go.transform.parent = mm;
                    stamper = go.AddComponent<MapMagicStamper>();
                    stamper.transform.localScale = new Vector3(scale, intensity, scale);
                    stamper.Offset = new Vector3(offset.x, 0, offset.y);
                    stamper.Generator = this;
                    Selection.activeObject = stamper;
                }
            } else if (null != stamper && stamper.Generator == this && layout.Button("Hide Stamper")) {
                GameObject.DestroyImmediate(stamper.gameObject);
            }
        }
    }
}