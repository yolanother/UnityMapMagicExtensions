using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapMagic {
    [System.Serializable]
    [GeneratorMenu(menu = "Map", name = "Height Clamp", disengageable = true)]
    public class HeightClamp : Generator {
        //public override Type guiType { get { return Generator.Type.curve; } }

        public AnimationCurve curve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0, 1, 1), new Keyframe(1, 1, 1, 1) });
        public bool extended = true;
        public float min = -1;
        public float max = -1;

        public bool unitClamp;

        public Input input = new Input(InoutType.Map);//, mandatory:true);
        public Input maskIn = new Input(InoutType.Map);
        public Output output = new Output(InoutType.Map);
        public override IEnumerable<Input> Inputs() { yield return input; yield return maskIn; }
        public override IEnumerable<Output> Outputs() { yield return output; }

        public override void Generate(CoordRect rect, Chunk.Results results, Chunk.Size terrainSize, int seed, Func<float, bool> stop = null) {
            //getting input
            Matrix src = (Matrix)input.GetObject(results);

            //return on stop/disable/null input
            if (stop != null && stop(0)) return;
            if (!enabled || src == null) { output.SetObject(results, src); return; }

            //preparing output
            Matrix dst = src.Copy(null);

            float minClamp = min / MapMagic.instance.terrainHeight;
            float maxClamp = max / MapMagic.instance.terrainHeight;

            for (int i = 0; i < dst.array.Length; i++) {
                if (!unitClamp) {
                    if(dst.array[i] < minClamp) {
                        dst.array[i] = minClamp;
                    }
                    if(dst.array[i] > maxClamp) {
                        dst.array[i] = maxClamp;
                    }
                } else {
                    dst.array[i] = dst.array[i] > minClamp && dst.array[i] < maxClamp ? 1 : 0;
                }
            }

            //mask and safe borders
            if (stop != null && stop(0)) return;
            Matrix mask = (Matrix)maskIn.GetObject(results);
            if (mask != null) Matrix.Mask(src, dst, mask);

            //setting output
            if (stop != null && stop(0)) return;
            output.SetObject(results, dst);
        }

        public override void OnGUI(GeneratorsAsset gens) {
            if (min < 0) min = 0;
            if (max < 0) max = MapMagic.instance.terrainHeight;

            //inouts
            layout.Par(20); input.DrawIcon(layout, "Input"); output.DrawIcon(layout, "Output");
            layout.Par(20); maskIn.DrawIcon(layout, "Mask");
            layout.Par(5);

            layout.Field(ref unitClamp, "Unit Clamp");

            layout.margin = 20;
            layout.Label("Range:");
            //layout.Par(); layout.Label("Min:", rect:layout.Inset(0.999f)); layout.Label("Max:", rect:layout.Inset(1f));
            layout.Field(ref min, "Lower", min: 0, max: MapMagic.instance.terrainHeight);
            layout.Field(ref max, "Upper", min: 0, max: MapMagic.instance.terrainHeight);
        }
    }
}