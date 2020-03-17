using System.Collections;
using System.Collections.Generic;
using com.doubtech.mapmagic.generators;
using UnityEngine;


namespace com.doubtech.mapmagic.generators {
    [ExecuteInEditMode]
    public class MapMagicStamper : MonoBehaviour {
        public MapMagicStamperGenerator Generator { get; internal set; }

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
            Vector2 offset = new Vector2(Offset.x, Offset.z);
            float scale = transform.localScale.x;
            float intensity = transform.localScale.y;

            // Adjust transform to valid stamper coords
            transform.localScale = new Vector3(
                transform.localScale.x,
                Mathf.Clamp(transform.localScale.y, 0, 1),
                transform.localScale.x);

            if (transform.hasChanged) {
                Generator.offset = offset;
                Generator.scale = scale;
                Generator.intensity = intensity;
                MapMagic.MapMagic.instance.ClearResults();
                MapMagic.MapMagic.instance.Generate(true);
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