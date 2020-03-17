using System.Collections;
using System.Collections.Generic;
using com.doubtech.mapmagic.generators;
using UnityEngine;


namespace com.doubtech.mapmagic.generators {
    [ExecuteInEditMode]
    public class MapMagicStamper : MonoBehaviour {
        private MapMagicStamperGenerator generator;
        public MapMagicStamperGenerator Generator { get {
                return generator;
            }
            set {
                generator = value;
                transform.localScale = new Vector3(generator.scale, generator.intensity, generator.scale);
                Offset = new Vector3(generator.offset.x, 0, generator.offset.y);
            }
        }

        public Vector3 Offset {
            get {
                return transform.position - Vector3.one * MapMagic.MapMagic.instance.terrainSize * transform.localScale.x / 2.0f;
            } set {
                transform.position = value + Vector3.one * MapMagic.MapMagic.instance.terrainSize * transform.localScale.x / 2.0f;
            }
        }

        private void OnEnable() {
            MapMagic.MapMagic.OnGenerateCompleted += MapMagic_OnGenerateCompleted;
        }

        private void MapMagic_OnGenerateCompleted(Terrain terrain) {

        }

        void Update() {
            MapMagic.MapMagic mm = MapMagic.MapMagic.instance;

            // Adjust transform to valid stamper coords
            transform.localScale = new Vector3(
                transform.localScale.x,
                Mathf.Clamp(transform.localScale.y, 0, 1),
                transform.localScale.x);

            float y = MapMagic.MapMagic.instance.transform.position.y + mm.terrainHeight * transform.localScale.y;
            if(y != transform.position.y) {
                transform.position = new Vector3(transform.position.x, y, transform.position.z);
            }
        }

        public void Generate() {
            MapMagic.MapMagic mm = MapMagic.MapMagic.instance;
            Vector2 offset = new Vector2(Offset.x, Offset.z);
            float scale = transform.localScale.x;
            float intensity = transform.localScale.y;


            if (null != Generator && (offset != Generator.offset || scale != Generator.scale || intensity != Generator.intensity)) {
                Debug.Log("Generating...");
                Generator.offset = offset;
                Generator.scale = scale;
                Generator.intensity = intensity;
                mm.ClearResults(Generator);
                mm.Generate(true);
            }
        }

        private void OnDrawGizmos() {
            MapMagic.MapMagic mm = MapMagic.MapMagic.instance;
            float size = mm.terrainSize;
            Vector3 cubeSize = new Vector3(
                    size * transform.localScale.x,
                    mm.terrainHeight * transform.localScale.y,
                    size * transform.localScale.z
                    );
            Vector3 position = new Vector3(
                Offset.x + cubeSize.x / 2.0f,
                MapMagic.MapMagic.instance.transform.position.y + mm.terrainHeight / 2.0f * transform.localScale.y,
                Offset.z + cubeSize.z / 2.0f
                );
            Gizmos.DrawWireCube(position, cubeSize);
        }
    }
}