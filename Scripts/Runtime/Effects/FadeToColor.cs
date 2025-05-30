using UnityEngine;
using UnityEngine.Rendering;

namespace Nexcide.PostProcessing {

    [VolumeComponentMenu("Nexcide/Color Fade")]
    public class FadeToColor : VolumeComponentBase {

        public ClampedFloatParameter Blend = new(0.0f, 0.0f, 1.0f);
        public ColorParameter Color = new(UnityEngine.Color.white, hdr: true, showAlpha: false, showEyeDropper: false);

        public override bool IsActive() => (Blend.value > 0.0f);
    }

    [PostProcessEffect(typeof(FadeToColor))]
    public class FadeToColorEffect : VolumeEffect {

        public override string ShaderName => "Nexcide/Fade To Color";

        private static readonly int _blend = Shader.PropertyToID("_Blend");
        private static readonly int _color = Shader.PropertyToID("_Color");

        public override bool ConfigureMaterial(VolumeStack volumeStack, MaterialPropertyBlock material) {
            bool active = ComponentActive(volumeStack, out FadeToColor component);

            if (active) {
                material.SetFloat(_blend, component.Blend.value);
                material.SetColor(_color, component.Color.value);
            }

            return active;
        }
    }
}
