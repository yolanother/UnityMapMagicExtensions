using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace com.doubtech.mapmagic.generators {
    [CustomEditor(typeof(MapMagicStamper))]
    public class StamperGeneratorEditor : Editor {
        private MapMagicStamper stamper;

        private void OnEnable() {
            stamper = (MapMagicStamper)target;
        }

        void OnSceneGUI() {
            switch(Event.current.type) {
                case EventType.MouseUp:
                    stamper.Generate();
                    break;
            }
        }


        public override void OnInspectorGUI() {
            MapMagic.MapMagic mm = MapMagic.MapMagic.instance;

            int index = 0;
            var names = new List<string>();
            var stampers = new List<MapMagicStamperGenerator>();
            foreach(var gen in mm.gens.list) {
                if (gen is MapMagicStamperGenerator) {
                    MapMagicStamperGenerator mgs = gen as MapMagicStamperGenerator;
                    if (gen == stamper.Generator) index = names.Count;
                    names.Add(mgs.texture.name);
                    stampers.Add(mgs);
                }
            }

            if (stampers.Count > 0) {
                index = EditorGUILayout.Popup(index, names.ToArray());

                if(stampers[index] != stamper.Generator) {
                    stamper.Generator = stampers[index];
                }

                if (null != stamper.Generator) {
                    stamper.Generator.offset = EditorGUILayout.Vector2Field("Offset", stamper.Generator.offset);
                    stamper.Generator.intensity = EditorGUILayout.FloatField("Height", stamper.Generator.intensity * mm.terrainHeight) / (float)mm.terrainHeight;
                }
            }
        }
    }
}