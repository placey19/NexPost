using UnityEngine;
using UnityEngine.Rendering;

namespace Nexcide.PostProcessing {

    [VolumeComponentMenu("Nexcide/Monochrome")]
    public class Monochrome : VolumeComponentBase {

        public ClampedFloatParameter Blend = new(0.0f, 0.0f, 1.0f);
        public NoInterpColorParameter Color = new(UnityEngine.Color.white);
        public BoolParameter InvertColors = new(false);
        public NoInterpClampedFloatParameter Edge = new(0.65f, 0.0f, 1.0f);

        public override bool IsActive() => (Blend.value > 0.0f);
    }

    [PostProcessEffect(typeof(Monochrome))]
    public class MonochromeEffect : VolumeEffect {

        public override string ShaderName => "Nexcide/Monochrome";
        
        private static readonly int _blend = Shader.PropertyToID("_Blend");
        private static readonly int _color = Shader.PropertyToID("_Color");
        private static readonly int _invertColors = Shader.PropertyToID("_InvertColors");
        private static readonly int _edge = Shader.PropertyToID("_Edge");

        public override bool ConfigureMaterial(VolumeStack volumeStack, MaterialPropertyBlock material) {
            bool active = ComponentActive(volumeStack, out Monochrome component);

            if (active) {
                material.SetFloat(_blend, component.Blend.value);
                material.SetColor(_color, component.Color.value);
                material.SetFloat(_invertColors, component.InvertColors.value ? 1.0f : 0.0f);
                material.SetFloat(_edge, component.Edge.value);
            }

            return active;
        }
    }
}
